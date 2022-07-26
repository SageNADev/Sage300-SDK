set RootDrive=D:
set SDKRoot=%RootDrive%\Projects\Sage300-SDK\Develop
set WizardSourceRoot=%SDKRoot%\src\wizards
set BuildConfiguration=Release

REM Setup Visual Studio environment variables first
REM set VSVersion=2022
set VSVersion=2019
REM set VSVersion=2017

REM VS 2019
call "C:\Program Files (x86)\Microsoft Visual Studio\%VSVersion%\Enterprise\Common7\Tools\VsDevCmd.bat"

REM VS 2022
REM call "C:\Program Files\Microsoft Visual Studio\%VSVersion%\Enterprise\Common7\Tools\VsDevCmd.bat"

REM --------------------------------------------------------------------------------------------------------
REM (UI Wizards) Sage 300 Solution and Code Generation Wizards
cd %WizardSourceRoot%\Sage300UIWizardPackage
set sln=Sage300UIWizardPackage.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Upgrade Wizard
cd %WizardSourceRoot%\Sage300UpgradeWizard\Sage300UpgradeWizardPackage
set sln=Sage300UpgradeWizardPackage.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM UI Customization Wizard (Plugin)
cd %WizardSourceRoot%\Customization\Sage300UICustomizationWizard
set sln=Sage300UICustomizationWizard.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Customization Wizard (Standalone)
cd %WizardSourceRoot%\Customization\Sage300CustomizationWizard
set sln=Sage300CustomizationWizard.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%

REM --------------------------------------------------------------------------------------------------------
REM Language Resource Wizard
cd %WizardSourceRoot%\Sage300LanguageResourceWizard\Sage300LanguageResourceWizardPackage
set sln=Sage300LanguageResourceWizardPackage.sln
msbuild.exe %sln% /t:Rebuild /p:Configuration=%BuildConfiguration%
