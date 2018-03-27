# ---------------------------------------------------------------------------------------
# File:     psCleanWebFolder.ps1
#
# Purpose:  Remove folders and files from a directory
#           based on the contents of a text-based manifest file
#
# Note:     This file is meant to be called from RebuildPayload.bat
#           Ensure that the manifest file is correct as well
#           For each product update/release, create a new
#           manifest file.
#
# ---------------------------------------------------------------------------------------

$SourcePath=$args[0]
$TargetPath=$args[1]
$manifest="web-upgrader-manifest-2018.2.txt"

Get-Content $manifest | ForEach-Object {
  # Build the source and target paths
  $s = Join-Path -Path $SourcePath -ChildPath $_
  $d = Join-Path -Path $TargetPath -ChildPath $_
  Write-Host "Copying $s to $d"
  Copy-Item -Path $s -Destination $d
}

# End of file
