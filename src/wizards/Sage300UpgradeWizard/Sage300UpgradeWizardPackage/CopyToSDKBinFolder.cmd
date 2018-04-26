@echo off
cls
setlocal

:: ┌───────────────────────────────────────────────────────────────────────────────────────────┐
:: │ Change to Unicode codepage so we can display extended characters                          │
:: └───────────────────────────────────────────────────────────────────────────────────────────┘
for /f "tokens=2 delims=:." %%x in ('chcp') do set CodePage=%%x
chcp 65001>nul

:: ┌───────────────────────────────────────────────────────────────────────────────────────────┐
:: │ Intializations                                                                            │
:: └───────────────────────────────────────────────────────────────────────────────────────────┘
set currentDirectory=%~dp0
set VSIXFile=Sage300UpgradeWizardPackage.vsix
set sourceFolder=%currentDirectory%bin\Release\
set destinationFolder=%currentDirectory%..\..\..\..\bin\wizards\

echo ┌─────────────────────────────────────────────────────────────────────────────────────────┐
echo │ File: CopyToSDKBinFolder.cmd                                                               │
echo │                                                                                         │
echo │ Purpose: This batch file copy the release version of Sage300UpgradeWizardPackage.vsix   │
echo │          to the \SDK\bin\wizards\ folder                                                │
echo └─────────────────────────────────────────────────────────────────────────────────────────┘

:: ┌───────────────────────────────────────────────────────────────────────────────────────────┐
:: │ Ensure that the VSIX file exists...                                                       │
:: └───────────────────────────────────────────────────────────────────────────────────────────┘
if not exist %sourceFolder%%VSIXFile% (
  echo %VSIXFile% does not exist. 
  echo Ensure that the program %VSIX% has been built in 'Release' mode.
  echo Exiting Program.
  chcp %CodePage%>nul
  goto :EOF
)

:: ┌───────────────────────────────────────────────────────────────────────────────────────────┐
:: │ Copy!                                                                                     │
:: └───────────────────────────────────────────────────────────────────────────────────────────┘
robocopy %sourceFolder% %destinationFolder% %VSIXFile%

:: ┌───────────────────────────────────────────────────────────────────────────────────────────┐
:: │ Restore the previous code page                                                            │
:: └───────────────────────────────────────────────────────────────────────────────────────────┘
chcp %CodePage%>nul

:: Enable the following line to prevent the console window from closing
:: This is used when invoking this file from windows explorer.
:: cmd /k
