#!/usr/bin/env bash
gcc -shared -fPIC -o libNativeLinuxInterop.so gettaskstat.c
gcc -o /tmp/show-taskstat-info show-taskstat-info.c
/tmp/show-taskstat-info
