set SDKRoot=C:\Projects\Sage300-SDK\Develop

REM Setup Visual Studio environment variables first
call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsDevCmd.bat"


REM Sage 300 Solution and Code Generation Wizards
cd %SDKRoot%\src\Wizards\
cd Sage300UIWizardPackage
set solutionFileName=Sage300UIWizardPackage.sln
set buildConfigurationName=Release
msbuild.exe %solutionFileName% /t:Rebuild /p:Configuration=%buildConfigurationName%

REM Sage 300 Upgrade Wizard
cd %SDKRoot%\src\Wizards\
cd Sage300UpgradeWizard\Sage300UpgradeWizardPackage
set solutionFileName=Sage300UpgradeWizardPackage.sln
set buildConfigurationName=Release
msbuild.exe %solutionFileName% /t:Rebuild /p:Configuration=%buildConfigurationName%

REM Sage 300 Language Resource Wizard
cd %SDKRoot%\src\Wizards\
cd Sage300LanguageResourceWizard\Sage300LanguageResourceWizardPackage
set solutionFileName=Sage300LanguageResourceWizardPackage.sln
set buildConfigurationName=Release
msbuild.exe %solutionFileName% /t:Rebuild /p:Configuration=%buildConfigurationName%

REM Sage 300 UI Customization Wizard (Plugin)
cd %SDKRoot%\src\Wizards\
cd Customization\Sage300UICustomizationWizard
set solutionFileName=Sage300UICustomizationWizard.sln
set buildConfigurationName=Release
msbuild.exe %solutionFileName% /t:Rebuild /p:Configuration=%buildConfigurationName%

REM Sage 300 Customization Wizard (Standalone)
cd %SDKRoot%\src\Wizards\
cd Customization\Sage300CustomizationWizard
set solutionFileName=Sage300CustomizationWizard.sln
set buildConfigurationName=Release
msbuild.exe %solutionFileName% /t:Rebuild /p:Configuration=%buildConfigurationName%

