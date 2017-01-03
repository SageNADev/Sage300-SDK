@echo off
REM Sage300SolutionWizard prebuild
REM - copy to \resources the common .props file for project references

REM Usage example:
REM  Prebuild.bat

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