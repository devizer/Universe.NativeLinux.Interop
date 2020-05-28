#!/usr/bin/env bash
gcc -shared -o libNativeLinuxInterop.o -fpic getdelays.c

# gcc -c -Wall -Werror -fpic getdelays.c
gcc -shared -o libNativeLinuxInterop.so libNativeLinuxInterop.o