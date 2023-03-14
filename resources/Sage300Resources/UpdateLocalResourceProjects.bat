@echo off

:: ---------------------------------------------------------------------------------------
:: File: UpdateLocalResourceProjects.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Remove all Resource projects from 
::            SDK\resources\Sage300Resources\ folder
::
::			* Copy all Columbus-Web Resource projects to 
::            SDK\resources\Sage300Resources\ folder
::
:: Usage Example:
::          UpdateLocalResourceProjects.bat
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
  echo For Example: Set CNA2_SOURCE_ROOT=C:\projects\SageAzureDev\release-2023.0\
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

call :DeleteResourceProjects
call :CopyResourceProjects


goto :EOF



:: ---------------------------------------------------------------------------------------
:: Function Definitions
:: ---------------------------------------------------------------------------------------

:DeleteResourceProjects
  setlocal
  @echo Deleting resource projects...
  call :RunPS "%myDir%psDeleteResourceProjects.ps1"
  goto :EOF

:CopyResourceProjects
  setlocal
  @echo Copying all Resource projects...
  call :RunPS "%myDir%psCopyResourceProjects.ps1"
  goto :EOF

:RunPS
  setlocal
  @echo on
  powershell.exe -ExecutionPolicy Bypass -file %1
  @echo off
  goto :EOF