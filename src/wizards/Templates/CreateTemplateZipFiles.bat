@echo off
setlocal

set zipDestinationPath=.
if not [%1]==[] (
  set zipDestinationPath=%1
)
echo zipDestinationPath=%zipDestinationPath%

call :Zip BusinessRepository
call :Zip Interfaces
call :Zip Models
call :Zip Resources
call :Zip Services
call :Zip Web
call :Zip Sage300SolutionTemplate

goto :EOF

REM ***********************************
REM *** Function Definitions

:Zip
  setlocal
  @echo on
  set zipFolderName=%~1
  set zipDir="%~dp0\%zipFolderName%"
  powershell.exe -ExecutionPolicy Bypass -file "%~dp0\pszip.ps1" ^
    "%zipDir%" "%zipDestinationPath%\%zipFolderName%.zip"
  @echo off
  goto :EOF
