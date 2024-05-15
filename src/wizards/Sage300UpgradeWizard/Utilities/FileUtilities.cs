// The MIT License (MIT) 
// Copyright (c) 1994-2020 The Sage Group plc or its licensors.  All rights reserved.
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
            /// <summary>
            /// Create a folder if it doesn't yet exist.
            /// </summary>
            /// <param name="folderPath">This is the folder path as a string</param>
            /// 
        }

        /// <summary>
        /// Create a folder if it doesn't yet exist.
        /// </summary>
        /// <param name="folderPath">This is the folder path as a string</param>
        /// <returns>true = folder already existed | false = folder did not already exist</returns>
        public static bool CreateFolderIfNotExists(string folderPath)
        {
            var folderAlreadyExisted = Directory.Exists(folderPath);
            if (folderAlreadyExisted == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderAlreadyExisted;
        }


        /// <summary>
        /// Returns the parent folder of another folder
        /// Example:
        ///    Input = C:\path1\path2\path3\
        ///    Output = C:\path1\path2\
        /// </summary>
        /// <param name="path">The string representation of a path to parse for its parent folder</param>
        /// <returns>The parent path as a string</returns>
        public static string GetParentPathFromPath(string path)
        {
            var parentPath = string.Empty;

            if (path.Length < 1) return path;

            var parts = path.Split(new char[] { Path.DirectorySeparatorChar });
            for (var index = 0; index < parts.Length - 1; index++)
            {
                parentPath += parts[index] + Path.DirectorySeparatorChar.ToString();
            }

            return parentPath;
        }

        /// <summary>
        /// Remove an existing file
        /// </summary>
        /// <param name="filePath">The file path specification</param>
        public static void RemoveExistingFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                //Log($"{Resources.File} '{filePath}' {Resources.Exists}.");
                File.Delete(filePath);
                //Log($"{Resources.File} '{filePath}' {Resources.Deleted}.");
            }
            else
            {
                // Do nothing. File doesn't actually exist

                //Log($"{Resources.File} '{filePath}' {Resources.DoesNotExist}.");
            }
        }

        /// <summary> Copy folder and files </summary>
        /// <param name="sourceDirectoryName">Source directory name</param>
        /// <param name="destinationDirectoryName">Destination directory name</param>
        public static void DirectoryCopy(string sourceDirectoryName, string destinationDirectoryName, bool ignoreDestinationFolder = true)
        {
            var dir = new DirectoryInfo(sourceDirectoryName);
            var dirs = dir.GetDirectories();

            // Create directory if not exists
            if (!Directory.Exists(destinationDirectoryName))
            {
                Directory.CreateDirectory(destinationDirectoryName);
            }

            // Iterate files
            foreach (var file in dir.GetFiles())
            {
                try
                {
                    var filePath = Path.Combine(destinationDirectoryName, file.Name);
                    file.CopyTo(filePath, true);

                    // Log detail
                    //Log($"{Resources.AddReplaceFile} {filePath}");
                }
                catch (IOException e)
                {
                    // Likely just a locked file.
                    // Just log it and move on.
                    var msg = e.Message;
                    //Log($"{Resources.ExceptionThrownPossibleLockedFile} : {file.FullName.ToString()}");
                    //Log($"{msg}");
                }
            }

            // For recursion
            foreach (DirectoryInfo subdir in dirs)
            {
                var subdirectoryName = subdir.FullName;
                if (ignoreDestinationFolder)
                {
                    if (subdirectoryName != destinationDirectoryName)
                    {
                        DirectoryCopy(subdirectoryName, Path.Combine(destinationDirectoryName, subdir.Name));
                    }
                }
                else
                {
                    DirectoryCopy(subdirectoryName, Path.Combine(destinationDirectoryName, subdir.Name));
                }
            }
        }

        /// <summary>
        /// Move the specified directory up one level from the sourceFolder
        /// Example:
        ///     sourceFolder =    C:\projects\Sage300-SDK\2020.2 (Read Only)\samples\SegmentCodes
        ///     directoryToMove = C:\projects\Sage300-SDK\2020.2 (Read Only)\samples\SegmentCodes\SegmentCodes-Backup-20200513-114518
        ///     
        /// Result:
        ///     destinationFolder = C:\projects\Sage300-SDK\2020.2 (Read Only)\samples\SegmentCodes-Backup-20200513-114518
        /// </summary>
        /// <param name="sourceFolder">The source folder where the directory to move currently lives</param>
        /// <param name="directoryToMove">The name of the directory to move</param>
        /// <returns>The fully-qualified path to the final destination folder</returns>
        public static string MoveDirectoryUpOneLevel(string sourceFolder, string directoryToMove)
        {
            var sourceDirectory = directoryToMove;

            // Extract just the source directory name (without the full path)
            var parts = sourceDirectory.Split('\\');
            var sourceDirectoryNameOnly = parts[parts.Length - 1].Trim();

            // Determine the directory one level up from the sourceFolder
            var destinationDirectory = new DirectoryInfo(sourceFolder).Parent.FullName;
            destinationDirectory = Path.Combine(destinationDirectory, sourceDirectoryNameOnly);

            Directory.Move(sourceDirectory, destinationDirectory);

            return destinationDirectory;
        }

        /// <summary>
        /// Create an empty text file and ensure it's closed immediately.
        /// </summary>
        /// <param name="filename">The fully-qualified path to the directory and name where the file should be created</param>
        public static void CreateEmptyFile(string filename)
        {
            File.Create(filename).Dispose();
        }

        /// <summary>
        /// Replace a piece of text with another in a file
        /// </summary>
        /// <param name="filePath">The file to alter</param>
        /// <param name="searchFor">The string to search for</param>
        /// <param name="replaceWith">The string replacement</param>
        public static void ReplaceTextInFile(string filePath, string searchFor, string replaceWith)
        {
            if (File.Exists(filePath))
            {
                // Load the file content
                string lines = File.ReadAllText(filePath);

                // Replace the text
                var alteredLines = lines.Replace(searchFor, replaceWith);

                // Save the text back to the file
                File.WriteAllText(filePath, alteredLines);
            }
        }
    }
}
