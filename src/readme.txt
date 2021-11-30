-------------------------------------------------------------------------
-         Rebuilding MergeISVProject, Templates and Wizards
-------------------------------------------------------------------------

The batch file 'NoWebSync_RebuildAndDeployAll.bat' is used to do the following:

1. Rebuild the utility MergeISVProject.exe and copy it to the following locations:

	\bin\Utilities\
	\src\wizards\Templates\UIWizards\Web\
	\src\wizards\Templates\UpgradeWizard\Items\

2. Rebuild the Web templates for the following wizards:
   Note: This will NOT synchronize the Columbus-Web assets
         If this is required, this needs to be done explicitly.

	UIWizards (Solution and Code Generation Wizards)
	UpgradeWizard

3. Rebuild the following wizards:

	Sage300LanguageResourceWizardPackage.vsix
	Sage.CA.SBS.ERP.Sage300.CustomizationWizard.vsix
	Sage300UICustomizationSolution.vsix
	Sage300UpgradeWizardPackage.vsix
	Sage300UIWizardPackage.vsix
	
