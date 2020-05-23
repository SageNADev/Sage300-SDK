@echo off

:: ---------------------------------------------------------------------------------------
:: File: UpdateAccpacDotNetVersionProps.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Copy the most recent version of AccpacDotNetVersion.props file
::            from SDK\settings\ 
::            into SDK\src\wizards\CustomizationWizardTemplates\Web\
::
:: Usage Example:
::          UpdateAccpacDotNetVersionProps.bat
::
:: ---------------------------------------------------------------------------------------

setlocal


:: ---------------------------------------------------------------------------------------
:: Get the current directory
:: ---------------------------------------------------------------------------------------
set myDir=%~dp0
set settingsFolder=%myDir%\..\..\..\settings


copy %settingsFolder%\AccpacDotNetVersion.props %myDir%\Web\
goto :EOF
