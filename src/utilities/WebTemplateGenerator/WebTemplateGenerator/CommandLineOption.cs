// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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

namespace WebTemplateGenerator
{
	/// <summary>
	/// Generic class used to hold a command-line option
	/// Supported types are:
	///		String
	///		Bool
	/// </summary>
	/// <typeparam name="T">The type of option (String | Bool)</typeparam>
	public class CommandLineOption<T>
    {
		/// <summary>
		/// This the name of the command-line option
		/// and is what would be specified on the command-line
		/// Example command-line argument: 
		///		--solutiondir="C:\MyProjects\MySolution\"
		///	Name:
		///		solutiondir
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// This the list of valid alternate names that
		/// can be used instead of the Name property
		/// These are defined when creating an object
		/// of this class.
		/// Example: 
		///		Name = "solutiondir"
		///		AliasList = "s", "sol", "sp", "solution"
		/// </summary>
		public List<string> AliasList { get; set; }

		/// <summary>
		/// This will hold the value of the command-line option
		/// Example command-line argument: 
		///		--solutiondir="C:\MyProjects\MySolution\"
		///	OptionValue
		///		"C:\MyProjects\MySolution\"
		/// </summary>
		public T OptionValue { get; set; }

		/// <summary>
		/// This will be the description of the command-line option
		/// and is used when the help is displayed
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// This will be used to hold a valid example of what
		/// this option will contain. This is used when the
		/// help is displayed.
		/// </summary>
		public string ExampleValue { get; set; }

		/// <summary>
		/// This will contain the error flag if there was a problem
		/// loading this option from the command-line.
		/// true  : An error occurred 
		/// false : No errors
		/// </summary>
		public bool LoadError { get; set; }
    }
}
