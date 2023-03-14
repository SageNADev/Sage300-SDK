set RootDrive=C:

set SDKRoot=%RootDrive%\Projects\Sage300-SDK\Develop
set WizardSourceRoot=%SDKRoot%\src\wizards
set TemplateSourceRoot=%SDKRoot%\src\wizards\Templates
set UtilitiesSourceRoot=%SDKRoot%\src\utilities
set BuildConfiguration=Release

REM Setup Visual Studio environment variables first
set VSVersion=2019
call "C:\Program Files (x86)\Microsoft Visual Studio\%VSVersion%\Enterprise\Common7\Tools\VsDevCmd.bat"

REM --------------------------------------------------------------------------------------------------------
REM Rebuild MergeISVProject.exe Utility
REM --------------------------------------------------------------------------------------------------------
cd %UtilitiesSourceRoot%\MergeISVProject
set sln=MergeISVProject.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Rebuild UIWizards and UpgradeWizard Templates (and get ready to bundle with Wizards)
REM --------------------------------------------------------------------------------------------------------
cd %TemplateSourceRoot%\UIWizards
call NoWebSync_RebuildAndDeployAll.bat
cd %TemplateSourceRoot%\UpgradeWizard
call NoWebSync_RebuildAndDeployAll.bat

REM --------------------------------------------------------------------------------------------------------
REM Rebuild (UI Wizards) Sage 300 Solution and Code Generation Wizards
REM --------------------------------------------------------------------------------------------------------
cd %WizardSourceRoot%\Sage300UIWizardPackage
set sln=Sage300UIWizardPackage.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Rebuild Upgrade Wizard
REM --------------------------------------------------------------------------------------------------------
cd %WizardSourceRoot%\Sage300UpgradeWizard\Sage300UpgradeWizardPackage
set sln=Sage300UpgradeWizardPackage.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Rebuild UI Customization Wizard (Plugin)
REM --------------------------------------------------------------------------------------------------------
cd %WizardSourceRoot%\Customization\Sage300UICustomizationWizard
set sln=Sage300UICustomizationWizard.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Rebuild Customization Wizard (Standalone)
REM --------------------------------------------------------------------------------------------------------
cd %WizardSourceRoot%\Customization\Sage300CustomizationWizard
set sln=Sage300CustomizationWizard.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Rebuild Language Resource Wizard
REM --------------------------------------------------------------------------------------------------------
cd %WizardSourceRoot%\Sage300LanguageResourceWizard\Sage300LanguageResourceWizardPackage
set sln=Sage300LanguageResourceWizardPackage.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

