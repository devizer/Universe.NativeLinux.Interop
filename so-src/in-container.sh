#!/usr/bin/env bash
source /etc/os-release
  if [[ $ID == debian ]] && [[ $VERSION_ID == 8 ]]; then
    rm -f /etc/apt/sources.list.d/backports* || true
    echo '
deb http://deb.debian.org/debian jessie main
deb http://security.debian.org jessie/updates main
' > /etc/apt/sources.list
    fi

  if [[ $ID == debian ]] && [[ $VERSION_ID == 7 ]]; then
    rm -f /etc/apt/sources.list.d/backports* || true
    echo '
deb http://archive.debian.org/debian wheezy main contrib
# deb http://security.debian.org/debian-security wheezy/updates main
deb http://archive.debian.org/debian-security wheezy/updates main
# deb http://archive.debian.org/debian wheezy-updates main

' > /etc/apt/sources.list
    fi

# NO Install-Recommends
echo '
APT::Install-Recommends "0";
APT::NeverAutoRemove:: ".*";

Acquire::Check-Valid-Until "0";
APT::Get::Assume-Yes "true";
APT::Get::AllowUnauthenticated "true";
Acquire::AllowInsecureRepositories "1";
Acquire::AllowDowngradeToInsecureRepositories "1";

Acquire::CompressionTypes::Order { "gz"; };
APT::Compressor::gzip::CompressArg:: "-1";
APT::Compressor::xz::CompressArg:: "-1";
APT::Compressor::bzip2::CompressArg:: "-1";
APT::Compressor::lzma::CompressArg:: "-1";
' > /etc/apt/apt.conf.d/99Z_Custom

export DEBIAN_FRONTEND=noninteractive

if [[ $(command -v apt-get 2>/dev/null) != "" ]]; then
    apt-get update -qq || apt-get update -qq || apt-get update
    # build-essential
    apt-get install --no-install-recommends libc6-dev gcc gettext -y -q || apt-get install --no-install-recommends libc6-dev gcc gettext -y -q || apt-get install --no-install-recommends libc6-dev gcc gettext -y
fi

if [[ $(command -v yum 2>/dev/null) != "" ]]; then
cat <<-'EOF' > /etc/yum.repos.d/CentOS-Base.repo
[C6.10-base]
name=CentOS-6.10 - Base
baseurl=http://vault.centos.org/6.10/os/$basearch/
gpgcheck=1
gpgkey=file:///etc/pki/rpm-gpg/RPM-GPG-KEY-CentOS-6
enabled=1
metadata_expire=never

[C6.10-updates]
name=CentOS-6.10 - Updates
baseurl=http://vault.centos.org/6.10/updates/$basearch/
gpgcheck=1
gpgkey=file:///etc/pki/rpm-gpg/RPM-GPG-KEY-CentOS-6
enabled=1
metadata_expire=never

[C6.10-extras]
name=CentOS-6.10 - Extras
baseurl=http://vault.centos.org/6.10/extras/$basearch/
gpgcheck=1
gpgkey=file:///etc/pki/rpm-gpg/RPM-GPG-KEY-CentOS-6
enabled=1
metadata_expire=never

[C6.10-contrib]
name=CentOS-6.10 - Contrib
baseurl=http://vault.centos.org/6.10/contrib/$basearch/
gpgcheck=1
gpgkey=file:///etc/pki/rpm-gpg/RPM-GPG-KEY-CentOS-6
enabled=0
metadata_expire=never

[C6.10-centosplus]
name=CentOS-6.10 - CentOSPlus
baseurl=http://vault.centos.org/6.10/centosplus/$basearch/
gpgcheck=1
gpgkey=file:///etc/pki/rpm-gpg/RPM-GPG-KEY-CentOS-6
enabled=0
metadata_expire=never
EOF
    yum makecache >/dev/null 2>&1 || yum makecache >/dev/null 2>&1 || yum makecache
    yum install gcc gettext -y || yum install gcc gettext -y || yum install gcc gettext -y;
fi

# set -e
# BUILD
gcc -O2 -o show-taskstat-info show-taskstat-info.c;
ls -la /get_taskstats.c;

gcc -O2 -shared -fPIC -o libNativeLinuxInterop.so get_taskstats.c;
ls -la libNativeLinuxInterop.so
