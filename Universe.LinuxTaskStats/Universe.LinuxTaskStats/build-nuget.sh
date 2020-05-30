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
Say "VERSION to build: [$Version]. Nu-Packaging"
nuget pack Universe.LinuxTaskStats.nuspec -Version "$Version"

nupkg_full_name="$(pwd)/$(ls *.nupkg | head -1)"
Say "Full Path: [$nupkg_full_name]"

popd

