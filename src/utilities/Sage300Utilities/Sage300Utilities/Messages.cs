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

namespace Sage300Utilities
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

			// Messages
			Msg_ProgramUsageMessage,
			Msg_Application,
			Msg_InnerException,
			Msg_CommandLineParameter_PreBuild,
			Msg_CommandLineParameter_EnableTemplateUpdates,
			Msg_CommandLineParameter_Help,
			Msg_CommandLineParameter_SDKRoot,
			Msg_CommandLineParameter_WebSource,
			Msg_CommandLineParameter_RebuildWebDotVstemplateFile,
		}
		#endregion

		private static System.Resources.ResourceManager RM => Resources.ResourceManager;

		#region Public Properties (Errors)
		public static string Error_InvalidCommandLineParameters => RM.GetString(IndexEnum.Error_InvalidCommandLineParameters.ToString());

		#endregion

		#region Public Properties (Messages)
		public static string Msg_ProgramUsageMessage => RM.GetString(IndexEnum.Msg_ProgramUsageMessage.ToString());
		public static string Msg_Application => RM.GetString(IndexEnum.Msg_Application.ToString());
		public static string Msg_InnerException => RM.GetString(IndexEnum.Msg_InnerException.ToString());
		public static string Msg_CommandLineParameter_PreBuild => RM.GetString(IndexEnum.Msg_CommandLineParameter_PreBuild.ToString());
		public static string Msg_CommandLineParameter_EnableTemplateUpdates => RM.GetString(IndexEnum.Msg_CommandLineParameter_EnableTemplateUpdates.ToString());
		public static string Msg_CommandLineParameter_Help => RM.GetString(IndexEnum.Msg_CommandLineParameter_Help.ToString());
		public static string Msg_CommandLineParameter_SDKRoot => RM.GetString(IndexEnum.Msg_CommandLineParameter_SDKRoot.ToString());
		public static string Msg_CommandLineParameter_WebSource => RM.GetString(IndexEnum.Msg_CommandLineParameter_WebSource.ToString());
		public static string Msg_CommandLineParameter_RebuildWebDotVstemplateFile => RM.GetString(IndexEnum.Msg_CommandLineParameter_RebuildWebDotVstemplateFile.ToString());
		#endregion
	}
}

// EOF