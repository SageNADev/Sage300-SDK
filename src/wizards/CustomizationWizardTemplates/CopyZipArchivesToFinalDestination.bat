@echo off

:: ---------------------------------------------------------------------------------------
:: File: CopyZipArchivesToFinalDestination.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Copy the two zip archives (Solution.zip and Web.zip) to the following folder:
::
::               'SDK\src\wizards\Customization\Sage300UICustomizationSolution\ProjectTemplates' 
::
:: Usage Example:
::          CopyZipArchivesToFinalDestination.bat
::
:: ---------------------------------------------------------------------------------------

setlocal


:: ---------------------------------------------------------------------------------------
:: Get the current directory
:: ---------------------------------------------------------------------------------------
set myDir=%~dp0

ROBOCOPY %myDir% %myDir%..\Customization\Sage300UICustomizationSolution\ProjectTemplates *.zip

goto :EOF


