#!/usr/bin/env bash

# Path to test project
PROJECT_DIR="src/DepthCharts.Tests"
PROJECT_NAME="DepthCharts.Tests.csproj"

if [[ "$1" == "--watch" ]]; then
    shift # remove --watch arg
    (
       cd "$PROJECT_DIR"
       dotnet watch test "$@"
    )
else
    dotnet test "$PROJECT_DIR/$PROJECT_NAME" "$@"
fi
