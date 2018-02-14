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

using System;
using System.IO;
using MergeISVProject.CustomExceptions;
using MergeISVProject.Interfaces;
using MergeISVProject.Logging;

#endregion

namespace MergeISVProject
{
	class Program
    {
		#region Constants
		const string LOGFILENAME = @"MergeISVProject.log";
		#endregion

		#region Class Variables
		private static ILogger _Logger;
		private static ICommandLineOptions _Options;
		private static MergeISVProjectDriver _Driver;
        #endregion

		#region Private Methods
		/// <summary>
		/// Create all the necessary components
		/// </summary>
		/// <param name="args">The command-line arguments list</param>
		private static void InitializeComponents(string[] args)
		{
			GetAppNameAndVersion(out string appName, out string appVersion);
			_Options = new CommandLineOptions(appName, appVersion, args);
			_Logger = new Logger(logfilename: LOGFILENAME,
									logfolder: Directory.GetCurrentDirectory(),
									enabled: _Options.Log.OptionValue);
			_Logger.Log($"Application: {_Options.ApplicationName} V{_Options.ApplicationVersion}");
			_Logger.Log(" ");
		}

		/// <summary>
		/// Display the program usage information to the console
		/// </summary>
		private static void DisplayUsageMessageToConsole() 
        {
            Console.WriteLine(_Options.UsageMessage);
        }

        /// <summary>
        /// Display any errors encountered parsing command-line arguments
        /// passed into the program.
        /// </summary>
        private static void DisplayErrorsToConsole()
        {
            if (_Options.LoadErrors.Count > 0)
            {
				Console.WriteLine("");
                Console.WriteLine($"Errors were encountered attempting to parse command-line arguments:");
				Console.WriteLine("");
				var errorMsg = _Options.GetLoadErrorsAsText();
                Console.WriteLine(errorMsg);
            }
        }

        /// <summary>
        /// Get the name of this application and it's version number
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="ver">Application Version</param>
        private static void GetAppNameAndVersion(out string name, out string ver)
        {
            name = typeof(Program).Assembly.GetName().Name + ".exe";
            ver = typeof(Program).Assembly.GetName().Version.ToString();
        }

		#endregion

		#region Public Method (Main)

		/// <summary>
		/// It all starts here folks!
		/// </summary>
		/// <param name="args">The command-line arguments</param>
		public static void Main(string[] args)
		{
			var bypassLogfileDisplay = false;
			try
			{
				InitializeComponents(args);

				if (_Options.AnyErrors())
				{
					DisplayUsageMessageToConsole();
					DisplayErrorsToConsole();

					// We don't want the log file to be displayed
					// so we'll prevent that from happening
					bypassLogfileDisplay = true;

					// Wait...
					Console.ReadLine();
				}
				else
				{
					_Driver = new MergeISVProjectDriver(_Options, _Logger);

					_Logger.Log($"Log File Location = {_Logger.LogFile}");
					_Logger.Log("Logging started...");
					_Logger.Log("Prerequisites are valid.");
					_Logger.Log("Argument List: ");
					foreach (var s in _Options.Arguments) { _Logger.Log($"    {s}"); }

					// Start Processing...
					_Driver.Run();
				}
			}
			catch (MergeISVProjectException)
			{
				// Errors have already been logged to log file.
			}
			finally
			{
				if (!bypassLogfileDisplay)
				{
					_Logger.ShowLog();
				}
			}
		}
		#endregion
	}
}