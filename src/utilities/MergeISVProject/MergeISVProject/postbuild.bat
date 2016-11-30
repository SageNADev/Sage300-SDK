@echo off
setlocal
set myDir=%~dp0
set artifactToPub=%myDir%bin\release\MergeISVProject.exe

REM if not [%1]==[] (
REM   set artifactToPub=%1
REM )
echo artifactToPub=%artifactToPub%

if not exist %artifactToPub% (
  @echo %artifactToPub% does not exist.
  @echo Build the RELEASE version to copy MergeISVProject.exe to SDK.
  exit 0
)
call :CopyToPublishLocations %artifactToPub%

goto :EOF

:CopyToPublishLocations
  setlocal
  @echo Delete destination files.
  call :RunPS "%myDir%\psCopyArtifactToPublish.ps1 -target %1"
  goto :EOF

  
:RunPS
  setlocal
  @echo on
  %WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -Command "%1"
  @echo off
  goto :EOF