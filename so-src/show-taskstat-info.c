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


void print_taskstats_structure() {
    struct taskstats t;
    printf("\ntaskstats internals:\n")
    printf("         version: %ld", (long) offsetof(struct taskstats, version));
    printf("     ac_exitcode: %ld", (long) offsetof(struct taskstats, ac_exitcode));
    // PRINT_FIELD(taskstats, version);
}

void main()
{
    printf("sizeof(int): %d; sizeof (struct taskstat): %d; TASKSTATS_VERSION: %d; TS_COMM_LEN: %d; \n",
                    sizeof(int),
                    sizeof (struct taskstats),
                    TASKSTATS_VERSION,
                    TS_COMM_LEN, sizeof(int));

    print_taskstats_structure();
}
