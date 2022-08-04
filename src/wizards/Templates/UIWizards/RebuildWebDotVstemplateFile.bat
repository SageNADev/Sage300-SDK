@echo off

:: ---------------------------------------------------------------------------------------
:: File: RebuildWebDotVstemplateFile.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Rebuild the Web.vstemplate file based on the contents of the
::            following folder (and subfolders)
:: 
::               'SDK\src\wizards\Templates\UIWizards\Web\' 
::
:: Usage Example:
::          RebuildWebDotVstemplateFile.bat
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
  echo For Example: Set CNA2_SOURCE_ROOT=D:\projects\SageAzureDev\release-2023.0\
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


set sdkroot=%myDir%..\..\..\..\
start ../../../../bin/Utilities/WebTemplateGenerator/WebTemplateGenerator.exe --solutionwizard --sdkroot=%sdkroot%

goto :EOF



