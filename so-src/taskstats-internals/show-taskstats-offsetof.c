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

#define PRINT_FIELD(__s__, __f__) { printf("    %26s: %ld\n", #__f__, (long) offsetof(struct __s__, __f__));  }
#define PRINT_TASKSTATS_FIELF(__f__) { PRINT_FIELD(taskstats, __f__); }

void main()
{
    PRINT_FIELD(taskstats, $TASKSTATS_FIELD_NAME);
}
