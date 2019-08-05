# Utilities

## WebTemplateGenerator.exe

As of the 2020.0 release of the Sage 300 Web SDK, this program is used by the Sage300UICustomizationWizard (Plugin)
and the Sage300UIWizardPackage (solution wizard). 

It's purpose is to facilitate the rebuilding of the Web.vstemplate files that exist in 
src\wizards\CustomizationWizardTemplates\Web (customization wizard) and 
src\wizards\Templates\Web (solution wizard). 

## MergeISVProject.exe

This program is used by various wizards within the Sage 300 Web SDK as well as by Sage partner
projects. It is used to take the compiled assets from an external Sage Partner project and bundle
them up for deployment. It also provides JavaScript minification functionality and can optionally 
deploy the solution into a local Sage 300 installation for testing purposes when the solution is 
in **Release** mode.

When updating the MergeISVProject.exe application, ensure that you copy the application to the 
following folders and zip archives:

- \bin\utilities\MergeISVProject.exe [This folder]
- \src\wizards\Templates\Web\MergeISVProject.exe
- \src\wizards\Sage300UpgradeWizard\Sage300UpgradeWizardPackage\ItemTemplates\Items.zip
- \src\wizards\Sage300UIWizardPackage\ProjectTemplates\Web.zip

> As a reminder, when building a solution in **Release** mode be sure to either re-start IIS or stop IIS
prior to building the solution. This is important if you have previously built in **Release** mode and 
have accessed the Sage 300 web screens as the worker role will be holding onto some assemblies and
the deployment of these files to your local Sage 300's **Worker** folder may not be successful. Be 
sure to re-start IIS after the deployment if you have stopped it prior to the build.

## Generating JavaScript for Grids

The Grid Enhancement logic that has been included in the samples projects has dramatically changed
the JavaScript requirements of a grid implementation. The documentation for the Generating JavaScript 
for Grids utility does not reflect these changes nor does the Generating JavaScript for Grids utility
itself. However, it is still valid for generating JavaScript which does not use the enhancemnet logic.

## Sage300InquiryConfigurationGenerator

As of the 2019.1 release of the Sage 300 Web SDK, a new program called Sage300InquiryConfigurationGenerator 
has been included. The purpose of this program is to aid in the creation of SQL scripts used to configure 
new Inquiry screens. In the future, this program will be replaced with a more full-featured wizard type application.
