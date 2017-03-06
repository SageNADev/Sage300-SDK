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
			"Summary",
			"Synchronization of Web Files",
			"R2/R3 Layout Changes",
			"Process Obsoleted Methods",
			"Upgrade MergeISVProject",
			"Resx Files with Blank Values",
			"Remove Dot in Bundle Name",
			"Upgrade Accpac.NET library reference",
			"Convert Web Project to Module Specific"
         };

        public static string[] messages = 
         {
			@"This upgrade wizard will upgrade Sage300c PU1 version projects and samples code to PU2 version. It includes the following steps:
 
			  1. Synchronization of Web project files
			  2. R2/R3 Layout Changes
			  3. Process obsoleted methods
			  4. Upgrade MergeISVProject
			  5. Resx Files with blank values
			  6. Remove dot in bundle name
			  7. Upgrade reference to new Accpac.NET library
			  8. Convert web project to contain module name

			For upgrade the projects and samples, it's better to backup the original projects and samples.",
           
			@"Synchronization of web project files

			  1. Add/update Sage300c common JavaScript files
			  2. Upgrade third party JavaScript libraries to new version
			  3. Add/update Sage300c web content files
			  4. Add new GlobalLayout.cshtml",             

			@"In the Areas/{module}/Views razor view (.cshtml file), apply following changes:

			  1. Refactoring shared layout with GlobalLayout.cshtml
			  2. Modify razor view with new layout, css
			  3. Modify razor view with new container
			  4. Modify razor view with new headers group
			  5. Modify razor view with new fiscal group
			  6. Modify razor view with new css for form elements
			  7. Modify razor view with new css for right alignment", 

			@"Iterate/Search through c# code files and find any reference calls to obsoleted methods.

			Remove the obsoleted methods, replace with new methods

			  1. Replace obsoleted method ResetLocks() with SessionLock()
			  2. Replace obsoleted method UnLocks() with SessionUnLock()",

			@"Copy new version MergeISVProject to solution web project folder. 

			In the web project property, build events tab page, the post-build event command line as:

			Call $(ProjectDir)MergeISVProject.exe $(SolutionPath) $(ProjectDir) {module}MenuDetails.xml $(ConfigurationName) $(FrameworkDir)
            
			This file is executed after release build. It will compile the razor views and deploy to local Sage300c for RAD development. New argument is added to specify whether to deploy to Sage 300c when built in release mode.

			Usage :

			Call $(ProjectDir)MergeISVProject.exe args0 args1 args2 args3 args4 args5

			  args0 is the solution file path
			  args1 is the web project path
			  args2 is the menu file name (i.e. XXMenuDetails.xml)
			  args3 is the configuration name (i.e. debug or release)
			  args4 is the framework directory containing the aspnet_compile.exe
			  args5 is the optional parameter as /nodeploy , default will deploy",

			@"Resx Files with blank values.

			In previous version Solution and Code Genaration wizard, it will generate the Menu Resx files and any screen Resx files for all supported languages. But, the values in these non-English Resx files will contain keys, but blank values, which is not ideal for external developers. It must be resolved to contain a value.

			Details see JT's blog:
 
			https://jthomas903.wordpress.com/2017/01/24/sage-300-optional-resource-files/",

			@"Remove dot in bundles name if it contains dot.

			In previouse version wizard to generated projects and samples, it's incorrectly use dot in the bundle name, it need correct this usages

			  1. Bundle name in bundle registration classes 
			  2. Bundle name in Index.cshtml files",

			@"Upgrade reference to new Accpac.NET library.
			
			In PU2 the Accpac.NET library in changed, the projects and samples projects need update the project reference to use new version",

			@"Convert web project to module specific web project.
            
			Using the PU1 and previous vesion solution wizard to generate the projects, the project name like {Company}.{Modlue}.{XXX} except the Web project.
 
			If you want to convert the web prject from {Company}.Web project to {Company}.{Module}.Web for consistent with other projects, please check
			the following check box."
         };
    }
 }
