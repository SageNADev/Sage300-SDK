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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
#endregion

namespace MergeISVProject.Logging
{
	/// <summary>
	/// A class to handle logging errors and message to a file
	/// </summary>
	public class Logger : ILogger
	{
		#region Constants and Readonly 
		// Double width line characters
		//private const byte METHODBLOCK_START_ASCIICODE = 201;
		//private const byte METHODBLOCK_END_ASCIICODE = 200;
		//private const byte METHODBLOCK_VERTICAL_ASCIICODE = 186;
		//private const byte METHODBLOCK_HORIZONTALLINE_ASCIICODE = 205;

		// Single line characters
		private const byte METHODBLOCK_START_ASCIICODE = 218;
		private const byte METHODBLOCK_END_ASCIICODE = 192;
		private const byte METHODBLOCK_VERTICAL_ASCIICODE = 179;
		private const byte METHODBLOCK_HORIZONTALLINE_ASCIICODE = 196;

		private static readonly string METHODBLOCK_HORIZONTAL = Utilities.ASCII8ToString(asciiCode: METHODBLOCK_HORIZONTALLINE_ASCIICODE);
		private readonly string METHODBLOCK_START = Utilities.ASCII8ToString(asciiCode: METHODBLOCK_START_ASCIICODE) + METHODBLOCK_HORIZONTAL;
		private readonly string METHODBLOCK_END = Utilities.ASCII8ToString(asciiCode: METHODBLOCK_END_ASCIICODE) + METHODBLOCK_HORIZONTAL;
		private readonly string METHODBLOCK_VERTICAL = Utilities.ASCII8ToString(asciiCode: METHODBLOCK_VERTICAL_ASCIICODE);
		private const int DEFAULT_INDENT_SPACES = 3;
		private const string DATETIME_FORMAT = "yyyyMMddHHmmss";
		private static readonly string divider = new string('-', 80);
		#endregion

		#region Private Variables
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
				IncreaseOrDecreaseIndentationLevel(value);
			}
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Increase or decrease the indentation level by a fixed amount
		/// </summary>
		/// <param name="increase">
		/// true = increase indentation level
		/// false = decrease indentation level
		/// </param>
		private void IncreaseOrDecreaseIndentationLevel(bool increase)
		{
			if (increase)
			{
				indentLevel += DEFAULT_INDENT_SPACES;
			}
			else
			{
				indentLevel -= DEFAULT_INDENT_SPACES;
				if (indentLevel < 0) { indentLevel = 0; }
			}
		}
		#endregion

		#region Public Properties

		/// <summary>
		/// Get or Set the fully-qualified path and name of the log file
		/// </summary>
		public string LogFile { get; set; }

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logfilename">The name of the log file</param>
		/// <param name="logfolder">The location to store the log file</param>
		/// <param name="enabled">true = enable logging | false = disable logging</param>
		public Logger(string logfilename, string logfolder, bool enabled)
		{
			this.Enabled = enabled;
			this.LogFile = Path.Combine(logfolder, logfilename);
			if (File.Exists(LogFile)) { DeleteLog(); }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Delete the log file
		/// </summary>
		/// <param name="path">The fully qualified path to the log file directory</param>
		/// <returns>Fully qualified path to the log file</returns>
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
		/// Delete the log file
		/// </summary>
		public string DeleteLog()
		{
			return DeleteLog(Directory.GetCurrentDirectory());
		}

		/// <summary>
		/// Write to log file
		/// </summary>
		/// <param name="path">The fully qualified path to the log file directory</param>
		/// <param name="messageIn">The error message to add to the log</param>
		/// <param name="applyTimeStamp">
		/// Optional Parameter : true = add timestamp | false = do not add timestamp
		/// </param>
		public void LogError(string path, string messageIn, bool applyTimeStamp = true)
		{
			if (!Enabled) return;
			try
			{
				// Write out to log file
				var fileName = Path.Combine(path, LogFile);
				var indent = indentLevel < 0 ? 0 : indentLevel;

				var temp = 0;
				if (indent >= DEFAULT_INDENT_SPACES)
				{
					temp = indent / DEFAULT_INDENT_SPACES;
				}

				var bars = string.Empty;
				if (temp > 0)
				{
					for (var x = 0; x < temp; x++)
					{
						bars += $"{METHODBLOCK_VERTICAL}{new string(' ', DEFAULT_INDENT_SPACES)}";
					}
				}

				var message = $"{bars}{messageIn}";
				var msg = (applyTimeStamp) ? DateTime.Now.ToString(DATETIME_FORMAT) + ": " + message : message;
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
		/// Write to log file
		/// </summary>
		/// <param name="messageIn">The error message to add to the log</param>
		/// <param name="applyTimeStamp">
		/// Optional Parameter : true = add timestamp | false = do not add timestamp
		/// </param>
		public void LogError(string messageIn, bool applyTimeStamp = true)
		{
			try
			{
				LogError(Directory.GetCurrentDirectory(), messageIn, applyTimeStamp);
			}
			catch
			{
				// Ignore error
			}
		}

		/// <summary>
		/// Write to the log file
		/// </summary>
		/// <param name="messageIn">The error message to add to the log</param>
		/// <param name="applyTimeStamp">
		/// Optional Parameter : true = add timestamp | false = do not add timestamp
		/// </param>
		public void Log(string messageIn, bool applyTimeStamp = true)
		{
			LogError(messageIn: messageIn);
		}

		/// <summary>
		/// Write a list of strings to the log file
		/// </summary>
		/// <param name="messagesIn">A list of strings</param>
		/// <param name="applyTimeStamp">
		/// Optional Parameter : true = add timestamp | false = do not add timestamp
		/// </param>
		public void Log(IEnumerable<string> messagesIn, bool applyTimeStamp = true)
		{
			foreach (var m in messagesIn)
			{
				Log(m, applyTimeStamp);
			}
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
			Log("");
			Log($"{METHODBLOCK_START} {Messages.Msg_Begin}: {methodName}");

			// This will increase the indentation level by one unit
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
			Log($"{METHODBLOCK_END} {Messages.Msg_End}: {methodName}");
			Log("");
		}
		#endregion
	}
}
