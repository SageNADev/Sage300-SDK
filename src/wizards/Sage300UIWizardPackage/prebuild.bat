@echo off

:: ---------------------------------------------------------------------------------------
:: File: prebuild.bat (Part of Sage300UIWizardPackage)
::
:: Purpose: This batch file will do the following:
::
::          * Get Columbus-Web artifacts if the environment 
::            variable EXTRAC_FROM_REPO is set to 1
::          * Create all the various zip files based
::            on the above artifacts.
::
:: Usage Example:
::          Prebuild.bat <ProjectTemplates path base directory>
::
::          Step 1: Do setup tasks
::                  * Setup constants and variables
::                  * Create a temporary folder
::
::          Step 2: Copy assets to temporary folder
::                  * \SDK\Settings\AccpacDotNetVersion.props
::                  * \assets\__TemplateIcon.ico
::                  * \assets\MyTemplate.vstemplate
::                  * Web.zip
::
::          Step 3: Clear out the final destination folder
::
::          Step 4: Zip everything up into a file called UpgradeWebItems.zip
::
::          Step 5: Remove the temporary working folder
::
::          Step 6: Done!
:: ---------------------------------------------------------------------------------------

setlocal

::
:: Initialize some variables
::
set EXTRACT_FROM_REPO=1

set projectTemplatesBaseDir=%~dp0

:: If a command-line parameter passed in...
if not [%1]==[] (
  set projectTemplatesBaseDir=%1
)
set myDir=%~dp0
set projectTemplatesPath=%projectTemplatesBaseDir%ProjectTemplates
set srcProjectTemplatesDir=%myDir%\..\Templates
set compressProjectTemplatesCmd=%srcProjectTemplatesDir%\CreateTemplateZipFiles.bat
set getArtifactsCmd=%srcProjectTemplatesDir%\CopyWebRepoFiles.bat

:: 
:: Do the work!
::
call :GetArtifacts
call :CompressProjectTemplates

GOTO :EOF


:: ---------------------------------------------------------------------------------------
:: Function Definitions
:: ---------------------------------------------------------------------------------------

:GetArtifacts
  if [%EXTRACT_FROM_REPO%]==[1] (
    set repoDirName=Columbus-Web
    set repoDirRelPath=..\..\..\..
    echo FOLDER=%myDir%%repoDirRelPath%\%repoDirName%
    if exist %myDir%%repoDirRelPath%\%repoDirName% (
      call %getArtifactsCmd%
    ) 
  ) else (
    echo ==================================================================================
    echo Set Env Variable [EXTRACT_FROM_REPO] to get artifacts. Requires source code access.
    echo ==================================================================================
  )
  goto :EOF

:CompressProjectTemplates
  if not exist %projectTemplatesPath% (
    mkdir %projectTemplatesPath%
  )
  pushd %projectTemplatesPath%
  call %compressProjectTemplatesCmd% %projectTemplatesPath%
  popd
  goto :EOF
