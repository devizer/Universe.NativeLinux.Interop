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

echo '
APT::Install-Recommends "1";
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

apt-get update;
# build-essential
apt-get install libc6-dev gcc -y;
yum install gcc -y;

# BUILD
gcc -o show-taskstat-info show-taskstat-info.c;
ls -la /gettaskstat.c;

gcc -shared -fPIC -o libNativeLinuxInterop.so gettaskstat.c;
ls -la libNativeLinuxInterop.so
