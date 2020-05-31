#!/usr/bin/env bash

rm -f libNativeLinuxInterop.so
rm -f /tmp/show-taskstat-info

set -e
Say "Build gettaskstat.c ...."
gcc -O2 -shared -fPIC -Werror=implicit-function-declaration -o libNativeLinuxInterop.so gettaskstat.c

Say "Build show-taskstat-info.c ..."
gcc -O2 -o /tmp/show-taskstat-info show-taskstat-info.c
/tmp/show-taskstat-info
set +e


pushd taskstats-internals >/dev/null
bash show-taskstats-structure.sh
popd >/dev/null
