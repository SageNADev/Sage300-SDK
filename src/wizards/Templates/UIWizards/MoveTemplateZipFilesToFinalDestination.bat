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

copy /Y BusinessRepository.zip %targetPath%Web.BusinessRepository.zip
del /Q BusinessRepository.zip

copy /Y Interfaces.zip %targetPath%Web.Interfaces.zip
del /Q Interfaces.zip

copy /Y Models.zip %targetPath%Web.Models.zip
del /Q Models.zip

copy /Y Resources.zip %targetPath%Web.Resources.zip
del /Q Resources.zip

copy /Y Services.zip %targetPath%Web.Services.zip
del /Q Services.zip

copy /Y Web.zip %targetPath%Web.Web.zip
del /Q Web.zip

copy /Y WebApi.WebApi.zip %targetPath%
del /Q  WebApi.WebApi.zip

copy /Y WebApi.Models.zip %targetPath%
del /Q  WebApi.Models.zip


copy /Y Sage300SolutionTemplate.zip %targetPath%
del /Q Sage300SolutionTemplate.zip

goto :EOF

:: End of file
