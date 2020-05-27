cmake_minimum_required(VERSION 3.7)
project(so_src C)

set(CMAKE_C_STANDARD 99)

add_library(so_src SHARED library.c library.h getdelays.c)