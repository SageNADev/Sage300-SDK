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
using MergeISVProject.Interfaces;

#endregion

namespace MergeISVProject.CustomExceptions
{
	/// <summary>
	/// General Application Exception Class
	/// </summary>
	class MergeISVProjectException : Exception
    {
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public MergeISVProjectException()
        {
        }

		/// <summary>
		/// Constructor accepting multiple arguments
		/// </summary>
		/// <param name="logger">An instance of the Logger object</param>
		/// <param name="message">The exception message to log</param>
		public MergeISVProjectException(ILogger logger, string message)
            : base(message)
        {
			logger.LogError(message);
        }

		/// <summary>
		/// Constructor accepting multiple arguments including an inner exception
		/// </summary>
		/// <param name="logger">An instance of the Logger object</param>
		/// <param name="message">The exception message to log</param>
		/// <param name="inner">The inner exeption object</param>
		public MergeISVProjectException(ILogger logger, string message, Exception inner)
            : base(message, inner)
        {
			logger.LogError($"[{Messages.Msg_InnerException}] : {inner.Message}");
			logger.LogError(message);
		}

		#endregion
	}
}


