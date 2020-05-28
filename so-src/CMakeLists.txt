cmake_minimum_required(VERSION 3.5)
project(NativeLinuxInterop C)

set(CMAKE_C_STANDARD 99)

add_library(so_src SHARED library.c library.h gettaskstat.c)