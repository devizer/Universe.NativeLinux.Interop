#!/usr/bin/env bash
rm -rf bin/Release || true
dotnet build -c Release
cp -f Universe.LinuxTaskstat.nuspec bin/Release
pushd bin/Release
mkdir -p .prev
mv -f *.nupkg .prev
nuget pack Universe.LinuxTaskstat.nuspec
popd

