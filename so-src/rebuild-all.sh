#!/usr/bin/env bash
script=https://raw.githubusercontent.com/devizer/test-and-build/master/install-build-tools-bundle.sh; (wget -q -nv --no-check-certificate -O - $script 2>/dev/null || curl -ksSL $script) | bash

export DEBIAN_FRONTEND=noninteractive
if [[ "$(command -v qemu-arm-static)" == "" || "$(command -v toilet)" == "" ]]; then 
  try-and-retry sudo apt-get update
  smart-apt-install -qq qemu-user-static toilet 
fi
try-and-retry docker pull multiarch/qemu-user-static:register
docker run --rm --privileged multiarch/qemu-user-static:register --reset

rm -f runtimes/missed.log
counter=0
errors=0;

# set -e
# nuget build will fail later in case of error

function build() {
  image=$1
  tag=$2
  nuget_arch=$3
  counter=$((counter+1))
  err=0;

  name="temp-builder-${tag}"
  echo ""
  echo "${counter} NAME: $name"
  docker rm -f $name >/dev/null 2>&1 || true
  cmd="docker pull ${image}:${tag} >/dev/null 2>&1"
  try-and-retry eval "$cmd"
  docker run -d --name $name --rm "${image}:${tag}" bash -c "while true; do sleep 999; done"
  for src in *.c in-container.sh taskstats-internals/* ; do
    name_only=$(basename $src)
    echo COPYING $src
    docker cp "$src" "$name:/$name_only"
  done

  docker exec -t $name bash in-container.sh
  mkdir -p runtimes/$tag;
  docker cp $name:/libNativeLinuxInterop.so runtimes/$tag/libNativeLinuxInterop.so || err=1
  lddVer="$(docker exec -t $name ldd --version | head -1)"
  printf "\n${image}:${tag}\n"
  toilet -f term -F border "$lddVer" | tee runtimes/$tag/versions.log
  docker exec -t $name ./show-taskstat-info | tee -a runtimes/$tag/versions.log
  echo "ldd libNativeLinuxInterop.so:" | tee -a runtimes/$tag/versions.log
  docker exec -t $name ldd libNativeLinuxInterop.so | tee -a runtimes/$tag/versions.log
  cmd='echo "Machine: $(uname -m)"; echo "Processor: $(uname -p)"'
  docker exec -t $name sh -c "$cmd" | tee -a runtimes/$tag/versions.log

  # show-taskstats-structure.sh
  docker exec -t $name bash show-taskstats-structure.sh | tee -a runtimes/$tag/versions.log || err=1


  
  echo "";

  # check is empty
  pushd runtimes/$tag >/dev/null
  size=$(du . -d 0 | awk '{print $1}')
  popd>/dev/null
  if [[ "$size" -lt 10 ]]; then # KILO-Bytes
    echo "Deleting runtimes/$tag SIZE is $size"
    # rm -rf runtimes/$tag
    echo $tag >> runtimes/missed.log
  else
    docker cp $name:/usr/include/linux/taskstats.h runtimes/$tag/taskstats.h
  fi
  
  if [[ -n "$TF_BUILD" ]]; then
    echo Deleting IMAGE ${image}:${tag}
    nohup bash -c "docker stop $name; sleep 2; try-and-retry docker rmi -f ${image}:${tag}" >/dev/null &
    # cmd="docker rmi -f ${image}:${tag}"
    # try-and-retry eval "$cmd"
  else
    docker stop $name >/dev/null 2>&1
  fi
  
  if [[ "$err" != "0" ]]; then errors=$((errors+1)); fi

}

echo 'Release uses:
deploy_to_runtimes rhel.6-x64     6
deploy_to_runtimes linux-arm      armhf-precise
deploy_to_runtimes linux-x64      amd64-precise
deploy_to_runtimes linux-x86      i386-precise
deploy_to_runtimes linux-arm64    arm64-trusty
deploy_to_runtimes linux-ppc64el  ppc64el-trusty
deploy_to_runtimes linux-powerpc  powerpc-wheezy
deploy_to_runtimes linux-armel    armel-wheezy
deploy_to_runtimes linux-mips64el mips64el-stretch
'
# for taskstats versions
# build fedora 32  # v10
# build ubuntu focal # v9

# for release
build multiarch/ubuntu-debootstrap amd64-precise 
build centos 6 linux-rhel.6
build multiarch/ubuntu-debootstrap armhf-precise 
build multiarch/ubuntu-debootstrap i386-precise 
build multiarch/ubuntu-debootstrap arm64-trusty
build multiarch/ubuntu-debootstrap ppc64el-trusty
build multiarch/debian-debootstrap powerpc-wheezy
build multiarch/debian-debootstrap armel-wheezy
build multiarch/debian-debootstrap mips64el-stretch

# exit $errors;
exit; 

build debian sid linux-x64

build fedora 27 linux
build fedora 28 linux
build fedora 29 linux

# ancient
build centos 6 linux-rhel.6
# latest
build fedora 33 linux-rhel.6
# popular
build ubuntu bionic linux-x64
# exit;

for dver in wheezy jessie stretch buster bullseye; do
  build multiarch/debian-debootstrap mips-${dver} linux-s390x # jessie only
  build multiarch/debian-debootstrap armel-${dver} linux-armel
  build multiarch/debian-debootstrap armhf-${dver} linux-arm
  build multiarch/debian-debootstrap arm64-${dver} linux-arm64
  build multiarch/debian-debootstrap amd64-${dver} linux-x64
  build multiarch/debian-debootstrap i386-${dver} linux-x86
  build multiarch/debian-debootstrap powerpc-${dver} linux-powerpc
  build multiarch/debian-debootstrap ppc64el-${dver} linux-ppc64el
  build multiarch/debian-debootstrap mips64el-${dver} linux-mips64el
  build multiarch/debian-debootstrap mipsel-${dver} linux-mipsel
  build multiarch/debian-debootstrap mips-${dver} linux-mips
done

build centos 7 linux-rhel.6
build centos 8 linux-rhel.6
build fedora 26
build fedora 30

for uver in precise trusty xenial bionic focal; do
  build multiarch/ubuntu-debootstrap powerpc-${uver} linux-powerpc
  build multiarch/ubuntu-debootstrap ppc64el-${uver} linux-ppc64el
  build multiarch/ubuntu-debootstrap armhf-${uver} linux-arm
  build multiarch/ubuntu-debootstrap arm64-${uver} linux-arm64
  build multiarch/ubuntu-debootstrap amd64-${uver} linux-x64
  build multiarch/ubuntu-debootstrap i386-${uver} linux-x86
done

echo 'tags: multiarch/debian-debootstrap

amd64-wheezy
i386-wheezy
armel-wheezy
armhf-wheezy
mipsel-wheezy
mips-wheezy
powerpc-wheezy

amd64-jessie
arm64-jessie
armel-jessie
armhf-jessie
i386-jessie
mipsel-jessie
mips-jessie
powerpc-jessie
ppc64el-jessie
s390x-jessie

*** multiarch/debian-debootstrap
amd64
amd64-artful
amd64-bionic
amd64-focal
amd64-precise
amd64-trusty
amd64-vivid
amd64-wily
amd64-xenial
amd64-yakkety
amd64-zesty
arm64
arm64-bionic
arm64-focal
arm64-trusty
arm64-vivid
arm64-wily
arm64-xenial
arm64-yakkety
arm64-zesty
armhf
armhf-bionic
armhf-focal
armhf-precise
armhf-trusty
armhf-vivid
armhf-wily
armhf-xenial
armhf-yakkety
armhf-zesty
i386
i386-artful
i386-bionic
i386-precise
i386-trusty
i386-vivid
i386-wily
i386-xenial
i386-yakkety
i386-zesty
powerpc
powerpc-precise
powerpc-trusty
powerpc-vivid
powerpc-wily
powerpc-xenial
powerpc-yakkety
ppc64el
ppc64el-bionic
ppc64el-focal
ppc64el-trusty
ppc64el-vivid
ppc64el-wily
ppc64el-xenial
ppc64el-yakkety
ppc64el-zesty


' > /dev/null


echo 'nuget folders: runtimes/{arch}/native/lib
linux-arm
linux-arm64
linux-armel
linux-x64
linux-x86
osx
win-x64
win-x86
' > /dev/null

# 2.24 - stretch, 2.23 - xenial, trusty: 2.19
