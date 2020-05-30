### taskstat for .NET Core and Mono
Per-task and per-process statistics from the kernel for .NET Core and Mono. Metrics are CPU Usage, counters and delays caused by IO, swap-in, memory reclaim and trashing.

In comparision to the [Universe.CpuUsage](https://github.com/devizer/Universe.CpuUsage) package it is linux only. Targets everywhere: .Net Standard 1.1+, Net Core 1.0-3.1, Mono 3.12+ 

### Precompiled native shared objects  
Minimum libc.so/glibc.so version requirement depends on architecture
- 2.15 for **x86_64**, **armhf**, **i386** (Ubuntu 12.04 LTS Precise) 
- 2.19 for **arm64**, **ppc64el** (Ubuntu 14.04 LTS Trusty) 
- 2.13 for **armel** & **powerpc** (Debian 7 Wheezy)
- 2.24 for **mips64el**: (Debian 9 Stretch)
- 2.12 for **x64** on Red Hat 6 

In any case the native shared library can by compiled on the target linux.
 
  

Supports range of taskstat versions from RLEL/CentOS 6 to debian sid.



