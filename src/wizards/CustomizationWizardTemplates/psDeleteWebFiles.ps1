# ---------------------------------------------------------------------------------------
# File:     psDeleteWebFiles.ps1
#
# Purpose:  Delete various files and folders from 
# 
#              'SDK\wizards\CustomizationWizardTemplates\Web\' 
#
# Note:     This file is meant to be called from CopyWebRepoFiles.bat
#
# ---------------------------------------------------------------------------------------

# ---------------------------------------------------------------------------------------
# Initializations
# ---------------------------------------------------------------------------------------
$verbosepreference = 'continue'
$webSubPaths = 'Areas\Shared,Areas\Core,Views,Scripts'
$webFolderName = "Web"

# ---------------------------------------------------------------------------------------
# Save the current location to the top of the location stack
# ---------------------------------------------------------------------------------------
# Write-Host $PSScriptRoot
push-location -path "$PSScriptRoot\$webFolderName"

# ---------------------------------------------------------------------------------------
# Now go recursively go through each folder listed in $webSubPaths 
# and remove the files and folders
# ---------------------------------------------------------------------------------------
$webSubPaths.split(',') | Foreach-Object { 
  if (test-path $_) {
    get-childitem -path "$_" -recurse | remove-item -force -recurse; Remove-Item "$_" -Force -recurse;
  }
  else
  {
    write-verbose "$_ not found."
  }
}

# ---------------------------------------------------------------------------------------
# Restore the current location back to the way it was before running this file
# ---------------------------------------------------------------------------------------
pop-location

# End of file