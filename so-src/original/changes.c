/* getdelays.c
 *
 * Utility to get per-pid and per-tgid delay accounting statistics
 * Also illustrates usage of the taskstats interface
 *
 * Copyright (C) Shailabh Nagar, IBM Corp. 2005
 * Copyright (C) Balbir Singh, IBM Corp. 2006
 * Copyright (c) Jay Lan, SGI. 2006
 *
 * Compile with
 *	gcc -I/usr/src/linux/include getdelays.c -o getdelays
 */

#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <unistd.h>
#include <poll.h>
#include <string.h>
#include <fcntl.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/socket.h>
#include <signal.h>

#include <linux/genetlink.h>
#include <linux/taskstats.h>
#include <linux/cgroupstats.h>
#include <syscall.h>

/*
 * Generic macros for dealing with netlink sockets. Might be duplicated
 * elsewhere. It is recommended that commercial grade applications use
 * libnl or libnetlink and use the interfaces provided by the library
 */
#define GENLMSG_DATA(glh)	((void *)(NLMSG_DATA(glh) + GENL_HDRLEN))
#define GENLMSG_PAYLOAD(glh)	(NLMSG_PAYLOAD(glh, 0) - GENL_HDRLEN)
#define NLA_DATA(na)		((void *)((char*)(na) + NLA_HDRLEN))
#define NLA_PAYLOAD(len)	(len - NLA_HDRLEN)

#define err(code, fmt, arg...)			\
	do {					\
		fprintf(stderr, fmt, ##arg);	\
		exit(code);			\
	} while (0)

int done;
int rcvbufsz;
char name[100];
int dbg = 0;
int print_delays = 1;
int print_io_accounting = 1;
int print_task_context_switch_counts = 1;
__u64 stime, utime;

#define PRINTF(fmt, arg...) {			\
	    if (dbg || debug) {				\
		printf(fmt, ##arg);		\
	    }					\
	}

/* Maximum size of response requested or message sent */
#define MAX_MSG_SIZE	4096
/* Maximum number of cpus expected to be specified in a cpumask */
#define MAX_CPUS	256

struct msgtemplate {
    struct nlmsghdr n;
    struct genlmsghdr g;
    char buf[MAX_MSG_SIZE];
};


char cpumask[100+6*MAX_CPUS];

/*
 * Create a raw netlink socket and bind
 */
static int create_nl_socket(int protocol)
{
    int fd;
    struct sockaddr_nl local;

    fd = socket(AF_NETLINK, SOCK_RAW, protocol);
    if (fd < 0)
        return -1;

    if (rcvbufsz)
        if (setsockopt(fd, SOL_SOCKET, SO_RCVBUF,
                       &rcvbufsz, sizeof(rcvbufsz)) < 0) {
            fprintf(stderr, "Unable to set socket rcv buf size "
                            "to %d\n",
                    rcvbufsz);
            return -1;
        }

    memset(&local, 0, sizeof(local));
    local.nl_family = AF_NETLINK;

    if (bind(fd, (struct sockaddr *) &local, sizeof(local)) < 0)
        goto error;

    return fd;
    error:
    close(fd);
    return -1;
}


static int send_cmd(int sd, __u16 nlmsg_type, __u32 nlmsg_pid,
                    __u8 genl_cmd, __u16 nla_type,
                    void *nla_data, int nla_len)
{
    struct nlattr *na;
    struct sockaddr_nl nladdr;
    int r, buflen;
    char *buf;

    struct msgtemplate msg;

    msg.n.nlmsg_len = NLMSG_LENGTH(GENL_HDRLEN);
    msg.n.nlmsg_type = nlmsg_type;
    msg.n.nlmsg_flags = NLM_F_REQUEST;
    msg.n.nlmsg_seq = 0;
    msg.n.nlmsg_pid = nlmsg_pid;
    msg.g.cmd = genl_cmd;
    msg.g.version = 0x1;
    na = (struct nlattr *) GENLMSG_DATA(&msg);
    na->nla_type = nla_type;
    na->nla_len = nla_len + 1 + NLA_HDRLEN;
    memcpy(NLA_DATA(na), nla_data, nla_len);
    msg.n.nlmsg_len += NLMSG_ALIGN(na->nla_len);

    buf = (char *) &msg;
    buflen = msg.n.nlmsg_len ;
    memset(&nladdr, 0, sizeof(nladdr));
    nladdr.nl_family = AF_NETLINK;
    while ((r = sendto(sd, buf, buflen, 0, (struct sockaddr *) &nladdr,
                       sizeof(nladdr))) < buflen) {
        if (r > 0) {
            buf += r;
            buflen -= r;
        } else if (errno != EAGAIN)
            return -1;
    }
    return 0;
}


/*
 * Probe the controller in genetlink to find the family id
 * for the TASKSTATS family
 */
static int get_family_id(int sd)
{
    struct {
        struct nlmsghdr n;
        struct genlmsghdr g;
        char buf[256];
    } ans;

    int id = 0, rc;
    struct nlattr *na;
    int rep_len;

    strcpy(name, TASKSTATS_GENL_NAME);
    rc = send_cmd(sd, GENL_ID_CTRL, getpid(), CTRL_CMD_GETFAMILY,
                  CTRL_ATTR_FAMILY_NAME, (void *)name,
                  strlen(TASKSTATS_GENL_NAME)+1);

    rep_len = recv(sd, &ans, sizeof(ans), 0);
    if (ans.n.nlmsg_type == NLMSG_ERROR ||
        (rep_len < 0) || !NLMSG_OK((&ans.n), rep_len))
        return 0;

    na = (struct nlattr *) GENLMSG_DATA(&ans);
    na = (struct nlattr *) ((char *) na + NLA_ALIGN(na->nla_len));
    if (na->nla_type == CTRL_ATTR_FAMILY_ID) {
        id = *(__u16 *) NLA_DATA(na);
    }
    return id;
}


extern __u32 get_tid() {
    pid_t tid = syscall(__NR_gettid);
    return tid;
}

extern __u32 get_pid() {
    pid_t pid = getpid();
    return pid;
}

// Get taskstats Size By Version
__u32 get_ts_size_bv(__u16 version)
{
    if (version > 10) return 352; // future version size should not ne less then prev
    if (version == 10) return 352;
    if (version == 9) return 344;
    if (version == 7 || version == 8) return 328;

    // CLARIFY using RHEL 5 for version 6?
    return 328;
}


void smart_copy_taskstats(struct taskstats *t, void *to)
{
    memcpy(to, t, get_ts_size_bv(t->version));
}

// pid and tid are 32 bit integers on both 32-bit and 64-bit OS
extern int get_taskstats(__s32 argPid, __s32 argTid, void *targetTaskStat, __s32 targetTaskStatLength, __s32 debug)
{
    if (debug) dbg = 1;
    int c, rc, rep_len, aggr_len, len2;
    int cmd_type = TASKSTATS_CMD_ATTR_UNSPEC;
    __u16 id;
    __u32 mypid;

    struct nlattr *na;
    int nl_sd = -1;
    int len = 0;
    pid_t tid = 0;
    pid_t rtid = 0;

    int fd = 0;
    int count = 0;
    int write_file = 0;
    int maskset = 0;
    char *logfile = NULL;
    int loop = 0;
    int containerset = 0;
    char containerpath[1024];
    int cfd = 0;

    struct msgtemplate msg;
    int returnError = 0;


    if (argPid) {
        cmd_type = TASKSTATS_CMD_ATTR_PID;
        tid = argPid;
    }
    else if (argTid) {
        cmd_type = TASKSTATS_CMD_ATTR_TGID;
        tid = argTid;
    }
    else {
        returnError = 1; goto done;
    }

    if ((nl_sd = create_nl_socket(NETLINK_GENERIC)) < 0) {
        // TODO: return error
        // err(1, "Error creating Netlink socket\n");
        if (debug) fprintf(stderr, "Error creating Netlink Socket 'create_nl_socket(NETLINK_GENERIC)'\n");
        returnError = 2; goto done;
    }



    mypid = getpid();
    id = get_family_id(nl_sd);
    if (!id) {
        if (debug) fprintf(stderr, "Error getting family id 'get_family_id(nl_sd)', errno %d\n", errno);
        returnError = 3;
        goto err;
    }
    PRINTF("family id %d\n", id);

    if (tid) {
        rc = send_cmd(nl_sd, id, mypid, TASKSTATS_CMD_GET,
                      cmd_type, &tid, sizeof(__u32));
        PRINTF("Sent pid/tgid, retval %d\n", rc);
        if (rc < 0) {
            if (debug) fprintf(stderr, "Error sending tid/tgid cmd 'send_cmd(...)'\n");
            returnError = 4;
            goto done;
        }
    }

    do {
        int i;

        rep_len = recv(nl_sd, &msg, sizeof(msg), 0);
        PRINTF("received %d bytes\n", rep_len);

        if (rep_len < 0) {
            if (debug) fprintf(stderr, "nonfatal reply error: errno %d, still waiting for reply\n", errno);
            continue;
        }
        if (msg.n.nlmsg_type == NLMSG_ERROR ||
            !NLMSG_OK((&msg.n), rep_len)) {
            struct nlmsgerr *err = NLMSG_DATA(&msg);
            if (debug) fprintf(stderr, "Fatal Reply Error. NLMSG_ERROR Recieved. errno %d\n", err->error);
            returnError=8;
            goto done;
        }

        PRINTF("nlmsghdr size=%zu, nlmsg_len=%d, rep_len=%d\n",
               sizeof(struct nlmsghdr), msg.n.nlmsg_len, rep_len);


        rep_len = GENLMSG_PAYLOAD(&msg.n);

        na = (struct nlattr *) GENLMSG_DATA(&msg);
        len = 0;
        i = 0;
        while (len < rep_len) {
            len += NLA_ALIGN(na->nla_len);
            switch (na->nla_type) {
                case TASKSTATS_TYPE_AGGR_TGID:
                    /* Fall through */
                case TASKSTATS_TYPE_AGGR_PID:
                    aggr_len = NLA_PAYLOAD(na->nla_len);
                    len2 = 0;
                    /* For nested attributes, na follows */
                    na = (struct nlattr *) NLA_DATA(na);
                    done = 0;
                    while (len2 < aggr_len) {
                        switch (na->nla_type) {
                            case TASKSTATS_TYPE_PID:
                                rtid = *(int *) NLA_DATA(na);
                                if (print_delays && debug)
                                    printf("PID\t%d\n", rtid);
                                break;
                            case TASKSTATS_TYPE_TGID:
                                rtid = *(int *) NLA_DATA(na);
                                if (print_delays && debug)
                                    printf("TGID\t%d\n", rtid);
                                break;
                            case TASKSTATS_TYPE_STATS:
                                count++;
                                done = 1;
                                struct taskstats *taskStat = (struct taskstats *) NLA_DATA(na);
                                smart_copy_taskstats(taskStat, targetTaskStat);
                                if (!loop)
                                    goto done;
                                break;
                            default:
                                if (debug) fprintf(stderr, "Warning! Unknown nested"
                                                           " nla_type %d\n",
                                                   na->nla_type);
                                break;
                        }
                        len2 += NLA_ALIGN(na->nla_len);
                        na = (struct nlattr *) ((char *) na + len2);
                    }
                    break;

                case CGROUPSTATS_TYPE_CGROUP_STATS:
                    // SKIP: print_cgroupstats(NLA_DATA(na));
                    break;
                default:
                    fprintf(stderr, "Unknown nla_type %d\n",
                            na->nla_type);
                    break;
            }
            na = (struct nlattr *) (GENLMSG_DATA(&msg) + len);
        }
    } while (loop);

    done:
    err:
    close(nl_sd);

    return returnError;
}


extern __u64 get_taskstats_version()
{
    // size that exceeds any version
    struct taskstats *t = malloc(1024);
    int isOk = get_taskstats(getpid(), 0, (void*)t, 1024, 0);
    int32_t ret = 0;
    if (isOk == 0) {
        ret = t->version;
    }

    free(t);
    return (__u64)(((__u64)isOk) << 32) | ((__u64) ret);
}

