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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
#endregion

namespace MergeISVProject
{
	/// <summary>
	/// These are general utility methods
	/// </summary>
	public static class Utilities
	{
		#region Constants
		// IBM437 (OEM United States)
		private const int DefaultCodePage = 437;
		#endregion

		#region Public Methods
		/// <summary>
		/// When this method is called from another class' method,
		/// this method will return the name of the calling
		/// method. How's that for a mouthful!
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetCurrentMethod()
		{
			var st = new StackTrace();
			var sf = st.GetFrame(1);
			return sf.GetMethod().Name;
		}

		/// <summary>
		/// Get the string equivalant of an ascii character
		/// </summary>
		/// <param name="data">byte array containing the character code</param>
		/// <returns>the string representation of the ascii character</returns>
		public static string ASCII8ToString(byte[] data)
		{
			return Encoding.GetEncoding(DefaultCodePage).GetString(data);
		}

		/// <summary>
		/// Get the string equivalent of an ascii character
		/// </summary>
		/// <param name="data">byte containing the character code</param>
		/// <returns>the string representation of the ascii character</returns>
		public static string ASCII8ToString(byte data)
		{
			var byteArray = new byte[] { data };
			return Encoding.GetEncoding(DefaultCodePage).GetString(byteArray);
		}

		/// <summary>
		/// Get the name of this application and it's version number
		/// Values are returned via output string parameters
		/// </summary>
		/// <param name="name">Application Name</param>
		/// <param name="ver">Application Version</param>
		public static void GetAppNameAndVersion(out string name, out string ver)
		{
			name = typeof(Program).Assembly.GetName().Name + ".exe";
			ver = typeof(Program).Assembly.GetName().Version.ToString();
		}
		#endregion
	}
}
