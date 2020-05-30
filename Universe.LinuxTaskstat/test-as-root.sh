#!/usr/bin/env bash
pushd Universe.LinuxTaskStats.Tests
dotnet publish -c Debug -f netcoreapp3.1 -o /tmp/taskstats
popd

pushd /tmp/taskstats
sudo dotnet test Universe.LinuxTaskStats.Tests.dll -v d
popd
