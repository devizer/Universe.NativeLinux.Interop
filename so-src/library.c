#include "library.h"

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


int hello(void) {
    return 0;
}

#include <stddef.h>

#define PRINT_FIELD(__s__, __f__) { printf("    __f__: %ld", (long) offsetof(struct __s__, __f__));  }


void print_taskstats_structure() {
    struct taskstats t;
    printf("   version: %ld", (long) offsetof(struct taskstats, version));
    // PRINT_FIELD(taskstats, version);
}

// int GetByProcess()
