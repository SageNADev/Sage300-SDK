$verbosepreference = 'continue'
$webSubPaths = 'Areas\Shared,Areas\Core,Views,Scripts,Content,Assets,Customization,WebForms'
$webFolderName = "Web"

push-location -path "$PSScriptRoot\$webFolderName"
$webSubPaths.split(',') | Foreach-Object { 
  if (test-path $_) {
    get-childitem -path "$_" -recurse | remove-item -force -recurse; Remove-Item "$_" -Force -recurse;
  }
  else
  {
    write-verbose "$_ not found."
  }
}
pop-location
