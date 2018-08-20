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
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities
{
    /// <summary>
    /// General utilities for dealing with the file system
    /// </summary>
    public static class FileUtilities
    {
        /// <summary>
        /// Build a list of filepaths based on a fileTypeFilter and an optional list of directories to ignore.
        /// This method is a wrapper for DirectoryInfo.EnumerateFiles()
        /// </summary>
        /// <param name="startingDirectory">Where shall this file search start?</param>
        /// <param name="fileTypeFilter">What types of files shall we look for?</param>
        /// <param name="ignoreDirectories">This is a list directories that we wish to ignore.</param>
        /// <returns>A list of files matching the fileTypeFilter with optionally removed directories</returns>
        public static IEnumerable<string> EnumerateFiles(DirectoryInfo startingDirectory,
                                                         string fileTypeFilter,
                                                         List<string> ignoreDirectories = null)
        {
            var results = startingDirectory.EnumerateFiles(fileTypeFilter, SearchOption.AllDirectories)
                                           .ToList<FileInfo>()
                                           .ConvertAll(x => (string)x.FullName);
            if (ignoreDirectories != null)
            {
                results.RemoveAll(f => ignoreDirectories.Exists(i => !String.IsNullOrWhiteSpace(i) && f.Contains(i)));
            }
            return results;
        }

        /// <summary>
        /// Build a list of filepaths based on a fileTypeFilter and an optional list of directories to ignore.
        /// This method is a wrapper for DirectoryInfo.EnumerateFiles()
        /// </summary>
        /// <param name="startingDirectory">The path to start the file search</param>
        /// <param name="fileTypeFilter">What types of files shall we look for?</param>
        /// <param name="ignoreDirectories">This is a list directories that we wish to ignore.</param>
        /// <returns>A list of files matching the fileTypeFilter with optionally removed directories</returns>
        public static IEnumerable<string> EnumerateFiles(string startingDirectory,
                                                         string fileTypeFilter,
                                                         List<string> ignoreDirectories = null)
        {
            var di = new DirectoryInfo(startingDirectory);
            var results = di.EnumerateFiles(fileTypeFilter, SearchOption.AllDirectories)
                            .ToList<FileInfo>()
                            .ConvertAll(x => (string)x.FullName);
            if (ignoreDirectories != null)
            {
                results.RemoveAll(f => ignoreDirectories.Exists(i => !String.IsNullOrWhiteSpace(i) && f.Contains(i)));
            }
            return results;
        }

        /// <summary>
        /// Create a folder if it doesn't yet exist.
        /// </summary>
        /// <param name="folderPath">This is the folder path as a string</param>
        public static void CreateFolderIfNotExists(string folderPath)
        {
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
