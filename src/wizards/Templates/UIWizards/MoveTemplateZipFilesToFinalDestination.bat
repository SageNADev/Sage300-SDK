@echo off
:: ---------------------------------------------------------------------------------------
:: File: MoveTemplateZipFilesToFinalDestination.bat 
::
:: Purpose: This batch file will do the following:
::
::          * Move the folloiwng template zip archies:
::
::                 - BusinessRepository
::                 - Interfaces
::                 - Models
::                 - Resources
::                 - Services
::                 - Web
::                 - Sage300SolutionTemplate
::            
::            to their final destination
::            \src\wizards\Sage300SolutionWizardPackage\ProjectTemplates\
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

set targetPath=..\..\Sage300SolutionWizardPackage\ProjectTemplates\

copy /Y BusinessRepository.zip %targetPath%
del /Q BusinessRepository.zip

copy /Y Interfaces.zip %targetPath%
del /Q Interfaces.zip

copy /Y Models.zip %targetPath%
del /Q Models.zip

copy /Y Resources.zip %targetPath%
del /Q Resources.zip

copy /Y Services.zip %targetPath%
del /Q Services.zip

copy /Y Web.zip %targetPath%
del /Q Web.zip

copy /Y Sage300SolutionTemplate.zip %targetPath%
del /Q Sage300SolutionTemplate.zip

goto :EOF

:: End of file
