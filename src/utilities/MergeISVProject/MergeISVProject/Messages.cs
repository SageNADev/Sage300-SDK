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
	/// Container class for holding error and informational messages
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
			Error_MethodCalledWithInvalidParameter,

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
			Msg_Simulation,
			Msg_CopyingFile,
			Msg_SourceFolder,
			Msg_DestinationFolder,
			Msg_Only,
			Msg_FilesHaveBeenDeployedToLocalSage300Directory,
			Msg_DeploymentToSage300InstallationDisabled,

			Msg_DoNotCopyAssetsToSage300installationDirectory,
			Msg_GenerateALogFileInTheCurrentWorkingFolder,
			Msg_MicrosoftVisualStudioSolutionPath,
			Msg_MicrosoftVisualStudioSolutionWebProjectPath,
			Msg_MinifyJavascriptFiles,
			Msg_NetFrameworkPathContainingAspnetCompileDotExe,
			Msg_Sage300MenuDefinitionFileName,
			Msg_SimulateCopyingOfAssetsTo,
			Msg_VisualStudioProjectBuildConfiguration,
			Msg_Application,
			Msg_InnerException,
			Msg_ApplicationRunComplete,
            Msg_ApplicationRunCompleteWithErrors,
			Msg_SimulatedDeploymentOnly,
			Msg_LiveDeployment,
			Msg_CheckingForRegistryKeys,
			Msg_FilesFound,
			Msg_ApplicationModeOption
		}
	#endregion

	private static System.Resources.ResourceManager RM => Resources.ResourceManager;

		#region Public Properties (Errors)
		public static string Error_InvalidCommandLineParameters => RM.GetString(IndexEnum.Error_InvalidCommandLineParameters.ToString());
		public static string Error_Sage300WebFolderMissing => RM.GetString(IndexEnum.Error_Sage300WebFolderMissing.ToString());
		public static string Error_CouldNotCompileRazorViews => RM.GetString(IndexEnum.Error_CouldNotCompileRazorViews.ToString());
		public static string Error_InvalidBuildProfile => RM.GetString(IndexEnum.Error_InvalidBuildProfile.ToString());
		public static string Error_Sage300Missing => RM.GetString(IndexEnum.Error_Sage300Missing.ToString());
		public static string Error_DeploymentFolderLockedOrInUse => RM.GetString(IndexEnum.Error_DeploymentFolderLockedOrInUse.ToString());
		public static string Error_MinificationFailed => RM.GetString(IndexEnum.Error_MinificationFailed.ToString());
		public static string Error_UnableToFindTheProgram => RM.GetString(IndexEnum.Error_UnableToFindTheProgram.ToString());
		public static string Error_ErrorParsingOptionNoValueWasSet => RM.GetString(IndexEnum.Error_ErrorParsingOptionNoValueWasSet.ToString());
		public static string Error_ErrorParsingOptionTheFolderDoesNotExist => RM.GetString(IndexEnum.Error_ErrorParsingOptionTheFolderDoesNotExist.ToString());
		public static string Error_MethodCalledWithInvalidParameter => RM.GetString(IndexEnum.Error_MethodCalledWithInvalidParameter.ToString());

		#endregion

		#region Public Properties (Messages)
		public static string Msg_LogFileLocation => RM.GetString(IndexEnum.Msg_LogFileLocation.ToString());
		public static string Msg_LoggingStarted => RM.GetString(IndexEnum.Msg_LoggingStarted.ToString());
		public static string Msg_PrerequisitesAreValid => RM.GetString(IndexEnum.Msg_PrerequisitesAreValid.ToString());
		public static string Msg_ArgumentList => RM.GetString(IndexEnum.Msg_ArgumentList.ToString());
		public static string Msg_ProgramUsageMessage => RM.GetString(IndexEnum.Msg_ProgramUsageMessage.ToString());
		public static string Msg_FolderEquals => RM.GetString(IndexEnum.Msg_FolderEquals.ToString());
		public static string Msg_FilesDotCount => RM.GetString(IndexEnum.Msg_FilesDotCount.ToString());
		public static string Msg_DeleteFile => RM.GetString(IndexEnum.Msg_DeleteFile.ToString());
		public static string Msg_Rename1To2 => RM.GetString(IndexEnum.Msg_Rename1To2.ToString());
		public static string Msg_BeginningMinificationProcessOnDirectory => RM.GetString(IndexEnum.Msg_BeginningMinificationProcessOnDirectory.ToString());
		public static string Msg_RunningCommand => RM.GetString(IndexEnum.Msg_RunningCommand.ToString());
		public static string Msg_MinificationComplete => RM.GetString(IndexEnum.Msg_MinificationComplete.ToString());
		public static string Msg_RenamingJavascriptFilesBackToUsableState => RM.GetString(IndexEnum.Msg_RenamingJavascriptFilesBackToUsableState.ToString());
		public static string Msg_RenamingComplete => RM.GetString(IndexEnum.Msg_RenamingComplete.ToString());
		public static string Msg_MinificationSuccessful => RM.GetString(IndexEnum.Msg_MinificationSuccessful.ToString());
		public static string Msg_Begin => RM.GetString(IndexEnum.Msg_Begin.ToString());
		public static string Msg_End => RM.GetString(IndexEnum.Msg_End.ToString());
		public static string Msg_PreparingDeployFoldersAndFilesForStaging => RM.GetString(IndexEnum.Msg_PreparingDeployFoldersAndFilesForStaging.ToString());
		public static string Msg_PathExists => RM.GetString(IndexEnum.Msg_PathExists.ToString());
		public static string Msg_PathDeleted => RM.GetString(IndexEnum.Msg_PathDeleted.ToString());
		public static string Msg_PathCreated => RM.GetString(IndexEnum.Msg_PathCreated.ToString());
		public static string Msg_Simulation => RM.GetString(IndexEnum.Msg_Simulation.ToString());
		public static string Msg_CopyingFile => RM.GetString(IndexEnum.Msg_CopyingFile.ToString());
		public static string Msg_SourceFolder => RM.GetString(IndexEnum.Msg_SourceFolder.ToString());
		public static string Msg_DestinationFolder => RM.GetString(IndexEnum.Msg_DestinationFolder.ToString());
		public static string Msg_Only => RM.GetString(IndexEnum.Msg_Only.ToString());
		public static string Msg_FilesHaveBeenDeployedToLocalSage300Directory => RM.GetString(IndexEnum.Msg_FilesHaveBeenDeployedToLocalSage300Directory.ToString());
		public static string Msg_DeploymentToSage300InstallationDisabled => RM.GetString(IndexEnum.Msg_DeploymentToSage300InstallationDisabled.ToString());
		public static string Msg_DoNotCopyAssetsToSage300installationDirectory => RM.GetString(IndexEnum.Msg_DoNotCopyAssetsToSage300installationDirectory.ToString());
		public static string Msg_GenerateALogFileInTheCurrentWorkingFolder => RM.GetString(IndexEnum.Msg_GenerateALogFileInTheCurrentWorkingFolder.ToString());
		public static string Msg_MicrosoftVisualStudioSolutionPath => RM.GetString(IndexEnum.Msg_MicrosoftVisualStudioSolutionPath.ToString());
		public static string Msg_MicrosoftVisualStudioSolutionWebProjectPath => RM.GetString(IndexEnum.Msg_MicrosoftVisualStudioSolutionWebProjectPath.ToString());
		public static string Msg_MinifyJavascriptFiles => RM.GetString(IndexEnum.Msg_MinifyJavascriptFiles.ToString());
		public static string Msg_NetFrameworkPathContainingAspnetCompileDotExe => RM.GetString(IndexEnum.Msg_NetFrameworkPathContainingAspnetCompileDotExe.ToString());
		public static string Msg_Sage300MenuDefinitionFileName => RM.GetString(IndexEnum.Msg_Sage300MenuDefinitionFileName.ToString());
		public static string Msg_SimulateCopyingOfAssetsTo => RM.GetString(IndexEnum.Msg_SimulateCopyingOfAssetsTo.ToString());
		public static string Msg_VisualStudioProjectBuildConfiguration => RM.GetString(IndexEnum.Msg_VisualStudioProjectBuildConfiguration.ToString());
		public static string Msg_Application => RM.GetString(IndexEnum.Msg_Application.ToString());
		public static string Msg_InnerException => RM.GetString(IndexEnum.Msg_InnerException.ToString());
		public static string Msg_ApplicationRunComplete => RM.GetString(IndexEnum.Msg_ApplicationRunComplete.ToString());
        public static string Msg_ApplicationRunCompleteWithErrors => RM.GetString(IndexEnum.Msg_ApplicationRunCompleteWithErrors.ToString());
        public static string Msg_SimulatedDeploymentOnly => RM.GetString(IndexEnum.Msg_SimulatedDeploymentOnly.ToString());
		public static string Msg_LiveDeployment => RM.GetString(IndexEnum.Msg_LiveDeployment.ToString());
		public static string Msg_CheckingForRegistryKeys => RM.GetString(IndexEnum.Msg_CheckingForRegistryKeys.ToString());
		public static string Msg_FilesFound => RM.GetString(IndexEnum.Msg_FilesFound.ToString());
		public static string Msg_ApplicationModeOption => RM.GetString(IndexEnum.Msg_ApplicationModeOption.ToString());
        #endregion
    }
}
