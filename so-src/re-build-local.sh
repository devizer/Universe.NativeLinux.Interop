#!/usr/bin/env bash
gcc -O2 -shared -fPIC -o libNativeLinuxInterop.so gettaskstat.c
gcc -O2 -o /tmp/show-taskstat-info show-taskstat-info.c
/tmp/show-taskstat-info

pushd taskstats-internals >/dev/null
bash show-taskstats-structure.sh
popd >/dev/null
