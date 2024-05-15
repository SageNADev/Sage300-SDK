@echo off
:: ---------------------------------------------------------------------------------------
:: File: MoveTemplateZipFilesToFinalDestination.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Move the following template zip archive:
::
::                 - Items
::            
::            to it's final destination
::            \src\wizards\Sage300WizardPackage\ItemTemplates\
::
:: Usage Example:
::          MoveTemplateZipFilesToFinalDestination.bat 
::
:: ---------------------------------------------------------------------------------------

setlocal


:: ---------------------------------------------------------------------------------------
:: Get the current directory
:: ---------------------------------------------------------------------------------------
set myDir=%~dp0

set targetPath=..\..\Sage300WizardPackage\ItemTemplates\

copy /Y Items.zip %targetPath%
del /Q Items.zip

goto :EOF

:: End of file
