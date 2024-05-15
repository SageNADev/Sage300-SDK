@echo off

:: ----------------------------------------------------------------------------------------------
:: File: CopyAccpacDotNetVersionSettings.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Copy the file 
::               SDK\Settings\AccpacDotNetVersion.props 
::            to 
::               SDK\src\wizards\Sage300WizardPackage\ItemTemplates\ 
::
:: Usage Example:
::          CopyAccpacDotNetVersionSettings.bat
::
:: ----------------------------------------------------------------------------------------------

setlocal


:: ---------------------------------------------------------------------------------------
:: Get the current directory
:: ---------------------------------------------------------------------------------------
set myDir=%~dp0

set sourcePath=..\..\..\..\Settings\AccpacDotNetVersion.props
set targetPath=..\..\Sage300WizardPackage\ItemTemplates\

copy /Y %sourcePath% %targetPath%


goto :EOF

:: End of file
