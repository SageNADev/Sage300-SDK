# ---------------------------------------------------------------------------------------
# File: psCopyWebFiles.ps1
#
# Purpose: This Powershell file will copy Web repo files from
#          CNA2\Columbus-Web\ (with some exclusions)
#
# Note:    Ensure that you have defined an environment variable called 
#          CNA2_SOURCE_ROOT that points to the root of your local CNA2 
#          source repository.
#
#          This file is meant to be called from CopyWebRepoFiles.bat
#
# Original Comments Below
#
# Left TODO after running this script.
# - Build of MergeISVProject.exe and commit copied artifacts.
# - Copy Web changes out to sample code
# If Changed:
# - Requires manual merge of Global.asax.cs
# - Requires manual merge of Web.Config
# - Diff for other code changes...
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
  Write-Host 'Example: CNA2_SOURCE_ROOT=C:\projects\SageAzureDev'
  Write-Host 'OR' 
  Write-Host 'Create a variable called rootCNA2SourceFolder in this file that points'
  Write-Host 'to your local CNA2 source directory.'
  Write-Host 'Example: $rootCNA2SourceFolder="C:\projects\SageAzureDev\"'
}
$verbosepreference = 'continue'
$webSubPaths = 'Areas\Core,Areas\Shared,Views,Content,Assets'
$webSubPathsCopyAll = 'Customization'
$scriptsWebSubPath = 'Scripts'
$includeScriptsWeb = 'Sage.CA.SBS.ERP.Sage300.Common.*.js,Sage.CA.SBS.ERP.Sage300.Core.*.js'
$webFolderName = "Web"
$absWebFolderPath = "$PSScriptRoot\$webFolderName"
$webAssetDirPath = "$rootCNA2SourceFolder\Columbus-Web\Sage.CA.SBS.ERP.Sage300.Web"

# ---------------------------------------------------------------------------------------
# Save the current folder location
# ---------------------------------------------------------------------------------------
#Write-Host $webAssetDirPath
push-location -path $absWebFolderPath

# ---------------------------------------------------------------------------------------
# Copy Web Artifact folders (with some exclusions)
# /xf : eXclude Files
# /xd : eXclude Directories
# ---------------------------------------------------------------------------------------
$webSubPaths.split(',') | Foreach-Object { 
  robocopy /S "$webAssetDirPath\$_" "$_" `
  /xf *.cs *.csproj *.user *.xml *.Sage300.Revaluation*.js *.IC.Common.js _wizard.cshtml packages.config `
  /xd TU obj
}

# ---------------------------------------------------------------------------------------
# Copy Web\Customization (All files and folders)
# ---------------------------------------------------------------------------------------
$webSubPathsCopyAll.split(',') | Foreach-Object { 
  robocopy /S "$webAssetDirPath\$_" "$_" 
}

# ---------------------------------------------------------------------------------------
# Copy Web\Scripts (Some exclusions)
# /xf : eXclude Files
# ---------------------------------------------------------------------------------------
$scriptsWebSubPath.split(',') | Foreach-Object { 
  robocopy /S "$webAssetDirPath\$_" "$_" `
  /xf kendo.all*.js Test_*.js *TestUtils.js chutzpah.json 
}

# ---------------------------------------------------------------------------------------
# Copy WebForms
# ---------------------------------------------------------------------------------------
"WebForms" | Foreach-Object { robocopy /E "$webAssetDirPath\$_" "$_" }

# ---------------------------------------------------------------------------------------
# Quick Edit Namespaces
# ---------------------------------------------------------------------------------------
Get-ChildItem "WebForms\**" | Foreach-Object {
  $content = (Get-Content $_);
  $content = $content.replace('Inherits="Sage.CA.SBS.ERP.Sage300.Web','Inherits="$companynamespace$.$applicationid$.Web');
  $content = $content.replace('namespace Sage.CA.SBS.ERP.Sage300.Web.WebForms','namespace $companynamespace$.$applicationid$.Web.WebForms');
  $content = $content.replace(' Common.Models.',' Sage.CA.SBS.ERP.Sage300.Common.Models.');
  $content = $content.replace('(Common.Models.','(Sage.CA.SBS.ERP.Sage300.Common.Models.');
  $content = $content.replace('<Common.Models.','<Sage.CA.SBS.ERP.Sage300.Common.Models.');
  $content | Out-File $_ -Encoding "UTF8" -Width 300
}

# ---------------------------------------------------------------------------------------
# Clean up
# Remove Some Empty folders
# ---------------------------------------------------------------------------------------
(Get-ChildItem . -r | Where-Object {$_.PSIsContainer -eq $True}) | 
Where-Object {
  $_.GetFiles().Count -eq 0 -and 
  $_.GetDirectories().Count -eq 0
} | Remove-Item

# ---------------------------------------------------------------------------------------
# Remove extra files.
# ---------------------------------------------------------------------------------------
Remove-Item $absWebFolderPath\Areas\Core\web.config
Remove-Item $absWebFolderPath\Areas\Shared\Models -force -recurse
pop-location


# End of file