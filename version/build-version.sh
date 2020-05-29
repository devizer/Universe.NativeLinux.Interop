#!/usr/bin/env bash
version_build=$(set TZ=GMT; git log -n 999999 --date=raw --pretty=format:"%cd" | wc -l)
version_main=$(cat version-main)
version_suffix=$(cat version-suffix)
export Version="${version_main}.${version_build}${version_suffix}"


