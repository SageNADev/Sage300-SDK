// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
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

namespace MergeISVProject.Errors
{
	public static class ErrorMessages
	{
		#region Private Enumerations and Variables
		private enum _msgIndexEnum
		{
			InvalidCommandLineParameters = 0,
			Sage300WebFolderMissing,
			CouldNotCompileRazorViews,
			InvalidBuildProfile,
			Sage300Missing,
			DeploymentFolderLockedOrInUse,
			MinificationFailed
		}

		private static string[] _msg = 
		{
			"Invalid or missing command-line parameters",

			"The post-build utility MergeISVProject could not find the Online Web folder for the Web UIs. " +
			"While the build was successful, the deployment was unsuccessful. Therefore, check view(s) for " +
			"issue(s) (i.e. localization syntax).",

			"The post-build utility MergeISVProject could not compile the razor view(s). While the build " +
			"was successful, the deployment was unsuccessful. Therefore, check view(s) for issue(s) " +
			"(i.e. localization syntax).",

			"Invalid build profile specified. 'Release' mode must be used.",

			"Sage 300 does not appear to be installed. This is a required application.",

			"Sorry, the folder 'Deploy' could not be deleted. It appears as though the 'Deploy' folder is locked " +
			"or in use. Please ensure that there are no command prompts or File Explorer instances referring " +
			"to the 'Deploy' folder (or any of it's sub-folders).",

			"Sorry, the minification process appears to have failed."
		};
		#endregion

		#region Public Properties
		public static string InvalidCommandLineParameters
		{ 
			get { return _msg[(int)_msgIndexEnum.InvalidCommandLineParameters]; }
		}
		public static string Sage300WebFolderMissing
		{
			get { return _msg[(int)_msgIndexEnum.Sage300WebFolderMissing]; }
		}
		public static string CouldNotCompileRazorViews
		{
			get { return _msg[(int)_msgIndexEnum.CouldNotCompileRazorViews]; }
		}
		public static string InvalidBuildProfile
		{
			get { return _msg[(int)_msgIndexEnum.InvalidBuildProfile]; }
		}
		public static string Sage300Missing
		{
			get { return _msg[(int)_msgIndexEnum.Sage300Missing]; }
		}
		public static string DeploymentFolderLockedOrInUse
		{
			get { return _msg[(int)_msgIndexEnum.DeploymentFolderLockedOrInUse]; }
		}
		public static string MinificationFailed
		{
			get { return _msg[(int)_msgIndexEnum.MinificationFailed]; }
		}

		#endregion
	}
}
