# ---------------------------------------------------------------------------------------
# File: psDeleteResourceProjects.ps1
#
# Purpose: This Powershell file will remove/delete all Web Resource projects from
#          the Sage300-SDK\src\wizards\Sage300Resources\ folder
#
#          This file is meant to be called from DeleteResourceProjects.bat
#
# ---------------------------------------------------------------------------------------

# ---------------------------------------------------------------------------------------
# Initializations
# ---------------------------------------------------------------------------------------
#
# None

# Settings
$ModuleNameList = "AP,AR,AS,CS,GL,IC,KN,KPI,MT,OE,PM,PO,PR,VPF"

# ---------------------------------------------------------------------------------------
# Remove existing module Resource project folders
# ---------------------------------------------------------------------------------------
Write-Host 'Removing all module resource project folders....'
$ModuleNameList.split(',') | Foreach-Object { 
  $moduleId=$_
  $destinationDirectory = "$PSScriptRoot\Sage.CA.SBS.ERP.Sage300.$moduleId.Resources"
  
  Remove-Item "$destinationDirectory" -recurse -force
}

# ---------------------------------------------------------------------------------------
# Remove existing common/core Resource project folders
# ---------------------------------------------------------------------------------------
Write-Host 'Removing all common/core resource project folders....'

$destinationDirectory = "$PSScriptRoot\Sage.CA.SBS.ERP.Sage300.Common.Resources"
Remove-Item "$destinationDirectory" -recurse -force

$destinationDirectory = "$PSScriptRoot\Sage.CA.SBS.ERP.Sage300.Core.Resources"
Remove-Item "$destinationDirectory" -recurse -force

# End of file