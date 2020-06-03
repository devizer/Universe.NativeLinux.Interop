#!/usr/bin/env bash
work=/transient-builds/taskstats-tests
sudo mkdir -p $work
sudo chown -R $(whoami) $work
pushd Universe.LinuxTaskStats.Tests
dotnet publish -c Debug -f netcoreapp3.1 -o $work
popd

pushd $work
sudo dotnet test Universe.LinuxTaskStats.Tests.dll -v d 2>&1 | tee full-test-log.tmp
popd
