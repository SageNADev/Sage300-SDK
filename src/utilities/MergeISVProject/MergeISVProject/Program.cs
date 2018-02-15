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
	/// <summary>
	/// Main program class
	/// </summary>
	public class Program
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
			Utilities.GetAppNameAndVersion(out string appName, out string appVersion);
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
			Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(_Options.UsageMessage);
			Console.ResetColor();
        }

        /// <summary>
        /// Display any errors encountered parsing command-line arguments
        /// passed into the program.
        /// </summary>
        private static void DisplayErrorsToConsole()
        {
            if (_Options.LoadErrors.Count > 0)
            {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("");
                Console.WriteLine(Messages.Error_InvalidCommandLineParameters);
				Console.WriteLine("");
				var errorMsg = _Options.GetLoadErrorsAsText();
                Console.WriteLine(errorMsg);
				Console.ResetColor();
            }
        }

		/// <summary>
		/// Write out some startup information to the log file and console
		/// </summary>
		private static void LogStartupInformation()
		{
			_Logger.Log(string.Format(Messages.Msg_LogFileLocation, _Logger.LogFile));
			_Logger.Log(Messages.Msg_LoggingStarted);
			_Logger.Log(Messages.Msg_PrerequisitesAreValid);
			_Logger.Log(Messages.Msg_ArgumentList);
			foreach (var s in _Options.Arguments) { _Logger.Log($"{new String(' ', 5)}{s}"); }
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
					LogStartupInformation();
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