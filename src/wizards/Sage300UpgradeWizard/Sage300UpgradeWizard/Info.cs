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
			
			  Sage 2017.2 to Sage 2018",
			"Synchronize Web Files",
			"Synchronize Accpac Libraries",
			"Confirmation"
         };

        public static string[] messages = 
         {
			@"The wizard will upgrade this solution and projects to Sage 300 2018 by performing the following steps:
 
			  Step 1: Synchronize web files
			  Step 2: Synchronize Accpac libraries
			  Step 3: Confirmation	


			Note:Before proceeding with the upgrade, ensure a backup of the solution and projects has been performed.




			Select 'Next' to view a summary of the individual steps.",
           
			@"This step will upgrade the various global files included in the solution.

			These files are maintained by Sage and if they are modified during the release cycle, they require synchronization.

			The following items are included in this step:

			  * Synchronize common files in web project
			  * Add new common JavaScript files 
			  * Upgrade third party JavaScript libraries
			  * Synchronize web content files",

			@"In the 2018 release, the Accpac.Advantage library's reference was incremented from 6.4.0.20 to 6.5.0.0.
			
			In previous releases, the Accpac.Advantage and Accpac.Advantage.Types libraries had the same reference number. With the 2017.2 release, this is no longer the case. These versions are represented in the project files via the AccpacDotNetVersion.props file, which has been modified to include the different reference numbers.

			This step will upgrade the existing props file and the csproj files with the new reference number.",

            @"Select 'Upgrade' to run the Sage Upgrade Wizard in order to convert the Sage 2017.2 solution and projects to version 2018. 
			
			A log file will be generated with the results.

			Note: Please be sure you have a backup of the Solution and Project files before proceeding.",

            @"The Solution and Project Files have been upgraded to version 2018!

			Please review the upgrade log by selecting the 'Show Log' button below.

			Please review the upgrade instructions document for any manual steps not performed by this wizard.

			The Sage 300 2018 Upgrade Wizard may be uninstalled by selecting Tools\Extensions and Updates...

			Please reload and recompile the solution in Visual Studio."

         };
    }
 }
