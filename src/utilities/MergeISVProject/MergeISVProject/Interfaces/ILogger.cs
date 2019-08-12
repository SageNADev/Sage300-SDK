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
	/// <summary>
	/// The ILogger interface definition
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Get or Set the log file name and path
		/// </summary>
		string LogFile { get; set; }

		/// <summary>
		/// Log a message to the log file
		/// </summary>
		/// <param name="message">The message text</param>
		/// <param name="timestamp">Optional: true = include timestamp | false = do not include timestamp</param>
		void Log(string message, bool timestamp=true);

		/// <summary>
		/// Log a list of messages to the log file
		/// </summary>
		/// <param name="messages">The list of messages</param>
		/// <param name="timestamp">Optional: true = include timestamp | false = do not include timestamp</param>
		void Log(IEnumerable<string> messages, bool timestamp = true);

		/// <summary>
		/// Log a method name to the log file as a header
		/// </summary>
		/// <param name="methodName">The name of the method</param>
		void LogMethodHeader(string methodName);

		/// <summary>
		/// Log a method name to the log file as a footer
		/// </summary>
		/// <param name="methodName">The name of the method</param>
		void LogMethodFooter(string methodName);

		/// <summary>
		/// Delete the log file using the default path
		/// </summary>
		/// <returns>The name of the log file</returns>
		string DeleteLog();

		/// <summary>
		/// Delete the log file from a specific path
		/// </summary>
		/// <param name="path">The fully-qualified path to the log file</param>
		/// <returns>The name of the log file</returns>
		string DeleteLog(string path);

		/// <summary>
		/// Log an error to the log file
		/// </summary>
		/// <param name="path">The fully-qualified path to the log file</param>
		/// <param name="message">The message text</param>
		/// <param name="timestamp">Optional: true = include timestamp | false = do not include timestamp</param>
		void LogError(string path, string message, bool applyTimeStamp = true);

		/// <summary>
		/// Log an error to the log file
		/// </summary>
		/// <param name="message">The message text</param>
		/// <param name="timestamp">Optional: true = include timestamp | false = do not include timestamp</param>
		void LogError(string message, bool applyTimeStamp = true);

		/// <summary>
		/// Display the log file (to default .log (or txt) viewer)
		/// </summary>
		void ShowLog();
	}
}
