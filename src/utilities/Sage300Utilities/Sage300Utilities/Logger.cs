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
using log4net;
using Sage300Utilities.Interfaces; 
#endregion

namespace Sage300Utilities.Logging
{
	/// <summary>
	/// Wrapper class for Log4Net
	/// </summary>
	public class Logger : ILogger
	{
		#region Private Variables
		private ILog _log4NetInstance;
		#endregion

		#region Public Properties
		public int DefaultIndentLevel { get { return 3; } }

		public int IndentLevel { get; set; }
		#endregion

		#region Constructor(s)
		/// <summary>
		/// 
		/// </summary>
		/// <param name="log4NetInstance"></param>
		public Logger(ILog log4NetInstance)
		{
			_log4NetInstance = log4NetInstance;
			ResetIndentLevel();
		}
		#endregion

		#region Public Methods
		public void ResetIndentLevel() => IndentLevel = 0;

		/// <summary>
		/// Log an information message
		/// </summary>
		/// <param name="m"></param>
		public void LogInfo(string msg)
		{
			_log4NetInstance.Info(ApplyIndent(msg));
		}

		/// <summary>
		/// Log an error message
		/// </summary>
		/// <param name="msg"></param>
		public void LogError(string msg)
		{
			_log4NetInstance.Error(ApplyIndent(msg));
		}

		/// <summary>
		/// Log a warning message
		/// </summary>
		/// <param name="msg"></param>
		public void LogWarning(string msg)
		{
			_log4NetInstance.Warn(ApplyIndent(msg));
		}

		/// <summary>
		/// Insert an empty line into the log file
		/// </summary>
		public void LogEmptyLine()
		{
			_log4NetInstance.Info(string.Empty);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Add some indentation to the message text
		/// prior to being logged.
		/// </summary>
		/// <param name="msgIn"></param>
		/// <returns></returns>
		private string ApplyIndent(string msgIn)
		{
			string msgOut = string.Empty;
			if (IndentLevel > 0)
			{
				msgOut = new string(' ', IndentLevel);
				msgOut += msgIn;
			}
			else
			{
				msgOut = msgIn;
			}
			return msgOut;
		}
		#endregion
	}
}
