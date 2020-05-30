#!/usr/bin/env bash
version_build=$(set TZ=GMT; git log -n 999999 --date=raw --pretty=format:"%cd" | wc -l)
version_main=$(cat version-main)
# on the build server we use it to find the nupkg artifact
tf_build=${BUILD_BUILDID}; if [[ $tf_build -gt 0 ]]; then tf_build=".${tf_build}"  
# version_suffix=$(cat version-suffix)
export Version="${version_main}.${version_build}${tf_build}${version_suffix}"


