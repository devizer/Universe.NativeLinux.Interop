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
deb http://archive.debian.org/debian wheezy main
' > /etc/apt/sources.list
    fi


apt-get update;
apt-get install gcc -yqq;
yum install gcc -y;

# BUILD
gcc -o show-taskstat-info show-taskstat-info.c;
ls -la /gettaskstat.c;

gcc -shared -fPIC -o libNativeLinuxInterop.so gettaskstat.c;
ls -la libNativeLinuxInterop.so
