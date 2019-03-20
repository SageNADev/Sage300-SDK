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
using WebTemplateGenerator.Interfaces;
using System.IO;
#endregion

namespace WebTemplateGenerator
{
	/// <summary>
	/// Some general file manipulation utilities
	/// </summary>
	public class FileUtilities
	{
		#region Private Variables
		private ILogger _Logger;
		#endregion

		#region Constructor(s)
		/// <summary>
		/// The primary constructor
		/// </summary>
		/// <param name="logger">An instance of the logger object</param>
		public FileUtilities(ILogger logger)
		{
			_Logger = logger;
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// Copy the contents of one directory to another
		/// </summary>
		/// <param name="sourceDirectory"></param>
		/// <param name="targetDirectory"></param>
		public void CopyDirectory(string sourceDirectory, string targetDirectory)
		{
			DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
			DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

			CopyAll(diSource, diTarget);
		}

		/// <summary>
		/// Copy the contents of one directory to another
		/// </summary>
		/// <param name="sourceDirectory">The source folder</param>
		/// <param name="targetDirectory">the target folder</param>
		public void CopyAll(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
		{
			Directory.CreateDirectory(targetDirectory.FullName);

			// Copy each file into the new directory.
			foreach (FileInfo fi in sourceDirectory.GetFiles())
			{
				var msg = $"Copying {targetDirectory.FullName}\\{fi.Name}";
				_Logger.LogInfo(msg);
				fi.CopyTo(Path.Combine(targetDirectory.FullName, fi.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in sourceDirectory.GetDirectories())
			{
				DirectoryInfo nextTargetSubDir =
					targetDirectory.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir);
			}
		}

		/// <summary>
		/// Recursively go through a directory
		/// removing all empty sub-directories
		/// </summary>
		/// <param name="startFolder">The directory to begin the search</param>
		public void RemoveEmptyDirectories(string startFolder)
		{
			foreach (var directory in Directory.GetDirectories(startFolder))
			{
				RemoveEmptyDirectories(directory);
				if (Directory.GetFiles(directory).Length == 0 &&
					Directory.GetDirectories(directory).Length == 0)
				{
					Directory.Delete(directory, false);
				}
			}
		}
		#endregion
	}
}
