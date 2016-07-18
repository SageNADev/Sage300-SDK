@echo on
REM
REM
REM

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
