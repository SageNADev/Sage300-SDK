@echo off
SET PROGRAMNAME=MergeISVProject.exe

REM *****************************************************************************************************
REM *                                                                                                   *
REM *   This program is used to copy MergeISVProject.exe and it's                                       *
REM *   dependencies into the following folders:                                                        *
REM *                                                                                                   *
REM *   Sage300WebSDK\Bin\Utilities\                                                                    *
REM *   Sage300WebSDK\src\Wizards\Templates\Web\                                                        *
REM *   Sage300WebSDK\src\wizards\Customization\Sage300UICustomizationSolution\ProjectTemplates\Web\    *
REM *                                                                                                   *
REM *****************************************************************************************************

setlocal
REM This is the folder where this batch file is located.
SET ProjectRoot=%~dp0
REM echo ProjectRoot = %ProjectRoot%
REM GOTO END

SET PROGRAMNAME=MergeISVProject.exe
SET RELEASEFOLDER=%ProjectRoot%bin\release
SET SDKROOT=..\..\..\..\..\..\
SET DEST1=%SDKROOT%Bin\Utilities\
SET DEST2=%SDKROOT%src\Wizards\Templates\Web\
SET DEST3=%SDKROOT%src\wizards\Customization\Sage300UICustomizationSolution\ProjectTemplates\Web\

REM Initialize the source paths
set MERGEISVPROJECTEXE=%RELEASEFOLDER%\%PROGRAMNAME%
set ANTLR3DLL=%ProjectRoot%\Antlr3.Runtime.dll
set NEWTONSOFTJSONDLL=%ProjectRoot%\Newtonsoft.Json.dll
set WEBGREASEDLL=%ProjectRoot%\WebGrease.dll
set WEBGREASEEXE=%ProjectRoot%\WG.EXE


REM Copy the files to the destinations

REM MergeISVProject.exe
copy %MERGEISVPROJECTEXE% %DEST1%
copy %MERGEISVPROJECTEXE% %DEST2%
copy %MERGEISVPROJECTEXE% %DEST3%

REM Antlr3.Runtime.dll
copy %ANTLR3DLL% %DEST1%
copy %ANTLR3DLL% %DEST2%
copy %ANTLR3DLL% %DEST3%

REM Newtonsoft.Json.dll
copy %NEWTONSOFTJSONDLL% %DEST1%
copy %NEWTONSOFTJSONDLL% %DEST2%
copy %NEWTONSOFTJSONDLL% %DEST3%

REM WebGrease.dll
copy %WEBGREASEDLL% %DEST1%
copy %WEBGREASEDLL% %DEST2%
copy %WEBGREASEDLL% %DEST3%

REM WG.EXE
copy %WEBGREASEEXE% %DEST1%
copy %WEBGREASEEXE% %DEST2%
copy %WEBGREASEEXE% %DEST3%


REM Let's just see what was copied :)
echo %PROGRAMNAME% and dependencies copied to %DEST1% 
dir %DEST1%

echo %PROGRAMNAME% and dependencies copied to %DEST2% 
dir %DEST2%

echo %PROGRAMNAME% and dependencies copied to %DEST3% 
dir %DEST3%

GOTO END
:END
@echo.
@echo.
@echo.




