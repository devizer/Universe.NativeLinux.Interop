#!/usr/bin/env bash
gcc -o delays delays.c
./delays
sudo ./delays -p 1 -d -i
