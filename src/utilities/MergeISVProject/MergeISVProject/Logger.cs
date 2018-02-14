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

using MergeISVProject.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

#endregion

namespace MergeISVProject.Logging
{
	public class Logger : ILogger
	{
		#region Constants
		private const int DEFAULT_INDENT_SPACES = 5;
		#endregion

		#region Private Variables
		private string divider = new string('-', 80);
		private int indentLevel = 0;
		#endregion

		#region Private Properties
		private bool Enabled { get; set; }

		private bool _enableIndentation;
		private bool EnableIndentation
		{
			get
			{
				return _enableIndentation;
			}
			set
			{
				_enableIndentation = value;
				if (value)
				{
					// Request to enable indentation

					indentLevel += DEFAULT_INDENT_SPACES;
				}
				else
				{
					// Request to disable indentation

					// Reduce the indentation level
					indentLevel -= DEFAULT_INDENT_SPACES;
					if (indentLevel < 0) { indentLevel = 0; }
				}
			}
		}
		#endregion

		#region Public Properties
		public string LogFile { get; set; }

		#endregion

		#region Constructor(s)

		public Logger(string logfilename, string logfolder, bool enabled)
		{
			this.Enabled = enabled;
			this.LogFile = Path.Combine(logfolder, logfilename);
			if (File.Exists(LogFile)) { DeleteLog(); }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Delete Log
		/// </summary>
		/// <param name="path">Path to file</param>
		/// <returns>Fully qualified file name</returns>
		public string DeleteLog(string path)
		{
			if (Enabled)
			{
				var fileName = Path.Combine(path, LogFile);

				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}

				return fileName;
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Delete Log
		/// </summary>
		/// <param name="path">Path to file</param>
		/// <returns>Fully qualified file name</returns>
		public string DeleteLog()
		{
			return DeleteLog(Directory.GetCurrentDirectory());
		}

		/// <summary>
		/// Write to Log
		/// </summary>
		/// <param name="path">Path to file</param>
		/// <param name="messageIn">Error message</param>
		public void LogError(string path, string messageIn, bool applyTimeStamp = true)
		{
			if (!Enabled) return;
			try
			{
				// Write out to log file
				var fileName = Path.Combine(path, LogFile);
				var indent = indentLevel < 0 ? 0 : indentLevel;
				//var message = (EnableIndentation == true) ? $"{new string(' ', indent)}{messageIn}" : messageIn;
				var message = $"{new string(' ', indent)}{messageIn}";
				var msg = (applyTimeStamp) ? DateTime.Now + ": " + message : message;
				//var msg = (applyTimeStamp) ? DateTime.Now + ": " + messageIn : messageIn;
				File.AppendAllLines(fileName, new[] { msg });

				// Write out to the console
				Console.WriteLine(msg);
			}
			catch
			{
				// Ignore error
			}
		}

		/// <summary>
		/// Write to Log
		/// </summary>
		/// <param name="path">Path to file</param>
		/// <param name="message">Error message</param>
		public void LogError(string message, bool applyTimeStamp = true)
		{
			try
			{
				LogError(Directory.GetCurrentDirectory(), message, applyTimeStamp);
			}
			catch
			{
				// Ignore error
			}
		}

		/// <summary>
		/// Write to Log (Simple logging)
		/// </summary>
		/// <param name="path">Path to file</param>
		/// <param name="message">The Message to log</param>
		public void Log(string message)
		{
			LogError(message: message);
		}

		/// <summary>
		/// Display the log file (to default .log (or txt) viewer)
		/// </summary>
		public void ShowLog()
		{
			if (!Enabled) return;
			var currentFolder = Directory.GetCurrentDirectory();
			var filename = Path.Combine(currentFolder, LogFile);
			if (File.Exists(filename))
			{
				var p = new Process
				{
					StartInfo =
				{
					UseShellExecute = true,
					FileName = filename
				}
				};
				p.Start();
				p.WaitForExit();
			}
		}

		/// <summary>
		/// Log the method name as a header
		/// </summary>
		/// <param name="methodName">The method name</param>
		public void LogMethodHeader(string methodName)
		{
			if (!Enabled) return;

			// This will increase the indentation level by one unit
			// Initially, this is set to a negative value
			Log("");
			Log(divider);
			Log($"Begin: {methodName}");
			EnableIndentation = true;
		}

		/// <summary>
		/// Log the method name as a footer
		/// </summary>
		/// <param name="methodName">The method name</param>
		public void LogMethodFooter(string methodName)
		{
			if (!Enabled) return;

			// This will reduce the indentation level by one unit
			EnableIndentation = false;
			Log($"End: {methodName}");
			Log("");
		}
		#endregion
	}
}
