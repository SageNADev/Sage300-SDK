<#
.DESCRIPTION
Copy the binary artifact .exe file to all required locations in the SDK

.PARAMETER $target
The full path to the target artifact file.

#>
param
(
  [Parameter(Mandatory=$True,Position=0)]
  [string] $target
)

$artifactFile = [System.IO.DirectoryInfo]$target
$sdkDir = "$PsScriptRoot\..\..\..\.."

Write-Verbose $sdkDir
Write-Verbose $artifactFile.Name
Write-Verbose $artifactFile.Parent.FullName

gci $sdkDir -filter $artifactFile.Name -recurse |  Foreach-Object { 
  Write-Verbose "Perform copy on:"
  $folder = (Split-Path $_.FullName -Parent)
  Write-Verbose $folder
 
  if ($folder -notmatch '\\utilities\\MergeISVProject\\')
  {
    # copy to all locations except this project
    robocopy /IS $artifactFile.Parent.FullName $folder $artifactFile.Name
  }
  else
  {
    write-verbose "Don't copy from source project to itself."
  } 
   
}
