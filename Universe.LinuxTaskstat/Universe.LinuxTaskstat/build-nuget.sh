#!/usr/bin/env bash
pushd ../../version
source build-version.sh;
popd
Say "VERSION to build: $Version"

rm -rf bin/Release || true
dotnet build -c Release /p:"PackageVersion=$Version" /p:"Version=$Version"
monodis --assembly bin/Release/net40/Universe.LinuxTaskstat.dll
cp -f Universe.LinuxTaskstat.nuspec bin/Release
exit;
pushd bin/Release
mkdir -p .prev
mv -f *.nupkg .prev
nuget pack Universe.LinuxTaskstat.nuspec -Version "$Version"
popd

