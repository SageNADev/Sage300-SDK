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

#region Imports
using System.Collections.Generic;
#endregion

namespace MergeISVProject.Interfaces
{
	public interface ICommandLineOptions
	{
		/// <summary>
		/// Returns the argument list
		/// </summary>
		string[] Arguments { get; }
		/// <summary>
		/// Character or characters that will prefix all incoming 
		/// command-line arguments
		/// </summary>
		string OptionPrefix { get; set; }

		/// <summary>
		/// The application name
		/// </summary>
		string ApplicationName { get; set; }

		/// <summary>
		/// The application version 
		/// </summary>
		string ApplicationVersion { get; set; }

		/// <summary>
		/// The list of all errors encountered while loading
		/// the command-line arguments
		/// </summary>
		List<string> LoadErrors { get; }

		/// <summary>
		/// How to call this application
		/// </summary>
		string UsageMessage { get; }

		/// <summary>
		/// The module Id
		/// </summary>
		string ModuleId { get; }

		// Required Command-Line Arguments

		/// <summary>
		/// The fully-qualified path to the Visual Studio solution 
		/// </summary>
		CommandLineOption<string> SolutionPath { get; set; }

		/// <summary>
		/// The fully-qualified path to the Visual Studio solution
		/// Web project
		/// </summary>
		CommandLineOption<string> WebProjectPath { get; set; }

		/// <summary>
		/// The name of the menu [XX]MenuDetails.xml
		/// </summary>
		CommandLineOption<string> MenuFilename { get; set; }

		/// <summary>
		/// The Visual Studio build profile name
		/// </summary>
		CommandLineOption<string> BuildProfile { get; set; }

		/// <summary>
		/// The fully-qualified path to the current Microsoft
		/// Dot Net Framework
		/// </summary>
		CommandLineOption<string> DotNetFrameworkPath { get; set; }

		// Optional Command-Line Arguments

		/// <summary>
		/// Enable | Disable minification of JavaScript files
		/// </summary>
		CommandLineOption<bool> Minify { get; set; }

		/// <summary>
		/// Enable | Disable the actual deployment 
		/// of files to the live Sage 300 installation
		/// </summary>
		CommandLineOption<bool> NoDeploy { get; set; }

		/// <summary>
		/// Enable | Disable run deployment in simulation mode
		/// No actual files will be copied.
		/// </summary>
		CommandLineOption<bool> TestDeploy { get; set; }

		/// <summary>
		/// Enable | Disable logging of errors and messages
		/// to a log file
		/// </summary>
		CommandLineOption<bool> Log { get; set; }

		/// <summary>
		/// Were there any errors during loading?
		/// </summary>
		/// <returns></returns>
		bool AnyErrors();

		/// <summary>
		/// Return a string containing all
		/// of the errors encountered during loading
		/// </summary>
		/// <returns></returns>
		string GetLoadErrorsAsText();
	}
}
