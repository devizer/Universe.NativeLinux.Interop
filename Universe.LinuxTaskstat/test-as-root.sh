#!/usr/bin/env bash
pushd Universe.LinuxTaskstat.Tests
dotnet publish -c Release -f netcoreapp3.1 -o /tmp/taskstat
popd

pushd /tmp/taskstat
sudo dotnet test Universe.LinuxTaskstat.Tests.dll -v d
popd