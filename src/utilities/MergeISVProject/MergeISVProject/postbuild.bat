@echo off
SET PROGRAMNAME=MergeISVProject.exe

REM *********************************************************************
REM *                                                                   *
REM *   This program is used to copy MergeISVProject.exe and it's       *
REM *   dependencies into the Sage300SDK\Bin\Utilities\ directory       *
REM *                                                                   *
REM *********************************************************************

setlocal
SET myDir=%~dp0
SET PROGRAMNAME=MergeISVProject.exe
SET SOURCEFOLDER=%myDir%bin\release
SET SDKROOT=..\..\..\..\..\..\
SET BIN_UTILITIES=%SDKROOT%Bin\Utilities\

REM Initialize the source paths
set MERGEISVPROJECTEXE=%SOURCEFOLDER%\%PROGRAMNAME%
set ANTLR3DLL=%SOURCEFOLDER%\Antlr3.Runtime.dll
set NEWTONSOFTJSONDLL=%SOURCEFOLDER%\Newtonsoft.Json.dll
set WEBGREASEDLL=%SOURCEFOLDER%\WebGrease.dll
set WEBGREASEEXE=%SOURCEFOLDER%\WG.EXE
REM dir %SDKROOT%bin\utilities

REM Copy the files to the destination
copy %MERGEISVPROJECTEXE% %BIN_UTILITIES%
copy %ANTLR3DLL% %BIN_UTILITIES%
copy %NEWTONSOFTJSONDLL% %BIN_UTILITIES%
copy %WEBGREASEDLL% %BIN_UTILITIES%
copy %WEBGREASEEXE% %BIN_UTILITIES%

REM Let's just see what was copied :)
echo %PROGRAMNAME% and dependencies copied to %BIN_UTILITIES%
dir %BIN_UTILITIES%
GOTO END
:END
@echo.
@echo.
@echo.




