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

using System.Drawing;

namespace Sage.CA.SBS.ERP.Sage300.ResxGeneration
{
    /// <summary> Resource Information class to hold info on rc files to be generated </summary>
    class ResourceInfo
    {
        #region Private Vars
        /// <summary> Status Type of the English status </summary>
        private StatusType _engStatusType = StatusType.None;
        /// <summary> Icon of the English status </summary>
        private Icon _engStatus = Properties.Resources.Blank;
        /// <summary> Status Type of the French status </summary>
        private StatusType _fraStatusType = StatusType.None;
        /// <summary> Icon of the French status </summary>
        private Icon _fraStatus = Properties.Resources.Blank;
        /// <summary> Status Type of the Spanish status </summary>
        private StatusType _esnStatusType = StatusType.None;
        /// <summary> Icon of the Spanish status </summary>
        private Icon _esnStatus = Properties.Resources.Blank;
        /// <summary> Status Type of the Chinese Simplified status </summary>
        private StatusType _chnStatusType = StatusType.None;
        /// <summary> Icon of the Chinese Simplified status </summary>
        private Icon _chnStatus = Properties.Resources.Blank;
        /// <summary> Status Type of the Chinese Traditional status </summary>
        private StatusType _chtStatusType = StatusType.None;
        /// <summary> Icon of the Chinese Traditional status </summary>
        private Icon _chtStatus = Properties.Resources.Blank;

        #endregion

        #region Public Enums
        /// <summary> Status type used to identify icon required for UI </summary>
        public enum StatusType
        {
            None,
            Success,
            Warning,
            Error
        }
        #endregion

        #region Public Constants
        /// <summary> Column in UI for Source Path </summary>
        public const int SourcePathColumn = 0;
        /// <summary> Column name in UI for Source Path </summary>
        public const string SourcePathName = "SourcePath";
        /// <summary> Column in UI for Source File </summary>
        public const int SourceFileColumn = 1;
        /// <summary> Column name in UI for Source File </summary>
        public const string SourceFileName = "SourceFile";
        /// <summary> Column in UI for Target Path </summary>
        public const int TargetPathColumn = 2;
        /// <summary> Column name in UI for Target Path </summary>
        public const string TargetPathName = "TargetPath";
        /// <summary> Column in UI for Target File </summary>
        public const int TargetFileColumn = 3;
        /// <summary> Column name in UI for Target File </summary>
        public const string TargetFileName = "TargetFile";
        /// <summary> Column in UI for English Status </summary>
        public const int EngStatusColumn = 4;
        /// <summary> Column in UI for French Status </summary>
        public const int FraStatusColumn = 5;
        /// <summary> Column in UI for Spanish Status </summary>
        public const int EsnStatusColumn = 6;
        /// <summary> Column in UI for Chinese Simplified Status </summary>
        public const int ChnStatusColumn = 7;
        /// <summary> Column in UI for Chinese Traditional Status </summary>
        public const int ChtStatusColumn = 8;
        /// <summary> English code </summary>
        public const string English = "ENG";
        /// <summary> French code </summary>
        public const string French = "FRA";
        /// <summary> Spanish code </summary>
        public const string Spanish = "ESN";
        /// <summary> Chinese Simplified code </summary>
        public const string ChineseSimplified = "CHN";
        /// <summary> Chinese Traditional code </summary>
        public const string ChineseTraditional = "CHT";
        #endregion

        #region Public Properties
        /// <summary> Source Path identifies where the rc file is located </summary>
        public string SourcePath { get; set; }
        /// <summary> Source File is the name of the rc file </summary>
        public string SourceFile { get; set; }
        /// <summary> Target Path identifies where the resx file will be created (minus language references) </summary>
        public string TargetPath { get; set; }
        /// <summary> Target File is the name of the resx file (minus language references) </summary>
        public string TargetFile { get; set; }
        /// <summary> Icon for UI display of the English status </summary>
        public Icon EngStatus
        {
            get { return _engStatus; }
        }
        /// <summary> Icon for UI display of the French status </summary>
        public Icon FraStatus
        {
            get { return _fraStatus; }
        }
        /// <summary> Icon for UI display of the Spanish status </summary>
        public Icon EsnStatus
        {
            get { return _esnStatus; }
        }
        /// <summary> Icon for UI display of the Chinese Simplified status </summary>
        public Icon ChnStatus
        {
            get { return _chnStatus; }
        }
        /// <summary> Icon for UI display of the Chinese Traditional status </summary>
        public Icon ChtStatus
        {
            get { return _chtStatus; }
        }
        #endregion

        #region Public Methods
        /// <summary> Sets status to appropriate status column based upon language and status type </summary>
        /// <param name="language">Language being set</param>
        /// <param name="statusType">Status Type</param>
        /// <returns>Column index of language being set</returns>
        public int SetStatus(string language, StatusType statusType)
        {
            var retVal = 0;
            var icon = GetIcon(statusType);

            switch (language)
            {
                case English:
                    _engStatus = icon;
                    _engStatusType = statusType;
                    retVal = EngStatusColumn;
                    break;
                case French:
                    _fraStatus = icon;
                    _fraStatusType = statusType;
                    retVal = FraStatusColumn;
                    break;
                case Spanish:
                    _esnStatus = icon;
                    _esnStatusType = statusType;
                    retVal = EsnStatusColumn;
                    break;
                case ChineseSimplified:
                    _chnStatus = icon;
                    _chnStatusType = statusType;
                    retVal = ChnStatusColumn;
                    break;
                case ChineseTraditional:
                    _chtStatus = icon;
                    _chtStatusType = statusType;
                    retVal = ChtStatusColumn;
                    break;
            }

            return retVal;
        }

        /// <summary> Gets status type for the requested language </summary>
        /// <param name="language">Language being set</param>
        /// <returns>Status Type</returns>
        public StatusType GetStatusType(string language)
        {
            var retVal = StatusType.None;

            switch (language)
            {
                case English:
                    retVal = _engStatusType;
                    break;
                case French:
                    retVal = _fraStatusType;
                    break;
                case Spanish:
                    retVal = _esnStatusType;
                    break;
                case ChineseSimplified:
                    retVal = _chnStatusType;
                    break;
                case ChineseTraditional:
                    retVal = _chtStatusType;
                    break;
            }

            return retVal;
        }

        #endregion

        #region Private Methods
        /// <summary> Get icon based upon status type </summary>
        /// <param name="statusType">Status Type</param>
        /// <returns>Icon</returns>
        private Icon GetIcon(StatusType statusType)
        {
            var retVal = Properties.Resources.Blank;

            switch (statusType)
            {
                case StatusType.None:
                    retVal = Properties.Resources.Blank;
                    break;
                case StatusType.Success:
                    retVal = Properties.Resources.Success;
                    break;
                case StatusType.Warning:
                    retVal = Properties.Resources.Warning;
                    break;
                case StatusType.Error:
                    retVal = Properties.Resources.Error;
                    break;
            }

            return retVal;
        }
        #endregion
    }
}
