@echo off

REM Set arguments
set arg1=%1

REM Change directory
cd %arg1%

REM Gather any .NET SDK installations
dotnet --info