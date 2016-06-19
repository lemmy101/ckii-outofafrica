#!/bin/bash
mcs src/*.cs src/*/*.cs -debug+ -optimize+ -pkg:dotnet -out:OutOfAfrica.exe
