#!/usr/bin/env bash
pushd ../../version
source build-version.sh;
popd
export Version="${Version}${NUPKG_PACKAGE_SUFFIX}"
Say "VERSION to build: [$Version]. Building"

rm -rf bin/Release || true
dotnet build -c Release /p:"PackageVersion=$Version" /p:"Version=$Version"
monodis --assembly bin/Release/net40/Universe.LinuxTaskStats.dll
cp -f Universe.LinuxTaskStats.nuspec bin/Release

Say "VERSION to build: [$Version]. Removing auto packaged package"
pushd bin/Release
7z a nupkgs-prev-packed.7z "*nupkg" -sdel || true


Say "VERSION to build: [$Version]. Align FileSystem"
runtimes="linux-arm  linux-arm64  linux-armel  linux-mips64el  linux-powerpc  linux-ppc64el  linux-x64  linux-x86  rhel.6-x64"
targets="netcoreapp1.0  netcoreapp3.1  netstandard1.1  netstandard2.0"
targets="netcoreapp1.0  netstandard1.1  netstandard2.0" # netcoreapp3.1 is redundant

for t in $targets; do
    for rt in $runtimes; do
        mkdir -p ref/$t
        cp -f $t/*.dll ref/$t/
        mkdir -p runtimes/$rt/native runtimes/$rt/lib/$t mkdir contentFiles/any/any
        cp -f ../../../../runtimes/$rt/*.so runtimes/$rt/native/ 
        cp -f $t/* runtimes/$rt/lib/$t/
        # cp -f ../../../../runtimes/$rt/libNativeLinuxInterop.so net40/libNativeLinuxInterop.so-$rt
        
        mkdir -p content/net40/native-libs
        cp ../../../../runtimes/$rt/libNativeLinuxInterop.so content/net40/native-libs/libNativeLinuxInterop.so-$rt
        cp ../../../../runtimes/$rt/libNativeLinuxInterop.so contentFiles/any/libNativeLinuxInterop.so-$rt
        cp ../../content/* content/net40/native-libs/
    done
done
find runtimes -name '*.deps.json' | xargs rm


Say "VERSION to build: [$Version]. Nu-Packaging"
nuget pack Universe.LinuxTaskStats.nuspec -Version "$Version"

nupkg_full_name="$(pwd)/$(ls *.nupkg | head -1)"
Say "Full Path: [$nupkg_full_name]"

popd

