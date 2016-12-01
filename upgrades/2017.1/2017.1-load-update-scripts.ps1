<#
.SYNOPSIS
Loads the script blocks required for the Sage 300 SDK 2017.1 update
  
.DESCRIPTION
The script blocks loaded by this script performs the following update functions:
3.2 - Updates the .NET Framework in the *.csproj files
      Run command: c:\ps> & $UpdateDotNetFramework

3.4 - Updates Web projects (.csproj) //ItemGroup/Content nodes with wildcard entries to
      the web file contents.
      Run command: c:\ps> & $UpdateWebProject
      
3.5 - Updates the Web projects file contents for Areas\Core, Areas\Shared, Assets,
      Content, Views, Scripts folder.  This is a brute force remove and copy.
      Run command: c:\ps> & $UpdateWebArtifactsFiles

5.1 - Optional - Run only if Print Reporting is not in the target solution, otherwise,
      contents will be added twice to the .csproj file which will cause errors.
      i.e. Your Web project doesn't have the \WebForms folder nor the \WebForms 
      folder contents listed in the *Web.csproj file.
      Run command: c:\ps> & $AddReportFunctionality

Text addition into the *Web.csproj file uses the files Global.asax, 
and BundleRegistration.cs as find/replace points.      
These script blocks can be run on command line once this script has run successfully.

.PARAMETER tempSDKWizGenPath
The path to the temporary Sage 300 SDK 2017.1 generated solution
e.g. "c:\2017pu1SDK-Generated\Sage300CMvcApplication1"

.PARAMETER webNamespacePrefix
The namespace value before the ".Web" for the Web Projects namespace
e.g. "ValuedPartner"

.EXAMPLE
c:\ps> .\2017.1-load-update-scripts.ps1 c:\2017pu1SDK-Generated\Sage300CMvcApplication1 ValuedPartner

.EXAMPLE
c:\ps> .\2017.1-load-update-scripts.ps1 -tempSDKWizGenPath "c:\2017pu1SDK-Generated\Sage300CMvcApplication1" -webNamespacePrefix ValuedPartner
#>
Param
(
  [Parameter(Mandatory=$True,Position=0)]
  [string] $tempSDKWizGenPath,
  
  [Parameter(Mandatory=$True,Position=1)]
  [string] $webNamespacePrefix
)

# Verbose
$global:VerbosePreference = 'Continue'
$global:ErrorActionPreference = "Stop"
################################################################################
# Initialize Powershell variables
################################################################################
# The path to the temporary Sage 300 SDK 2017.1 generated solution
# e.g. "c:\2017pu1SDK-Generated\Sage300CMvcApplication1"
$global:tempWizardGenPath = $tempSDKWizGenPath
# The namespace value before the ".Web" for the Web Projects namespace
# e.g. "ValuedPartner"
$global:nameSpaceBeforeWeb = $webNamespacePrefix

# Web project
$global:webFolderName = "$nameSpaceBeforeWeb.Web"
$global:webAssetDirPath = "$tempWizardGenPath\$webFolderName"

$errorMsgFormat = "({0}) does not exist.  Please correct the value and re-run this script or ensure you are running this script in the same directory as the solution of your project."
# Folder existence check
if ($false -eq (Test-Path $tempWizardGenPath))
{
  Write-Error ($errorMsgFormat -f $tempWizardGenPath)
}
if ($false -eq (Test-Path $webFolderName))
{
  Write-Error ($errorMsgFormat -f $webFolderName)
}
if ($false -eq (Test-Path $webAssetDirPath))
{
  Write-Error ($errorMsgFormat -f $webAssetDirPath)
}

# Web Artifact files
$global:webSubPaths = 'Areas\Shared,Areas\Core,Views,Scripts,Content,Assets'
# Webform namespace related
$global:replaceWebFormNamespace = 'Inherits="Sage.CA.SBS.ERP.Sage300.Web'
$global:searchReplaceWebFormNamespace = 'Inherits="' + $webFolderName

################################################################################
# 3.2 Update the .NET Framework
################################################################################
$global:UpdateDotNetFramework = {
# Update .NET Framework for the project files (*.csproj)
Get-ChildItem -Recurse *.csproj | Foreach-Object {(Get-Content $_).replace('<TargetFrameworkVersion>v4.5.1</TargetFrameworkVersion>','<TargetFrameworkVersion>v4.6.2</TargetFrameworkVersion>')  | Out-File $_ -Encoding "UTF8" }
Write-Verbose ".NET Framework updated in *.csproj files."
}
################################################################################
# 3.4 Update Web project's ItemGroup
################################################################################
$global:UpdateWebProject = {
# Clean the existing contents from the web project
Get-ChildItem "$webFolderName\*Web.csproj" | Foreach-Object {
  $csprojXml = [xml] (Get-Content $_)
  $webSubPaths.split(',') | Foreach-Object {
    & $RemoveXmlElements $csprojXml $_
  }
  
  Format-XML $csprojXml | Out-File -Encoding utf8 $_
}

# Re-Add Web Artifacts to Web project
$globalasax='<Content Include="Global.asax" />'
$itemgroupContent = @"
$globalasax
    <Content Include="Areas\Shared\**" />
    <Content Include="Areas\Core\**" />
    <Content Include="Views\**" />
    <Content Include="Scripts\**" />
    <Content Include="Content\**" />
    <Content Include="Assets\**" />
"@
Get-ChildItem "$webFolderName\*Web.csproj" | Foreach-Object {(Get-Content $_).replace($globalasax,$itemgroupContent)  | Out-File $_ -Encoding "UTF8" -Width 300 }
Write-Verbose "Web csproj file updated."
}

#######################################
$global:RemoveXmlElements = {
  Param ([xml]$xml, [string]$startsWithValue, [string]$xpathFormat = "//ns:Content[starts-with(@Include, '{0}')]" )
  $xpathSearchRemove = $xpathFormat -f $startsWithValue
  Write-Verbose "Remove: $xpathSearchRemove"
  $ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
  $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)

  $node = $xml.SelectSingleNode($xpathSearchRemove, $ns)
  while ($node -ne $null) {
      $node.ParentNode.RemoveChild($node)
      $node = $xml.SelectSingleNode($xpathSearchRemove, $ns)
  }
}

#######################################
function global:Format-XML ([xml]$xml, $indent=2) 
{ 
  $StringWriter = New-Object System.IO.StringWriter 
  $XmlWriter = New-Object System.XMl.XmlTextWriter $StringWriter 
  $xmlWriter.Formatting = “indented” 
  $xmlWriter.Indentation = $Indent 
  $xml.WriteContentTo($XmlWriter) 
  $XmlWriter.Flush() 
  $StringWriter.Flush() 
  Write-Output $StringWriter.ToString() 
}

################################################################################
# 3.5 Update the Web Artifacts
################################################################################
$global:UpdateWebArtifactsFiles = {
#Remove Artifacts
$webSubPaths.split(',') | Foreach-Object { get-childitem -path "$webFolderName\$_" -recurse | remove-item -force -recurse; Remove-Item "$webFolderName\$_" -Force -recurse }

#Copy Web Artifacts
$webSubPaths.split(',') | Foreach-Object { robocopy /E "$webAssetDirPath\$_" "$webFolderName\$_" }

#Copy MergeISVProject file
robocopy /is "$webAssetDirPath" "$webFolderName" MergeISVProject.exe

Write-Verbose "Web files added from generated solution."
}

################################################################################
# 5.1 WebForms
################################################################################
$global:AddReportFunctionality = {
$bundleReg='<Compile Include="BundleRegistration.cs" />'
$itemgroupCompile = @"
$bundleReg
    <Compile Include="WebForms\BaseWebPage.cs">
      <SubType>ASPXCodeBehind</SubType>
    </Compile>
    <Compile Include="WebForms\CustomReportViewer.aspx.cs">
      <DependentUpon>CustomReportViewer.aspx</DependentUpon>
      <SubType>ASPXCodeBehind</SubType>
    </Compile>
    <Compile Include="WebForms\CustomReportViewer.aspx.designer.cs">
      <DependentUpon>CustomReportViewer.aspx</DependentUpon>
    </Compile>
    <Compile Include="WebForms\ReportViewer.aspx.cs">
      <DependentUpon>ReportViewer.aspx</DependentUpon>
      <SubType>ASPXCodeBehind</SubType>
    </Compile>
    <Compile Include="WebForms\ReportViewer.aspx.designer.cs">
      <DependentUpon>ReportViewer.aspx</DependentUpon>
    </Compile>
"@
Get-ChildItem "$webFolderName\*Web.csproj" | Foreach-Object {(Get-Content $_).replace($bundleReg,$itemgroupCompile)  | Out-File $_ -Encoding "UTF8" -Width 300 }

$searchToReplace='<Content Include="Global.asax" />'
$itemgroupContent = @"
$searchToReplace
    <Content Include="WebForms\CustomReportViewer.aspx" />
    <Content Include="WebForms\ReportViewer.aspx" />
"@
Get-ChildItem "$webFolderName\*Web.csproj" | Foreach-Object {(Get-Content $_).replace($searchToReplace,$itemgroupContent)  | Out-File $_ -Encoding "UTF8" -Width 300 }

"WebForms" | Foreach-Object { robocopy /E "$webAssetDirPath\$_" "$webFolderName\$_" }

Get-ChildItem "$webFolderName\WebForms\CustomReportViewer.aspx" | Foreach-Object {(Get-Content $_).replace($searchReplaceWebFormNamespace,$replaceWebFormNamespace)  | Out-File $_ -Encoding "UTF8" -Width 300 }
Write-Verbose "Webforms folder added."
}

################################################################################
Write-Verbose "Update script blocks loaded."