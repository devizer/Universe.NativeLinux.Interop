#!/usr/bin/env bash

rm -f libNativeLinuxInterop.so || true
rm -f /tmp/show-taskstat-info || true

set -e
Say "Build get_taskstats.c ...."
gcc -O2 -shared -fPIC -Werror=implicit-function-declaration -o libNativeLinuxInterop.so get_taskstats.c

Say "Build show-taskstat-info.c ..."
gcc -O2 -o /tmp/show-taskstat-info show-taskstat-info.c
/tmp/show-taskstat-info

pushd taskstats-internals >/dev/null
bash -e show-taskstats-structure.sh
popd >/dev/null

set +e
