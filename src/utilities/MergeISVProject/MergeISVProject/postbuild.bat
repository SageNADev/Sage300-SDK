@echo off
setlocal
set myDir=%~dp0
set artifactToPub=%myDir%bin\release\MergeISVProject.exe

if not [%1]==[] (
  set artifactToPub=%1
)
echo artifactToPub=%artifactToPub%

if not exist %artifactToPub% (
  @echo %artifactToPub% does not exist.
  exit 1
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
  powershell.exe -ExecutionPolicy Bypass -Command "%1"
  @echo off
  goto :EOF