// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    /// <summary> Information class to hold message information for wizard </summary>
    public class Info
    {
        public static string[] titles = 
         {
			@"Solution and Projects Upgrade
			
			  Sage 2017.1 to Sage 2017.2",
			"Synchronization of Web Files",
			"Refactor Razor Views to Use the GlobalLayout",
			"Remove Deprecated Methods",
			"Address Resx Files For Blank Values",
			"Ensure Bundle Names are Correct",
			"Synchronize Accpac Libraries",
			"Optionally Convert Web Project to Contain Module ID",
			"Confirmation"
         };

        public static string[] messages = 
         {
			@"This wizard will upgrade this solution and projects to Sage 300 2017.2 by performing the following steps:
 
			  Step 1: Synchronize of Web Files
			  Step 2: Refactor Razor Views to Use the GlobalLayout
			  Step 3: Remove Deprecated Methods
			  Step 4: Address Resx Files For Blank Values
			  Step 5: Ensure Bundle Names are Correct
			  Step 6: Synchronize Accpac Libraries
			  Step 7: Optionally Convert Web Project to Contain Module ID
			  Step 8: Conformation	



			Note:Before proceeding with the upgrade, ensure a backup of this solution and projects has been performed.




			Select 'Next' to view a summary of the individual steps to be performed.",
           
			@"The Synchronization of Web Files step will upgrade the various global files that are part of the solution and which has been included in the solution in order to compile and run the solution.

			These files are maintained by Sage and are potentially modified during the release cycle and therefore require synchronization.

			The following items are included in this step:

			  * Sync common files in web project
			  * Add new common JavaScript files 
			  * Upgrade third party JavaScript libraries
			  * Sync web content files
			  * Add new GlobalLayout.cshtml file for refactoring performed by Step 2",             

			@"A combination of 'R2' and 'R3' layouts have been implemented in the razor views. This step will replace any 'R2' and 'R3' layout and replace them with the GlobalLayout.cshtml reference.

			The razor views in the Areas/{module}/Views folder will also be refactored, if needed, for any changes required for the new layout.

			  * Implement new GlobalLayout.cshtml file
			  * Modify razor views for new layout, CSS changes, container changes, header group changes, fiscal group changes, for element and alignment changes.
			  


			Note: Please refer to Gloabl Layout Refactoring document for a detailed explanation of the steps being performed and for any manual refactoring that may not be performed by this step.", 

			@"Several methods were deprecated this release. While it is unlikely development partner will have used these methods, the code will be scanned and if any methods are discovered, they will be replaced with the new method signatures.

			  * Replace deprecated ResetLocks() with SessionLock()
			  * Replace deprecated UnLocks() with SessionUnLock()",

			@"Resx Files with blank values can yield unexpected results ranging from blank captions amd labels to 'keys' as opposed to 'values' being displayed in the Web Portal Menus.

			The 2017.2 version of the Solution and Code Genaration Wizards generated all 5 language files and thus if a partner did not provide translations for one of the languages and the langauge was selected in the browser, the unexpected results would be displayed.

			This step is informational only and does not perform any removal of unneeded language files. Please refer to the following article for information on this issue along with the required manual steps to resolve any uneeded Resx files:",

			@"JavaScript Bundles Names must not contains the dot('.') character. This is a known Microsoft issue and while Sage 300 bundle names do not include the dot character, a version of the wizard did include the dot character. Thus the JavaScript bundle names will be scanned and the dot character removed if found.

			  * Scan and correct bundle names in the registration classes 
			  * Scan and correct bundle names in the Index.cshtml files",

			@"In the 2017.2 release, the Accpac.Advantage library's reference was incremented from 6.4.0.0 to 6.4.0.20.
			
			In previous releases the Accpac.Advantage and Accpac.Advantage.Types libraries had the same reference. With 2017.2 release this is no longer the case. These versions are represented in the project files via the AccpacDotNetVersion.props, which has been modified this release to included the different reference numbers.

			This step will upgrade the existing props file and the csproj files for the upgraded props file and new reference number.",

			@"The 2017.2 version of the Solution Wizard will generate the web project with the module in the project name(i.e. ValuedPartner.TU.Web). In previous versions, the module was not added and caused issues with partners who have multiple modules.

			This step is optional to convert a web project without the module in the name to include it. However, these is no requirement to do so and may be skipped if not needed or desired.",
			
			@"Select 'Upgrade' to run the Sage Upgrade Wizard in order to convert a Sage 2017.1 Solution and Project files to version 2017.2. 
			
			A log file will be generated with the results of the upgrade process.

			Note: Please be sure you have a backup of the Solution and Project files before proceeding.",

			@"The Solution and Project Files have been upgrades to version 2017.2!

			Please review the upgrade log by selecting the 'Show Log' button below.

			Please review the upgrade instructions document for any manual steps not be performed by this wizard.

			Please reload this Solution in Visual Studio and re-complile the solution."

         };
    }
 }
