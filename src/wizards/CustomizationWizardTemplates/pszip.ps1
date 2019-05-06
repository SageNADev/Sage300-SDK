# ---------------------------------------------------------------------------------------
# File: pszip.ps1
#
# Purpose: Simple zip script.  Will overwrite destination zip files.
#
# Note:    This file is meant to be called from CreateTemplateZipFiles.bat
#
# ---------------------------------------------------------------------------------------

Param 
(
  [Parameter(Mandatory=$True,Position=0)]
    [string] $SrcPath,
  [Parameter(Mandatory=$True,Position=1)]
    [string] $DestPath 
)
Add-Type -A System.IO.Compression.FileSystem
if (Test-Path $DestPath)
{
  Remove-Item $DestPath -Force
}
[IO.Compression.ZipFile]::CreateFromDirectory($SrcPath, $DestPath)