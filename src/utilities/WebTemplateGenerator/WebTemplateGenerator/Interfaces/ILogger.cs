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
#endregion

namespace WebTemplateGenerator.Interfaces
{
	/// <summary>
	/// The ILogger interface definition
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Set this to the default indent level (number of spaces)
		/// </summary>
		int DefaultIndentLevel
		{
			get;
		}

		/// <summary>
		/// This property will allow getting and setting of
		/// the current indent level
		/// </summary>
		int IndentLevel
		{
			get;
			set;
		}

		/// <summary>
		/// Set the indent level back to zero
		/// </summary>
		void ResetIndentLevel();

		/// <summary>
		/// Log an informational message
		/// </summary>
		/// <param name="message">The messagse text</param>
		void LogInfo(string message);

		/// <summary>
		/// Log a warning
		/// </summary>
		/// <param name="message">The messagse text</param>
		void LogWarning(string message);

		/// <summary>
		/// Log an error
		/// </summary>
		/// <param name="message">The messagse text</param>
		void LogError(string message);

		/// <summary>
		/// Log an empty line
		/// </summary>
		void LogEmptyLine();
	}
}
