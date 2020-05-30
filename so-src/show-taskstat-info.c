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
#include <stddef.h>

#define PRINT_FIELD(__s__, __f__) { printf("    %20s: %ld\n", #__f__, (long) offsetof(struct __s__, __f__));  }

#define PRINT_TASKSTATS_FIELF(__f__) { PRINT_FIELD(taskstats, __f__); }


void print_taskstats_structure() {
    struct taskstats t;
    printf("\n-->! taskstats internals:\n");
    printf("                 version: %ld\n", (long) offsetof(struct taskstats, version));
    printf("             ac_exitcode: %ld\n", (long) offsetof(struct taskstats, ac_exitcode));
    PRINT_FIELD(taskstats, ac_exitcode);
    PRINT_FIELD(taskstats, ac_flag);
    PRINT_TASKSTATS_FIELF(ac_nice);
    PRINT_TASKSTATS_FIELF(cpu_count);
    PRINT_TASKSTATS_FIELF(cpu_delay_total);


}

void main()
{
    printf("sizeof(int): %ld; sizeof (struct taskstat): %ld; TASKSTATS_VERSION: %ld; TS_COMM_LEN: %ld; \n",
                    sizeof(int),
                    sizeof (struct taskstats),
                    TASKSTATS_VERSION,
                    TS_COMM_LEN, sizeof(int));

    // print_taskstats_structure();
}
