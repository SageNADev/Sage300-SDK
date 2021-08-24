// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Collections;
using System.IO;
using System.Resources;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Utilities
{
    /// <summary>
    /// A class to manage Resx files.
    /// </summary>
    public class ResXManager
    {
        #region Private Variables
        private string _filePath;
        private bool _fileExists;
        private Hashtable _resourceEntries;
        #endregion

        #region Constructor
        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="filePath">The path to the resource file</param>
        public ResXManager(string filePath)
        {
            _resourceEntries = new Hashtable();

            if (filePath.Length > 0)
            {
                _fileExists = File.Exists(filePath);
                _filePath = filePath;

                if (_fileExists)
                {
                    Load();
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Load the resource entries
        /// </summary>
        public void Load()
        {
            // Get existing resources
            using (var reader = new ResXResourceReader(_filePath))
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        _resourceEntries.Add(d.Key.ToString(), string.Empty);
                    else
                        _resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
            }
        }

        /// <summary>
        /// Determine whether or not a key exists
        /// </summary>
        /// <param name="key">The string-based key</param>
        /// <returns></returns>
        public bool EntryExists(string key)
        {
            return _resourceEntries.ContainsKey(key);
        }

        /// <summary>
        /// Insert a key/value pair into the resource object
        /// </summary>
        /// <param name="key">The key of the resource entry</param>
        /// <param name="value">The value of the resource entry</param>
        public void Insert(string key, string value)
        {
            _resourceEntries.Add(key, value);
        }

        /// <summary>
        /// Insert a key/value pair into the resource object
        /// if it does not yet exist
        /// </summary>
        /// <param name="key">The key of the resource entry</param>
        /// <param name="value">The value of the resource entry</param>
        public void InsertIfNotExist(string key, string value)
        {
            if (_resourceEntries.ContainsKey(key) == false)
            {
                Insert(key, value);
            }
        }

        /// <summary>
        /// Sage the resource file
        /// </summary>
        public void Write()
        {
            using (var writer = new ResXResourceWriter(_filePath))
            {
                foreach (string key in _resourceEntries.Keys)
                {
                    var node = new ResXDataNode(key, _resourceEntries[key]);
                    writer.AddResource(node);
                }
                writer.Generate();
            }
        }
        #endregion
    }
}
