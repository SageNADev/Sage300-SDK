# Utilities

## WebTemplateGenerator.exe

As of the 2019.2 release of the Sage 300 Web SDK, this program is used by the Sage300UICustomizationWizard (Plugin). 
It's purpose is to facilitate the rebuilding of the Web.vstemplate file that exists in the 
src\wizards\CustomizationWizardTemplates\Web\ folder of the SDK. 

## MergeISVProject.exe

This program is used by various wizards within the Sage 300 Web SDK as well as by Sage partner
projects. It is used to take the compiled assets from an external Sage Partner project and bundle
them up for deployment. It also provides JavaScript minification functionality and can optionally 
deploy the solution into a local Sage 300 installation for testing purposes when the solution is 
in **Release** mode.

> As a reminder, when building a solution in **Release** mode be sure to either re-start IIS or stop IIS
prior to building the solution. This is important if you have previously built in **Release** mode and 
have accessed the Sage 300 web screens as the worker role will be holding onto some assemblies and
the deployment of these files to your local Sage 300's **Worker** folder may not be successful. Be 
sure to re-start IIS after the deployment if you have stopped it prior to the build.

## Sage300Utilties.exe

As of the 2018.2 release of the Sage 300 Web SDK, this program is used by the Sage300UIWizardPackage. 
It's purpose is to facilitate the rebuilding of all of the templates that exist in the \src\Templates\ 
folder of the SDK. In the future, this application will be enhanced to provide additional functionality 
to other wizards.

## Generating JavaScript for Grids

The Grid Enhancement logic that has been included in the samples projects has dramatically changed
the JavaScript requirements of a grid implementation. The documentation for the Generating JavaScript 
for Grids utility does not reflect these changes nor does the Generating JavaScript for Grids utility
itself. However, it is still valid for generating JavaScript which does not use the enhancemnet logic.

## Sage300InquiryConfigurationGenerator

As of the 2019.1 release of the Sage 300 Web SDK, a new program called Sage300InquiryConfigurationGenerator 
has been included. The purpose of this program is to aid in the creation of SQL scripts used to configure 
new Inquiry screens. In the future, this program will be replaced with a more full-featured wizard type application.
