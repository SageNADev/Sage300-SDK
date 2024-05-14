@echo off

:: ---------------------------------------------------------------------------------------
:: File: MoveZipArchivesToFinalDestination.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Move the two zip archives (Solution.zip and Web.zip) to the following folder:
::
::               'SDK\src\wizards\Customization\Sage300UICustomizationSolution\ProjectTemplates' 
::
:: Usage Example:
::          MoveZipArchivesToFinalDestination.bat
::
:: ---------------------------------------------------------------------------------------

setlocal


:: ---------------------------------------------------------------------------------------
:: Get the current directory
:: ---------------------------------------------------------------------------------------
set myDir=%~dp0

ROBOCOPY %myDir% %myDir%..\..\Sage300CustomizationWizardPackage\ProjectTemplates *.zip
del /Q Web.zip
del /Q Solution.zip

goto :EOF


