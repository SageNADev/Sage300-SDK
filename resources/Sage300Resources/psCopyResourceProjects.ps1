# ---------------------------------------------------------------------------------------
# File: psCopyResourceProjects.ps1
#
# Purpose: This Powershell file will copy all Web Resource projects from
#          CNA2\Columbus-XX\ 
#
# Note:    Ensure that you have defined an environment variable called 
#          CNA2_SOURCE_ROOT that points to the root of your local CNA2 
#          source repository.
#
#          This file is meant to be called from UpdateLocalResourceProjects.bat
#
# ---------------------------------------------------------------------------------------

# ---------------------------------------------------------------------------------------
# Initializations
# ---------------------------------------------------------------------------------------
#
# Developer Note: 
# Change this path to point to YOUR local CNA2 directory (or leave as is, if correct)
# Check for the existence of an environment variable called CNA2_SOURCE_ROOT
# that is set to your local CNA2 source directory. If this environment variable
# is empty or undefined, then display a message on how to resolve/set it.
#
$rootCNA2SourceFolder = $Env:CNA2_SOURCE_ROOT
if ($rootCNA2SourceFolder -eq '') {
  Write-Host 'Please ensure that you have defined an environment variable'
  Write-Host 'named CNA2-SOURCE-ROOT that points to your local CNA2 source directory.'
  Write-Host 'Example: CNA2_SOURCE_ROOT=D:\projects\SageAzureDev\release-2023.0\'
  Write-Host 'OR' 
  Write-Host 'Create a variable called rootCNA2SourceFolder in this file that points'
  Write-Host 'to your local CNA2 source directory.'
  Write-Host 'Example: $rootCNA2SourceFolder="D:\projects\SageAzureDev\release-2023.0\"'
}

# Settings [Common and Core are not included in this list but handled later in file]
$ModuleNameList = "AP,AR,AS,CS,GL,IC,KN,KPI,MT,OE,PM,PO,PR,TA,TS,TW,VPF"

# ---------------------------------------------------------------------------------------
# Copy all module resource projects
# ---------------------------------------------------------------------------------------
Write-Host 'Copying all module resource project folders to destination....'
$ModuleNameList.split(',') | Foreach-Object { 
  $moduleId=$_
  $sourceDirectory = "$rootCNA2SourceFolder\Columbus-$moduleId\Sage.CA.SBS.ERP.Sage300.$moduleId.Resources"	
  $destinationDirectory = "$PSScriptRoot\Sage.CA.SBS.ERP.Sage300.$moduleId.Resources"
  
  robocopy /E "$sourceDirectory" "$destinationDirectory"
}

# ---------------------------------------------------------------------------------------
# Copy all common/core resource projects
# ---------------------------------------------------------------------------------------
Write-Host 'Copying all common/core resource project folders to destination....'

$sourceDirectory = "$rootCNA2SourceFolder\Columbus-Framework\Common\Sage.CA.SBS.ERP.Sage300.Common.Resources"	
$destinationDirectory = "$PSScriptRoot\Sage.CA.SBS.ERP.Sage300.Common.Resources"
robocopy /E "$sourceDirectory" "$destinationDirectory"

$sourceDirectory = "$rootCNA2SourceFolder\Columbus-Framework\Core\Sage.CA.SBS.ERP.Sage300.Core.Resources"	
$destinationDirectory = "$PSScriptRoot\Sage.CA.SBS.ERP.Sage300.Core.Resources"
robocopy /E "$sourceDirectory" "$destinationDirectory"

# End of file