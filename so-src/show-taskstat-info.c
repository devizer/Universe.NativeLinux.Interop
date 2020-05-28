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

void main()
{
    printf("sizeof(int): %d; sizeof (struct taskstat): %d; TASKSTATS_VERSION: %d; TS_COMM_LEN: %d; \n",
                    sizeof(int),
                    sizeof (struct taskstats),
                    TASKSTATS_VERSION,
                    TS_COMM_LEN, sizeof(int));
}
