@echo off
:: ---------------------------------------------------------------------------------------
:: File: CreateTemplateZipFiles.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Create zip archives of the following:
::            - Items
::
:: Usage Example:
::          CreateTemplateZipFiles.bat
::
:: ---------------------------------------------------------------------------------------

setlocal

set zipDestinationPath=.
if not [%1]==[] (
  set zipDestinationPath=%1
)
echo zipDestinationPath=%zipDestinationPath%

call :Zip Items

goto :EOF

:: ---------------------------------------------------------------------------------------
:: Function Definitions
:: ---------------------------------------------------------------------------------------

:Zip
  setlocal
  @echo on
  set zipFolderName=%~1
  set zipDir="%~dp0\%zipFolderName%"
  powershell.exe -ExecutionPolicy Bypass -file "%~dp0\pszip.ps1" ^
    "%zipDir%" "%zipDestinationPath%\%zipFolderName%.zip"
  @echo off
  goto :EOF

:: End of file
