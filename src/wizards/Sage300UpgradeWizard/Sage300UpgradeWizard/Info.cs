// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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
			"Synchronize Web Files",
			"Refactor Razor Views to use the Global Layout",
			"Remove Deprecated Methods",
			"Address Resx Files for Blank Values",
			"Ensure Bundle Names are Correct",
			"Synchronize Accpac Libraries",
			"Convert Web Project to Contain Module ID (optional)",
			"Confirmation"
         };

        public static string[] messages = 
         {
			@"The wizard will upgrade this solution and projects to Sage 300 2017.2 by performing the following steps:
 
			  Step 1: Synchronize web files
			  Step 2: Refactor razor views to use the Global Layout
			  Step 3: Remove deprecated methods
			  Step 4: Address resx files for blank values
			  Step 5: Ensure bundle names are correct
			  Step 6: Synchronize Accpac libraries
			  Step 7: Convert web project to contain module id (optional)
			  Step 8: Confirmation	



			Note:Before proceeding with the upgrade, ensure a backup of the solution and projects has been performed.




			Select 'Next' to view a summary of the individual steps.",
           
			@"This step will upgrade the various global files included in the solution.

			These files are maintained by Sage and if they are modified during the release cycle, they require synchronization.

			The following items are included in this step:

			  * Synchronize common files in web project
			  * Add new common JavaScript files 
			  * Upgrade third party JavaScript libraries
			  * Synchronize web content files
			  * Add new GlobalLayout.cshtml file for refactoring performed by Step 2",             

			@"A combination of 'R2' and 'R3' layouts have been implemented in the razor views. This step will replace any 'R2' and 'R3' layout with the 'Global Layout' reference.

			If needed, the razor views in the Areas/{module}/Views folder will also be refactored for any changes required by the new layout.

			  * Implement new GlobalLayout.cshtml file
			  * Modify razor views for new layout, CSS changes, container changes, header group changes, fiscal group changes, element and alignment changes.
			  


			Note: Please refer to the Global Layout Refactoring document for a detailed explanation of the steps being performed and for any manual refactoring that may not be performed by this step.", 

			@"Several methods were deprecated this release. While it is unlikely that development partners will have used these methods, the code will be scanned and if any methods are discovered, they will be replaced with new method signatures.

			  * Replace deprecated 'ResetLocks' with 'SessionLock'
			  * Replace deprecated 'UnLocks' with 'SessionUnLock'",

			@"Resx files with blank values can yield unexpected results such as blank captions and labels and 'keys' as opposed to 'values' being displayed in the Web Portal Menus.

			The 2017.1 version of the Solution Wizard and Code Generation Wizard generate all 5 language files and thus if a partner did not provide a translation for one of the languages and the langauge was selected in the browser, an unexpected result would be displayed.

			This step is informational only and does not perform any removal of unneeded language files. Please refer to the following article for information on this issue along with the required manual steps to resolve any uneeded Resx files:",

			@"JavaScript bundle names must not contain the dot('.') character. This is a known Microsoft issue and while Sage 300 bundle names do not include the dot character, a version of the wizard did produce the dot character in the bundle names. Therefore the JavaScript bundle names will be scanned and the dot character removed if found.

			  * Scan and correct bundle names in the registration classes 
			  * Scan and correct bundle names in the Index.cshtml files",

			@"In the 2017.2 release, the Accpac.Advantage library's reference was incremented from 6.4.0.0 to 6.4.0.20.
			
			In previous releases, the Accpac.Advantage and Accpac.Advantage.Types libraries had the same reference number. With the 2017.2 release, this is no longer the case. These versions are represented in the project files via the AccpacDotNetVersion.props file, which has been modified to include the different reference numbers.

			This step will upgrade the existing props file and the csproj files with the new reference number.",

			@"The 2017.2 version of the Solution Wizard will generate the web project with the module in the project name (i.e. ValuedPartner.TU.Web). In previous versions, the module was not added and caused issues with partners who have multiple modules.

			This step is optional to convert a web project, without the module in the name, to add the module to the name. However, there is no requirement to do so and this step may be skipped.",
			
			@"Select 'Upgrade' to run the Sage Upgrade Wizard in order to convert the Sage 2017.1 solution and projects to version 2017.2. 
			
			A log file will be generated with the results.

			Note: Please be sure you have a backup of the Solution and Project files before proceeding.",

            @"The Solution and Project Files have been upgraded to version 2017.2!

			Please review the upgrade log by selecting the 'Show Log' button below.

			Please review the upgrade instructions document for any manual steps not performed by this wizard.

            The Sage 300 2017.2 Upgrade Wizard may be uninstalled by selecting Tools\Extensions and Updates...

			Please reload and recompile the solution in Visual Studio."

         };
    }
 }
