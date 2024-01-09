@echo off

REM Set arguments
set arg1=%1

REM Change directory
cd %arg1%

REM Gather any Visual Studio installations
vswhere