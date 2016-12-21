@echo off
REM
REM
REM

REM Usage example:
REM  Prebuild.bat <ProjectTemplates path base directory>
setlocal
set myDir=%~dp0

set resourcesDir=%myDir%Resources

call :GetPropsFile

goto :EOF

:GetPropsFile
  if not exist %resourcesDir% (
    mkdir %resourcesDir%
  )
  robocopy /is %myDir%..\ProjectSettings %resourcesDir% AccpacDotNetVersion.props

  goto :EOF  