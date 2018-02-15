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

namespace MergeISVProject
{
	/// <summary>
	/// Container to hold error messages
	/// </summary>
	public static class Messages
	{
		#region Private Enumerations and Variables
		private enum IndexEnum
		{
			// Errors
			Error_InvalidCommandLineParameters = 0,
			Error_Sage300WebFolderMissing,
			Error_CouldNotCompileRazorViews,
			Error_InvalidBuildProfile,
			Error_Sage300Missing,
			Error_DeploymentFolderLockedOrInUse,
			Error_MinificationFailed,
			Error_UnableToFindTheProgram,
			Error_ErrorParsingOptionNoValueWasSet,
			Error_ErrorParsingOptionTheFolderDoesNotExist,

			// Messages
			Msg_LogFileLocation,
			Msg_LoggingStarted,
			Msg_PrerequisitesAreValid,
			Msg_ArgumentList,
			Msg_ProgramUsageMessage,
			Msg_FolderEquals,
			Msg_FilesDotCount,
			Msg_DeleteFile,
			Msg_Rename1To2,
			Msg_BeginningMinificationProcessOnDirectory,
			Msg_RunningCommand,
			Msg_MinificationComplete,
			Msg_RenamingJavascriptFilesBackToUsableState,
			Msg_RenamingComplete,
			Msg_MinificationSuccessful,
			Msg_Begin,
			Msg_End,
			Msg_PreparingDeployFoldersAndFilesForStaging,
			Msg_PathExists,
			Msg_PathDeleted,
			Msg_PathCreated,
		}
		#endregion

		private static System.Resources.ResourceManager RM { get { return Resources.ResourceManager; }}

		#region Public Properties (Errors)
		public static string Error_InvalidCommandLineParameters { get { return RM.GetString(IndexEnum.Error_InvalidCommandLineParameters.ToString()); } }
		public static string Error_Sage300WebFolderMissing { get { return RM.GetString(IndexEnum.Error_Sage300WebFolderMissing.ToString()); } }
		public static string Error_CouldNotCompileRazorViews { get { return RM.GetString(IndexEnum.Error_CouldNotCompileRazorViews.ToString()); } }
		public static string Error_InvalidBuildProfile { get { return RM.GetString(IndexEnum.Error_InvalidBuildProfile.ToString()); } }
		public static string Error_Sage300Missing { get { return RM.GetString(IndexEnum.Error_Sage300Missing.ToString()); } }
		public static string Error_DeploymentFolderLockedOrInUse { get { return RM.GetString(IndexEnum.Error_DeploymentFolderLockedOrInUse.ToString()); } }
		public static string Error_MinificationFailed { get { return RM.GetString(IndexEnum.Error_MinificationFailed.ToString()); } }
		public static string Error_UnableToFindTheProgram { get { return RM.GetString(IndexEnum.Error_UnableToFindTheProgram.ToString()); } }
		public static string Error_ErrorParsingOptionNoValueWasSet { get { return RM.GetString(IndexEnum.Error_ErrorParsingOptionNoValueWasSet.ToString()); } }
		public static string Error_ErrorParsingOptionTheFolderDoesNotExist { get { return RM.GetString(IndexEnum.Error_ErrorParsingOptionTheFolderDoesNotExist.ToString()); } }
		#endregion

		#region Public Properties (Messages)
		public static string Msg_LogFileLocation { get { return RM.GetString(IndexEnum.Msg_LogFileLocation.ToString()); } }
		public static string Msg_LoggingStarted { get { return RM.GetString(IndexEnum.Msg_LoggingStarted.ToString()); } }
		public static string Msg_PrerequisitesAreValid { get { return RM.GetString(IndexEnum.Msg_PrerequisitesAreValid.ToString()); } }
		public static string Msg_ArgumentList { get { return RM.GetString(IndexEnum.Msg_ArgumentList.ToString()); } }
		public static string Msg_ProgramUsageMessage { get { return RM.GetString(IndexEnum.Msg_ProgramUsageMessage.ToString()); } }
		public static string Msg_FolderEquals { get { return RM.GetString(IndexEnum.Msg_FolderEquals.ToString()); } }
		public static string Msg_FilesDotCount { get { return RM.GetString(IndexEnum.Msg_FilesDotCount.ToString()); } }
		public static string Msg_DeleteFile { get { return RM.GetString(IndexEnum.Msg_DeleteFile.ToString()); } }
		public static string Msg_Rename1To2 { get { return RM.GetString(IndexEnum.Msg_Rename1To2.ToString()); } }
		public static string Msg_BeginningMinificationProcessOnDirectory { get { return RM.GetString(IndexEnum.Msg_BeginningMinificationProcessOnDirectory.ToString()); } }
		public static string Msg_RunningCommand { get { return RM.GetString(IndexEnum.Msg_RunningCommand.ToString()); } }
		public static string Msg_MinificationComplete { get { return RM.GetString(IndexEnum.Msg_MinificationComplete.ToString()); } }
		public static string Msg_RenamingJavascriptFilesBackToUsableState { get { return RM.GetString(IndexEnum.Msg_RenamingJavascriptFilesBackToUsableState.ToString()); } }
		public static string Msg_RenamingComplete { get { return RM.GetString(IndexEnum.Msg_RenamingComplete.ToString()); } }
		public static string Msg_MinificationSuccessful { get { return RM.GetString(IndexEnum.Msg_MinificationSuccessful.ToString()); } }
		public static string Msg_Begin { get { return RM.GetString(IndexEnum.Msg_Begin.ToString()); } }
		public static string Msg_End { get { return RM.GetString(IndexEnum.Msg_End.ToString()); } }
		public static string Msg_PreparingDeployFoldersAndFilesForStaging { get { return RM.GetString(IndexEnum.Msg_PreparingDeployFoldersAndFilesForStaging.ToString()); } }
		public static string Msg_PathExists { get { return RM.GetString(IndexEnum.Msg_PathExists.ToString()); } }
		public static string Msg_PathDeleted { get { return RM.GetString(IndexEnum.Msg_PathDeleted.ToString()); } }
		public static string Msg_PathCreated { get { return RM.GetString(IndexEnum.Msg_PathCreated.ToString()); } }
		#endregion
	}
}
