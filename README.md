### taskstats for .NET Core and Mono
Per-task and per-process statistics from the Linux kernel for .NET Core and Mono for x64, arm, and arm64. The metrics include CPU Usage, counters and delays caused by IO, swap-in, memory reclaim, and memory trashing. 

Supports a range of taskstat versions available in the wild starting from 7th on RHEL 6. In comparison to the [Universe.CpuUsage](https://github.com/devizer/Universe.CpuUsage) package it is linux only, but it provides much more details on CPU usage and IO activity by thread/process. 

Targets everywhere: .Net Standard 1.1+, Net Core 1.0-3.1, Mono 3.12+

### Precompiled native shared objects  
Minimum libc.so/glibc.so version requirement depends on architecture
- 2.15 for **x86_64**, **armhf**, **i386** (Ubuntu 12.04 LTS Precise) 
- 2.19 for **arm64**, **ppc64el** (Ubuntu 14.04 LTS Trusty) 
- 2.13 for **armel** & **powerpc** (Debian 7 Wheezy)
- 2.24 for **mips64el**: (Debian 9 Stretch)
- 2.12 for **RHEL/CentOS 6 x84_64**  

In any case the native shared library can by compiled on the target linux.





