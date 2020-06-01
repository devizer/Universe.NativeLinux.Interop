#!/usr/bin/env bash
# https://github.com/NuGet/Home/issues/6645
# https://github.com/Apollo3zehn/EtherCAT.NET/blob/1ecc3c751736e9526543b0bd8415e3c0597aed3a/src/SOEM.PInvoke/SOEM.PInvoke.csproj#L15-L28
# https://docs.microsoft.com/en-us/dotnet/core/tools/csproj#includeassets-excludeassets-and-privateassets

# https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-items?view=vs-2019
# в андроид
https://android.googlesource.com/platform/system/extras/+/master/iotop/taskstats.cpp#126
set -e

lib=libNativeLinuxInterop.so

function deploy_to_runtimes () {
  rid=$1
  src_dir=$2
  echo RID: $rid
  full_target=../runtimes/$rid
  mkdir -p $full_target
  cp -f runtimes/$src_dir/$lib $full_target/$lib
}

deploy_to_runtimes rhel.6-x64     6
deploy_to_runtimes linux-arm      armhf-precise
deploy_to_runtimes linux-x64      amd64-precise
deploy_to_runtimes linux-x86      i386-precise
deploy_to_runtimes linux-arm64    arm64-trusty
deploy_to_runtimes linux-ppc64el  ppc64el-trusty
deploy_to_runtimes linux-powerpc  powerpc-wheezy
deploy_to_runtimes linux-armel    armel-wheezy
deploy_to_runtimes linux-mips64el mips64el-stretch

echo "DONE"

