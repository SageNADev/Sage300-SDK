################################################################################
# 3.1 Initialize Powershell variables
################################################################################
# - EDIT AS REQUIRED -
# The path to the temporary Sage 300 SDK 2017.1 generated solution
# e.g. "c:\2017pu1SDK-Generated\Sage300CMvcApplication1"
$global:tempWizardGenPath = <path to temp gen 2017.1 sdk solution>
# The namespace value before the ".Web" for the Web Projects namespace
# e.g. "ValuedPartner"
$global:nameSpaceBeforeWeb = <name before .Web>

# - should not require editing -
# Web project
$global:webFolderName = "$nameSpaceBeforeWeb.Web"
$global:webAssetDirPath = "$tempWizardGenPath\$webFolderName"
$global:webSubPaths = 'Areas\Shared,Areas\Core,Views,Scripts,Content,Assets'
# Webforms
$global:replaceWebFormNamespace = 'Inherits="Sage.CA.SBS.ERP.Sage300.Web'
$global:searchReplaceWebFormNamespace = 'Inherits="' + $webFolderName
# Verbose
$global:verbosepreference = 'continue'

################################################################################
# 3.2 Update the .NET Framework
################################################################################
$global:UpdateDotNetFramework = {
# Update .NET Framework for the project files (*.csproj)
Get-ChildItem -Recurse *.csproj | Foreach-Object {(Get-Content $_).replace('<TargetFrameworkVersion>v4.5.1</TargetFrameworkVersion>','<TargetFrameworkVersion>v4.6.2</TargetFrameworkVersion>')  | Out-File $_ -Encoding "UTF8" }
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
}

################################################################################
# 4.X WebForms
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
    <Content Include="WebForms\*.aspx" />
"@
Get-ChildItem "$webFolderName\*Web.csproj" | Foreach-Object {(Get-Content $_).replace($searchToReplace,$itemgroupContent)  | Out-File $_ -Encoding "UTF8" -Width 300 }

"WebForms" | Foreach-Object { robocopy /E "$webAssetDirPath\$_" "$webFolderName\$_" }

Get-ChildItem "$webFolderName\WebForms\CustomReportViewer.aspx" | Foreach-Object {(Get-Content $_).replace($searchReplaceWebFormNamespace,$replaceWebFormNamespace)  | Out-File $_ -Encoding "UTF8" -Width 300 }
}

################################################################################
Write-Verbose "Update scripts loaded."