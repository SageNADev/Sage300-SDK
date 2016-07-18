// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

namespace Sage.CA.SBS.ERP.Sage300.ResxGeneration
{
    /// <summary> Process Generation Class (worker) </summary>
    class ProcessGeneration
    {
        #region Private Vars
        /// <summary> Resource information is from a file, entered manually or a combination of both </summary>
        private BindingList<ResourceInfo> _resourceInfo;
        
        /// <summary> Settings from UI (languages, overwrite) </summary>
        private Settings _settings;
        
        /// <summary> Invalid characters to remove from text </summary>
        private readonly string _invalidChars;

        /// <summary>List of segments to be excluded</summary>
        private readonly List<string> _excludedSegments;
        #endregion

        #region Private Constants
        /// <summary> 101 OR IDS_UI_TITLE in the rc files will become Entity in the resx files </summary>
        private const string Entity101 = "Entity";
        /// <summary> 102 in the rc files will become Version in the resx files </summary>
        private const string Version102 = "Version";
        /// <summary> 103 in the rc files will become Copyright in the resx files </summary>
        private const string Copyright103 = "Copyright";
        /// <summary> Resx extension </summary>
        private const string ResxExtension = ".resx";
        /// <summary> BEGIN line </summary>
        private const string LineBegin = "BEGIN";
        /// <summary> END line </summary>
        private const string LineEnd = "END";
        /// <summary> 101 line </summary>
        private const string Line101 = "101";
        /// <summary> IDS_UI_TITLE line </summary>
        private const string LineIdsUiTitle = "IDS_UI_TITLE";
        /// <summary> 102 line </summary>
        private const string Line102 = "102";
        /// <summary> 103 line </summary>
        private const string Line103 = "103";
        /// <summary> Back Slash character </summary>
        private const string BackSlash = @"\";
        /// <summary> Resx file comment </summary>
        private const string ResxComment = "Auto-Generated";
        /// <summary> Resx suffix </summary>
        private const string ResxSuffix = "Resx";
        #endregion

        #region Public Constants
        /// <summary> resourceInfoKey is used as a dictionary key for resourceInfo </summary>
        public const string ResourceInfoKey = "resourceInfo";
        /// <summary> settingsKey is used as a dictionary key for settings </summary>
        public const string SettingsKey = "settings";
        #endregion

        #region Public Delegates
        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void ProcessingEventHandler(string text);
        
        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <param name="language">Language</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        /// <param name="rowIndex">Row Index for UI sync</param>
        public delegate void StatusEventHandler(ResourceInfo resourceInfo, string language, ResourceInfo.StatusType statusType, 
            string text, int rowIndex);
        #endregion

        #region Public Events
        /// <summary> Event to update UI with name of file being processed </summary>
        public event ProcessingEventHandler ProcessingEvent;

        /// <summary> Event to update UI with status of file being processed </summary>
        public event StatusEventHandler StatusEvent;
        #endregion

        #region Constructor
        /// <summary> Constructor setting defaults </summary>
        public ProcessGeneration()
        {
            // Assign invalid characters and cache
            _invalidChars = GetInvalidChars();

            // Assign excluded segments and cache
            _excludedSegments = GetExcludedSegments();
        }
        #endregion

        #region Public Methods
        /// <summary> Validate the resource info </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <returns>True if valid otherwise false</returns>
        /// <remarks>Resource Info must have something to process</remarks>
        public bool ValidResourceInfo(BindingList<ResourceInfo> resourceInfo)
        {
            var retVal = false;

            foreach (var info in resourceInfo)
            {
                retVal = (info.SourceFile != null) && (info.SourcePath != null) && (info.TargetFile != null) &&
                         (info.TargetPath != null);
                if (!retVal)
                {
                    break;
                }
            }

            return retVal;
        }

        /// <summary> Validate the settings </summary>
        /// <param name="settings">Settings</param>
        /// <returns>True if valid otherwise false</returns>
        /// <remarks>At least one language must be selected</remarks>
        public bool ValidSettings(Settings settings)
        {
            return (settings.Languages.Count > 0);
        }

        /// <summary> Get resource info from file </summary>
        /// <param name="fileName">Resource Info file name</param>
        /// <returns>Resource Info</returns>
        public BindingList<ResourceInfo> GetResourceInfo(string fileName)
        {
            var retVal = new BindingList<ResourceInfo>();

            try
            {
                // Read resource info file
                var lines = File.ReadAllLines(@fileName);

                // Iterate and add to list
                foreach (var line in lines)
                {
                    var resourceInfo = new ResourceInfo();
                    var parsedLine = line.Split(';');

                    resourceInfo.SourcePath = parsedLine[0];
                    resourceInfo.SourceFile = parsedLine[1];
                    resourceInfo.TargetPath = parsedLine[2];
                    resourceInfo.TargetFile = parsedLine[3];

                    retVal.Add(resourceInfo);
                }
            }
            catch
            {
                throw new Exception(Properties.Resources.ErrorResourceFile);
            }

            return retVal;
        }

        /// <summary> Get settings from file </summary>
        /// <param name="fileName">Settings file name</param>
        /// <returns>Settings string</returns>
        public string GetSettings(string fileName)
        {
            var retVal = string.Empty;

            try
            {
                // Read settings file
                var lines = File.ReadAllLines(@fileName);

                // Iterate and add to list
                retVal = lines.Select(line => line.Split(';')).SelectMany(parsedLine => parsedLine).Aggregate(retVal, (current, segment) => current + segment + ";");
            }
            catch
            {
                throw new Exception(Properties.Resources.ErrorSettingsFile);
            }

            // Trim ending
            if (retVal.EndsWith(";"))
            {
                retVal = retVal.Substring(0, retVal.Length - 1);
            }

            return retVal;
        }

        /// <summary> Build settings for background worker </summary>
        /// <param name="settings">Settings string</param>
        /// <returns>Settings</returns>
        public Settings BuildSettings(string settings)
        {
            var parsedLine = settings.Split(';');
            var retVal = new Settings { Overwrite = Convert.ToBoolean(parsedLine[0]) };

            if (parsedLine.Length > 0)
            {
                for (var segment = 1; segment < parsedLine.Length; segment++)
                {
                    var key = parsedLine[segment];
                    var value = string.Empty;

                    switch (key)
                    {
                        case ResourceInfo.English:
                            value = Properties.Resources.EnglishCulture;
                            break;
                        case ResourceInfo.French:
                            value = Properties.Resources.FrenchCulture;
                            break;
                        case ResourceInfo.Spanish:
                            value = Properties.Resources.SpanishCulture;
                            break;
                        case ResourceInfo.ChineseSimplified:
                            value = Properties.Resources.ChineseSimplifiedCulture;
                            break;
                        case ResourceInfo.ChineseTraditional:
                            value = Properties.Resources.ChineseTraditionalCulture;
                            break;
                        default:
                            // Invalid value
                            key = string.Empty;
                            break;
                    }

                    // If key is not empty then add
                    if (!key.Equals(string.Empty))
                    {
                        retVal.Languages.Add(key, value);
                    }
                }
            }

            return retVal;
        }

        /// <summary> Start the generation process </summary>
        /// <param name="dictionary">Dictionary containing the resource information and settings</param>
        public void Process(Dictionary<string, object> dictionary)
        {
            // Unpack the dictionary
            UnPackDictionary(dictionary);
            
            // Iterate resource info
            IterateResources();
        }
        #endregion

        #region Private Methods
        /// <summary> Unpack the dictionary into local storages </summary>
        /// <param name="dictionary">Dictionary containing the resource information and settings</param>
        private void UnPackDictionary(IReadOnlyDictionary<string, object> dictionary)
        {
            _resourceInfo = (BindingList<ResourceInfo>)dictionary[ResourceInfoKey];
            _settings = (Settings) dictionary[SettingsKey];
        }

        /// <summary> Iterate the resource information and languages supplied </summary>
        private void IterateResources()
        {
            var rowIndex = -1;

            foreach (var resourceInfo in _resourceInfo)
            {
                // Increment row index for UI sync
                rowIndex++;

                // Iterate languages for resource
                foreach (var language in _settings.Languages)
                {
                    // Construct full source file name
                    var sourceName = GetSourceName(resourceInfo, language);

                    // Construct full target file name
                    bool targetExists;
                    var targetName = GetTargetName(resourceInfo, language, out targetExists);

                    // Update display of files being processed
                    if (ProcessingEvent != null)
                    {
                        ProcessingEvent(string.Format(Properties.Resources.FilesBeingProcessed, sourceName, targetName));
                    }

                    // If source file does not exist, notify UI and continue with processing next file
                    if (!File.Exists(sourceName))
                    {
                        if (StatusEvent != null)
                        {
                            StatusEvent(resourceInfo, language.Key, ResourceInfo.StatusType.Error,
                                string.Format(Properties.Resources.DoesNotExist, sourceName), rowIndex);
                        }
                        continue;
                    }

                    // If target file already exists AND is not to be overwritten, continue with processing next file
                    if (targetExists && !_settings.Overwrite)
                    {
                        if (StatusEvent != null)
                        {
                            StatusEvent(resourceInfo, language.Key, ResourceInfo.StatusType.Warning,
                                string.Format(Properties.Resources.AlreadyExists, targetName), rowIndex);
                        }
                        continue;
                    }

                    // Iterate source file and create target file
                    var success = CreateResxFile(sourceName, targetName, language);

                    // Update status
                    if (StatusEvent != null)
                    {
                        if (success)
                        {
                            StatusEvent(resourceInfo, language.Key, ResourceInfo.StatusType.Success, string.Empty, rowIndex);
                        }
                        else
                        {
                            StatusEvent(resourceInfo, language.Key, ResourceInfo.StatusType.Error, 
                                string.Format(Properties.Resources.ErrorCreatingFile, targetName), rowIndex);
                        }
                    }
                }

            }

        }

        /// <summary> Get source name of rc file with language information </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <param name="language">Languages being processed</param>
        /// <returns>Constructed source name</returns>
        private string GetSourceName(ResourceInfo resourceInfo, KeyValuePair<string, string> language)
        {
            return resourceInfo.SourcePath + BackSlash + language.Key + BackSlash + resourceInfo.SourceFile;
        }

        /// <summary> Get target name of resx file with language information </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <param name="language">Languages being processed</param>
        /// <param name="targetExists">True if target file exists otherwise false</param>
        /// <returns>Constructed target name</returns>
        /// <remarks>targetExists is a returned parameter </remarks>
        private string GetTargetName(ResourceInfo resourceInfo, KeyValuePair<string, string> language, out bool targetExists)
        {
            // Create target folder if required
            if (!Directory.Exists(resourceInfo.TargetPath + BackSlash + language.Key))
            {
                Directory.CreateDirectory(resourceInfo.TargetPath + BackSlash + language.Key);
            }

            // Construct target name and validate that if it exists, it can be overwritten
            var retVal = resourceInfo.TargetPath + BackSlash + language.Key + BackSlash + resourceInfo.TargetFile + 
                (resourceInfo.TargetFile.EndsWith(@ResxSuffix) ? "" :  @ResxSuffix) + 
                ((language.Value.Equals(string.Empty)) ? "" : @"." + language.Value) + @ResxExtension;
            targetExists = File.Exists(retVal);

            //  If it exists does settings say this can be overwritten?
            if (targetExists && _settings.Overwrite)
            {
                // Delete file so that it can be rewritten
                File.Delete(retVal);
            }

            return retVal;
        }

        /// <summary> Iterates source rc file and creates target resx file </summary>
        /// <param name="sourceName">Source name of rc file</param>
        /// <param name="targetName">Target name of resx file</param>
        /// <param name="language">Languages being processed</param>
        /// <returns>True if successful otherwise false</returns>
        private bool CreateResxFile(string sourceName, string targetName, KeyValuePair<string, string> language)
        {
            bool retVal;

            try
            {
                var evaluateLine = false;
                var ids = new Dictionary<string, string>();

                // Read source file
                var lines = File.ReadAllLines(@sourceName, GetEncoding(language.Key));

                // Create resx file
                var resx = new ResXResourceWriter(@targetName);

                // Iterate 
                foreach (var newLine in lines.Select(line => line.Replace("\\t", " ").Trim()))
                {
                    // Only evaluate lines between the BEGIN and END
                    if (newLine.ToUpper().Equals(LineBegin))
                    {
                        // BEGIN was found. Can begin evaluating
                        evaluateLine = true;
                        continue;
                    }
                    
                    if (newLine.ToUpper().Equals(LineEnd))
                    {
                        // END was found. Can stop evaluating
                        evaluateLine = false;
                        continue;
                    }

                    // Is evaluation active and is there something to evaluate?
                    if (!evaluateLine || newLine.Equals(string.Empty) || newLine.StartsWith(@"//") || newLine.StartsWith(@"/*"))
                    {
                        continue;
                    }

                    // Split line based upon quotation mark
                    var splitLine = newLine.Split('\"');

                    // Certain cleanups will be required
                    var text = ReplaceInvalidChars(splitLine[1].Trim());

                    // id is the value that will be used as the key to the new resx file
                    var id = GetUniqueId(CleanupId(splitLine[0].Trim()), ids, text);

                    // Do not add if the id is empty
                    if (id.Equals(string.Empty))
                    {
                        continue;
                    }

                    // Add to resx file and list of ids for the resource file
                    var node = new ResXDataNode(id, text) {Comment = ResxComment};

                    resx.AddResource(node);
                    ids.Add(id.ToUpper(), text);
                }

                // Close resx file
                resx.Close();
                retVal = true;
            }
            catch
            {
                // Need to delete file if error ocurred as file will be incomplete
                File.Delete(@targetName);
                retVal = false;
            }

            return retVal;
        }

        /// <summary> Perform various cleanup scenarios on id field </summary>
        /// <param name="id">Id being cleaned</param>
        /// <returns>Cleaned id field</returns>
        /// <remarks>Removes invalid chars and excluded segments, reformats rc id into new format</remarks>
        private string CleanupId(string id)
        {
            var retVal = id;
            var temp = string.Empty;

            // Cleanup trailing comma and/or underscore
            if (retVal.EndsWith(",") || retVal.EndsWith("_"))
            {
                retVal = retVal.Substring(0, retVal.Length - 1).Trim();
            }

            // Split it and put it back together
            var segments = retVal.Split('_');

            for (var segment = 0; segment < segments.Length; segment++)
            {
                // Skip segment if it is to be excluded
                if (_excludedSegments.Contains(segments[segment].ToUpper()))
                {
                    continue;
                }

                // Remove 'HotKey' character from last segment, if present
                if (segment.Equals(segments.Length - 1) && segments[segment].Length > 0)
                {
                    var chars = segments[segment].Substring(0, 1).ToCharArray();
                    var ascVal = Convert.ToInt32(chars[0]);
                    if (ascVal >= 97 && ascVal <= 122)
                    {
                        // First character is lower case. Now, second character must not be lower case for
                        // it to be a hotkey
                        chars = segments[segment].Substring(1, 1).ToCharArray();
                        ascVal = Convert.ToInt32(chars[0]);
                        if (!(ascVal >= 97 && ascVal <= 122))
                        {
                            // Second character was not lowercase, thus it is a hotkey character
                            segments[segment] = segments[segment].Substring(1);
                        }
                    }
                }

                // Upper case 1st character. Lower case the rest, but not if last segment
                temp += segments[segment].Substring(0, 1).ToUpper() +
                        (segment.Equals(segments.Length - 1)
                            ? segments[segment].Substring(1)
                            : segments[segment].Substring(1).ToLower());
            }

            // Special case for strings 101, IDS_UI_TITLE, 102 and 103 located in the .rc files
            if (temp.Equals(Line101) || retVal.Trim().ToUpper().Equals(LineIdsUiTitle))
            {
                // Description of entity (i.e. Accounts Payable, A/P Vendors, ...)
                temp = Entity101;
            }
            else if (temp.Equals(Line102))
            {
                // Description of product version
                temp = Version102;
            }
            else if (temp.Equals(Line103))
            {
                // Copyright information
                temp = Copyright103;
            }

            // Assign modified string
            retVal = temp.Trim();

            return retVal;
        }

        /// <summary> Replaces invalid characters in text </summary>
        /// <param name="text">Text being cleaned</param>
        /// <returns>Cleaned text</returns>
        private string ReplaceInvalidChars(string text)
        {
            var retVal = text.Trim();

            // Replace invalid chars with ""
            for (var i = 0; i < _invalidChars.Length; i++)
            {
                retVal = retVal.Replace(_invalidChars.Substring(i, 1), "");
            }

            //// If there is a backslash character
            //if (retVal.Contains(BackSlash))
            //{
            //    // Special escape sequence replacement
            //    retVal = retVal.Replace("\\n", "&#xA");
            //    retVal = retVal.Replace("\\r", "&#xD");
            //    retVal = retVal.Replace("\\t", "&#x9");

            //    // Fail safe in case any "\" are remaining
            //    retVal = retVal.Replace(BackSlash, "");
            //}

            return retVal;
        }

        /// <summary> Gets invalid characters for replacement logic </summary>
        /// <returns>Invalid characters</returns>
        private string GetInvalidChars()
        {
            // Ampersand and quote
            var retVal = "&" + "\"";

            // Non-keyboard characters
            for (var i = 130; i < 183; i++)
            {
                retVal += (char) i;
            }

            return retVal;
        }

        /// <summary> Gets excluded segments to be removed if found in string </summary>
        /// <returns>Excluded segments</returns>
        private List<string> GetExcludedSegments()
        {
            // Return excluded segments
            return new List<string> {"IDS", "CAP", "MSG", "IE", "ERR", "WARN", "BUT", "TTL", "TEXT", 
                "TIP", "TAB", "SEC", "MENU", "PROP", "BUTTON", "CAPTION", "ACL"};
        }

        /// <summary> Get unique id incase the id already exists </summary>
        /// <param name="id">Id for text</param>
        /// <param name="ids">Dictionary of ids</param>
        /// <param name="text">Text</param>
        /// <returns>Unique id</returns>
        private string GetUniqueId(string id, Dictionary<string, string> ids, string text )
        {
            // Assign entered id as default
            var retVal = id;

            // Is entered id unique as is?
            if (!ids.ContainsKey(retVal.ToUpper()))
            {
                return retVal;
            }

            // Assign a previous value for iterative comparison
            var prevText = ids[retVal.ToUpper()];

            // Need to ensure id is unique
            var counter = 0;

            while (true)
            {
                // Increment counter to append to entered id
                retVal = id + Convert.ToString(++counter);

                // Does new id not exist and is therefore unqiue?
                if (!ids.ContainsKey(retVal.ToUpper()))
                {
                    // Before exitting, if the content is the same, do not add it again
                    if (text.Equals(prevText))
                    {
                        retVal = string.Empty;
                    }

                    break;
                }

                // Assign a previous value for iterative comparison
                prevText = ids[retVal.ToUpper()];

            }

            return retVal;
        }

        /// <summary> Get encoding per language </summary>
        /// <param name="language">Language</param>
        /// <returns>Encoding</returns>
        private Encoding GetEncoding(string language)
        {
            var retVal = Encoding.UTF8;

            switch (language)
            {
                case ResourceInfo.English:
                    retVal = Encoding.GetEncoding(1252); //Encoding.UTF8;
                    break;
                case ResourceInfo.French:
                    retVal = Encoding.GetEncoding(1252);
                    break;
                case ResourceInfo.Spanish:
                    retVal = Encoding.GetEncoding(1252);
                    break;
                case ResourceInfo.ChineseSimplified:
                    retVal = Encoding.GetEncoding(936);
                    break;
                case ResourceInfo.ChineseTraditional:
                    retVal = Encoding.GetEncoding(950);
                    break;
            }

            return retVal;
        }

        #endregion
    }
}
