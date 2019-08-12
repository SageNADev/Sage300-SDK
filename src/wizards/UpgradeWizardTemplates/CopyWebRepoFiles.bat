@echo off

:: ---------------------------------------------------------------------------------------
:: File: CopyWebRepoFiles.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Remove folders and files from the 
::            SDK\src\wizards\UpgradeWizardTemplates\Items\ folder
::            Note: It does not remove all folders and files, just
::                  folders (and files) defined in the psDeleteWebFiles.ps1 script file.
::
::          * Copy the Columbus-Web artifacts to the 
::            SDK\src\wizards\UpgradeWizardTemplates\Items\ folder
::
:: Usage Example:
::          CopyWebRepoFiles.bat
::
:: ---------------------------------------------------------------------------------------

setlocal


:: ---------------------------------------------------------------------------------------
:: Get the current directory
:: ---------------------------------------------------------------------------------------
set myDir=%~dp0

:: ---------------------------------------------------------------------------------------
:: Check for the existence of the CNA2_SOURCE_ROOT environment variable.
:: ---------------------------------------------------------------------------------------
if [%CNA2_SOURCE_ROOT%]==[] (
  echo The environment variable ----  CNA2_SOURCE_ROOT ---- has not yet been defined.
  echo Please set CNA2_SOURCE_ROOT to the root folder of the CNA2 source code.
  echo For Example: Set CNA2_SOURCE_ROOT=C:\projects\SageAzureDev\
  goto :EOF
)

set srcCopyPath=%CNA2_SOURCE_ROOT%Columbus-Web
if not [%1]==[] (
  set srcCopyPath=%1
)
echo srcCopyPath=%srcCopyPath%
if not exist %srcCopyPath% (
  echo %srcCopyPath% does not exist
  exit 1
)

call :DeleteDestinationFiles
call :CopyWebRepoArtifactFiles

goto :EOF



:: ---------------------------------------------------------------------------------------
:: Function Definitions
:: ---------------------------------------------------------------------------------------

:DeleteDestinationFiles
  setlocal
  @echo Delete destination files.
  call :RunPS "%myDir%psDeleteWebFiles.ps1"
  goto :EOF

:CopyWebRepoArtifactFiles
  setlocal
  @echo Copy artifact files.
  call :RunPS "%myDir%psCopyWebFiles.ps1"
  goto :EOF
  
:RunPS
  setlocal
  @echo on
  powershell.exe -ExecutionPolicy Bypass -file %1
  @echo off
  goto :EOF