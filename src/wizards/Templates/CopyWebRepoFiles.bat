@echo off
setlocal
set myDir=%~dp0

set srcCopyPath=%myDir%..\..\..\..\Columbus-Web
if not [%1]==[] (
  set srcCopyPath=%1
)
echo srcCopyPath=%srcCopyPath%
if not exist %srcCopyPath% (
  echo %srcCopyPath% does not exist
  exit 1
)

call :DeleteDestinationFiles
call :CopyWebRepoArtifactFiles

goto :EOF

REM ***********************************
REM *** Function Definitions

:DeleteDestinationFiles
  setlocal
  @echo Delete destination files.
  call :RunPS "%myDir%\psDeleteWebFiles.ps1"
  goto :EOF

:CopyWebRepoArtifactFiles
  setlocal
  @echo Delete destination files.
  call :RunPS "%myDir%\psCopyWebFiles.ps1"
  goto :EOF
  
:RunPS
  setlocal
  @echo on
  powershell.exe -ExecutionPolicy Bypass -file %1
  @echo off
  goto :EOF