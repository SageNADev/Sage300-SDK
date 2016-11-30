# Left TODO after running this script.
# - Build of MergeISVProject.exe and commit copied artifacts.
# - Copy Web changes out to sample code
# If Changed:
# - Requires manual merge of Global.asax.cs
# - Requires manual merge of Web.Config
# - Diff for other code changes...

$verbosepreference = 'continue'

$webSubPaths = 'Areas\Core,Areas\Shared,Views,Content,Assets'
$webSubPathsCopyAll = 'Customization'
$scriptsWebSubPath = 'Scripts'

$includeScriptsWeb = 'Sage.CA.SBS.ERP.Sage300.Common.*.js,Sage.CA.SBS.ERP.Sage300.Core.*.js'

$webFolderName = "Web"
$absWebFolderPath = "$PSScriptRoot\$webFolderName"

$webAssetDirPath = "$PSScriptRoot\..\..\..\..\Columbus-Web\Sage.CA.SBS.ERP.Sage300.Web"

push-location -path $absWebFolderPath
# Copy Web Artifact folders
$webSubPaths.split(',') | Foreach-Object { 
  robocopy /S /E "$webAssetDirPath\$_" "$_" `
  /xf *.cs,*.csproj,*.user,*.xml,*.Sage300.Revaluation*.js,*.IC.Common.js,_wizard.cshtml,packages.config
}

$webSubPathsCopyAll.split(',') | Foreach-Object { 
  robocopy /S /E "$webAssetDirPath\$_" "$_" 
}

$scriptsWebSubPath.split(',') | Foreach-Object { 
  robocopy /S /E "$webAssetDirPath\$_" "$_" `
  /xf kendo.all*.js,Test_*.js,*TestUtils.js,chutzpah.json 
}
# Copy WebForms
"WebForms" | Foreach-Object { robocopy /E "$webAssetDirPath\$_" "$_" }
# Quick Edit Namespaces
Get-ChildItem "WebForms\**" | Foreach-Object {
  $content = (Get-Content $_);
  $content = $content.replace('Inherits="Sage.CA.SBS.ERP.Sage300.Web','Inherits="$companynamespace$.Web');
  $content = $content.replace('namespace Sage.CA.SBS.ERP.Sage300.Web.WebForms','namespace $companynamespace$.Web.WebForms');
  $content = $content.replace(' Common.Models.',' Sage.CA.SBS.ERP.Sage300.Common.Models.');
  $content = $content.replace('(Common.Models.','(Sage.CA.SBS.ERP.Sage300.Common.Models.');
  $content = $content.replace('<Common.Models.','<Sage.CA.SBS.ERP.Sage300.Common.Models.');
  $content | Out-File $_ -Encoding "UTF8" -Width 300
}

# Clean up
# Remove Some Empty folders
(gci . -r | ? {$_.PSIsContainer -eq $True}) | ? {$_.GetFiles().Count -eq 0 -and $_.GetDirectories().Count -eq 0} | Remove-Item
# Remove extra files.
rm $absWebFolderPath\Areas\Core\web.config
rm $absWebFolderPath\Areas\Shared\Models -force -recurse
pop-location
