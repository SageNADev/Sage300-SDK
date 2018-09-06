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

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Build a list of lines from multiple files that match a predicate
        /// </summary>
        /// <param name="fileNames">The list of file paths</param>
        /// <param name="predicate">The method to use to check each line</param>
        /// <returns></returns>
        public static IEnumerable<string> FindLines(this IEnumerable<string> fileNames, 
                                                    Func<string, bool> predicate)
        {
            return fileNames.Select(fileName =>
            {
                using (var sr = new StreamReader(fileName))
                {
                    var line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (predicate(line))
                        {
                            return line;
                        }
                    }
                }
                return null;
            })
            .Where(line => !string.IsNullOrEmpty(line));
        }

        /// <summary>
        /// Build a list of files containing a particular string
        /// </summary>
        /// <param name="fileNames">The list of files to search in</param>
        /// <param name="predicate">The method to use to check each line</param>
        /// <returns>A list of filepaths that match the predicate</returns>
        public static IEnumerable<string> FindFilesContaining(this IEnumerable<string> fileNames, 
                                                              Func<string, bool> predicate)
        {
            return fileNames.Select(fileName =>
            {
                using (var sr = new StreamReader(fileName))
                {
                    var line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (predicate(line))
                        {
                            return fileName;
                        }
                    }
                }
                return null;
            })
            .Where(line => !string.IsNullOrEmpty(line));
        }

        /// <summary>
        /// Replace text in a list of files
        /// </summary>
        /// <param name="fileNames">The list of file paths</param>
        /// <param name="searchText">The text to search for</param>
        /// <param name="replacementText">The replacement text</param>
        public static void ReplaceTextInFiles(this IEnumerable<string> fileNames, 
                                              string searchText, 
                                              string replacementText)
        {
            foreach (var file in fileNames)
            {
                string text = File.ReadAllText(file);
                text = text.Replace(searchText, replacementText);
                File.WriteAllText(file, text);
            }
        }

        /// <summary>
        /// Get a list of all files in a directory 
        /// and optionally also get all files in all subdirectories
        /// </summary>
        /// <param name="dir">This is the starting directory as a string</param>
        /// <param name="includeSubDirectories">
        /// true : include subdirectories 
        /// false : root folder only
        /// </param>
        /// <returns>Returns the list of files and their paths</returns>
        public static IEnumerable<string> GetFileNames(this string dir, 
                                                       bool includeSubDirectories = true)
        {
            // Get the files in the root folder
            var rootFiles = Directory.EnumerateFiles(dir);

            // Optionally get the files in all subfolders
            if (includeSubDirectories == true)
            {
                rootFiles = rootFiles.Concat(Directory.EnumerateDirectories(dir)
                                                      .SelectMany(subdir => GetFileNames(subdir)));
            }

            return rootFiles;
        }
    }
}
