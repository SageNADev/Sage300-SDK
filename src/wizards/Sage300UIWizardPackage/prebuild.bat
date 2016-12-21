@echo off
REM Sage300UIWizardPackage prebuild
REM - get columbus-web artifacts if env variable EXTRACT_FROM_REPO set to 1
REM - compress to zip the wizard package

REM Usage example:
REM  Prebuild.bat <ProjectTemplates path base directory>

setlocal
set projectTemplatesBaseDir=%~dp0
if not [%1]==[] (
  set projectTemplatesBaseDir=%1
)
echo projectTemplatesBaseDir=%projectTemplatesBaseDir%

set myDir=%~dp0

set projectTemplatesPath=%projectTemplatesBaseDir%\ProjectTemplates
set srcProjectTemplatesDir=%myDir%\..\Templates
set compressProjectTemplatesCmd=%srcProjectTemplatesDir%\CreateTemplateZipFiles.bat
set getArtifactsCmd=%srcProjectTemplatesDir%\CopyWebRepoFiles.bat

call :GetArtifacts
call :CompressProjectTemplates

goto :EOF

:CompressProjectTemplates
  if not exist %projectTemplatesPath% (
    mkdir %projectTemplatesPath%
  )
  pushd %projectTemplatesPath%
  call %compressProjectTemplatesCmd% %projectTemplatesPath%
  popd
  goto :EOF
  
:GetArtifacts
  if [%EXTRACT_FROM_REPO%]==[1] (
    set repoDirName=Columbus-Web
    set repoDirRelPath=..\..\..\..
    if exist %myDir%%repoDirRelPath%\%repoDirName% (
      call %getArtifactsCmd%
    ) 
  ) else (
    echo ==================================================================================
    echo Set Env Variable [EXTRACT_FROM_REPO] to get artifacts. Requires source code access.
    echo ==================================================================================
  )
  goto :EOF