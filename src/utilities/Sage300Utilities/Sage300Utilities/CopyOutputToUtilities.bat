@echo off
REM *****************************************************************************************************
REM *                                                                                                   *
REM *   This program is used to copy Sage300Utilities.exe and it's                                      *
REM *   dependencies into the following folder:                                                         *
REM *                                                                                                   *
REM *   Sage300WebSDK\Bin\Utilities\                                                                    *
REM *                                                                                                   *
REM *****************************************************************************************************

setlocal
REM This is the folder where this batch file is located.
SET ProjectRoot=%~dp0
REM echo ProjectRoot = %ProjectRoot%
REM GOTO END

SET PROGRAMNAME=Sage300Utilities.exe
SET RELEASEFOLDER=%ProjectRoot%bin\release
SET SDKROOT=..\..\..\..\..\..\
SET DEST1=%SDKROOT%Bin\Utilities\

REM Initialize the source paths
SET SAGE300UTILITIES_EXE=%RELEASEFOLDER%\%PROGRAMNAME%
SET SAGE300UTILITIES_EXE_CONFIG=%RELEASEFOLDER%\Sage300Utilities.exe.config
SET LOG4NET_CONFIG=%RELEASEFOLDER%\Log4net.config
SET LOG4NET_DLL=%RELEASEFOLDER%\Log4net.dll
SET LOG4NET_XML=%RELEASEFOLDER%\Log4net.xml


REM Copy the files to the destinations
copy %SAGE300UTILITIES_EXE% %DEST1%
copy %SAGE300UTILITIES_EXE_CONFIG% %DEST1%
copy %LOG4NET_CONFIG% %DEST1%
copy %LOG4NET_DLL% %DEST1%
copy %LOG4NET_XML% %DEST1%

REM Let's just see what was copied :)
echo %PROGRAMNAME% and dependencies copied to %DEST1% 
dir %DEST1%

GOTO END
:END
@echo.
@echo.
@echo.




