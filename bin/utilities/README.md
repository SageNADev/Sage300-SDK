# Utilities

## MergeISVProject.exe

This program is used by various wizards within the Sage 300 Web SDK as well as by Sage partner projects. It is used to take the compiled assets from an external Sage Partner project and bundle them up for deployment. It also provides javascript minification functionality and can optionally deploy the custom solution into a local Sage 300 installation for testing purposes.

When updating the MergeISVProject.exe application, ensure that you copy the application to the following folders and zip archives:

- \bin\utilities\MergeISVProject.exe [This folder]
- \src\wizards\Templates\Web\MergeISVProject.exe
- \src\wizards\Customization\Sage300UICustomizationSolution\ProjectTemplates\Web\MergeISVProject.exe
- \src\wizards\Sage300UpgradeWizard\Sage300UpgradeWizardPackage\ItemTemplates\Items.zip
- \src\wizards\Sage300UIWizardPackage\ProjectTemplates\Web.zip

## Sage300Utilties.exe

As of the 2018.2 release of the Sage 300 Web SDK, this program is used by the Sage300UIWizardPackage. It's purpose is to facilitate the rebuilding of all of the templates that exist in the \src\Templates\ folder of the SDK. In the future, this application will be enhanced to provide additional functionality to other wizards.

## Generating JavaScript for Grids

The Grid Enhancement logic that has been included in the samples projects has dramatically changed
the JavaScript requirements of a grid implementation. The documentation for the Generating JavaScript 
for Grids utility does not reflect these changes nor does the Generating JavaScript for Grids utility
itself. However, it is still valid for generating JavaScript which does not use the enhancemnet logic.
