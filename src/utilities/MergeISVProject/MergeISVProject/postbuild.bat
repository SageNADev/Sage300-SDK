@echo off
SET PROGRAMNAME=MergeISVProject.exe

REM *********************************************************************
REM *                                                                   *
REM *   This program is used to copy MergeISVProject.exe and it's       *
REM *   dependencies into the following folders:                        *
REM *                                                                   *
REM *   Sage300WebSDK\Bin\Utilities\                                    *
REM *   Sage300WebSDK\src\Wizards\Templates\Web\                        *
REM *                                                                   *
REM *********************************************************************

setlocal
SET myDir=%~dp0
SET PROGRAMNAME=MergeISVProject.exe
SET SOURCEFOLDER=%myDir%bin\release
SET SDKROOT=..\..\..\..\..\..\
SET BIN_UTILITIES=%SDKROOT%Bin\Utilities\
SET WIZARDS_TEMPLATES_WEB=%SDKROOT%src\Wizards\Templates\Web\

REM Initialize the source paths
set MERGEISVPROJECTEXE=%SOURCEFOLDER%\%PROGRAMNAME%
set ANTLR3DLL=%SOURCEFOLDER%\Antlr3.Runtime.dll
set NEWTONSOFTJSONDLL=%SOURCEFOLDER%\Newtonsoft.Json.dll
set WEBGREASEDLL=%SOURCEFOLDER%\WebGrease.dll
set WEBGREASEEXE=%SOURCEFOLDER%\WG.EXE


REM Copy the files to the destinations

REM MergeISVProject.exe
copy %MERGEISVPROJECTEXE% %BIN_UTILITIES%
copy %MERGEISVPROJECTEXE% %WIZARDS_TEMPLATES_WEB%

REM Antlr3.Runtime.dll
copy %ANTLR3DLL% %BIN_UTILITIES%
copy %ANTLR3DLL% %WIZARDS_TEMPLATES_WEB%

REM Newtonsoft.Json.dll
copy %NEWTONSOFTJSONDLL% %BIN_UTILITIES%
copy %NEWTONSOFTJSONDLL% %WIZARDS_TEMPLATES_WEB%

REM WebGrease.dll
copy %WEBGREASEDLL% %BIN_UTILITIES%
copy %WEBGREASEDLL% %WIZARDS_TEMPLATES_WEB%

REM WG.EXE
copy %WEBGREASEEXE% %BIN_UTILITIES%
copy %WEBGREASEEXE% %WIZARDS_TEMPLATES_WEB%


REM Let's just see what was copied :)
echo %PROGRAMNAME% and dependencies copied to %BIN_UTILITIES% 
dir %BIN_UTILITIES%

echo %PROGRAMNAME% and dependencies copied to %WIZARDS_TEMPLATES_WEB% 
dir %WIZARDS_TEMPLATES_WEB%

GOTO END
:END
@echo.
@echo.
@echo.




