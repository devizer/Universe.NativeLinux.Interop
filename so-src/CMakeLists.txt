cmake_minimum_required(VERSION 3.16)
project(libTaskStats C)
# set(CMAKE_CONFIGURATION_TYPES "Release" CACHE STRING "" FORCE)

set(CMAKE_C_STANDARD 11)

add_library(libTaskStats
        get_taskstats.c
        show-taskstat-info.c
        original/delays.c
)
# target_link_libraries(libTaskStats)


