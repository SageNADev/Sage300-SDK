REM Build Models project for entered module

REM Set arguments
set arg1=%1
set arg2=%2
set arg3=%3
set arg4=%4
set arg5=%5
set arg6=%6
set arg7=%7
set arg8=%8
set arg9=%9

REM Change directory
cd %arg9%

REM Build Models Project
dotnet build %arg1% %arg2% %arg3% %arg4% %arg5% %arg6% %arg7% %arg8% > NUL
