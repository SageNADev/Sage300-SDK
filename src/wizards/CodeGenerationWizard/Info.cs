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

using System.Drawing;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> Information class to hold info for classes to be generated </summary>
    class Info
    {
        #region Private Vars
        /// <summary> Status Type for status </summary>
        private StatusTypeEnum _statusType = StatusTypeEnum.None;
        /// <summary> Icon of the status </summary>
        private Icon _status = Properties.Resources.Blank;
        #endregion

        #region Public Enums
        /// <summary> Status type used to identify icon required for UI </summary>
        public enum StatusTypeEnum
        {
            None,
            Success,
            Error
        }
        #endregion

        #region Public Constants
        /// <summary> Column in UI for File Name </summary>
        public const int FileNameColumnNo = 0;
        /// <summary> Column name in UI for File Name </summary>
        public const string FileNameColumnName = "FileName";
        /// <summary> Column in UI for Status </summary>
        public const int StatusColumnNo = 1;
        /// <summary> Column name in UI for File Name </summary>
        public const string StatusColumnName = "Status";
        #endregion

        #region Public Properties
        /// <summary> Name for UI display of File Name </summary>
        public string FileName { get; set; }

        /// <summary> Icon for UI display of status </summary>
        public Icon Status
        {
            get { return _status; }
        }
        #endregion

        #region Public Methods
        /// <summary> Sets status to appropriate status column based upon status type </summary>
        /// <param name="statusType">Status Type</param>
        /// <returns>Column index being set</returns>
        public int SetStatus(StatusTypeEnum statusType)
        {
            _status = GetIcon(statusType); 
            _statusType = statusType;

            return StatusColumnNo;
        }

        /// <summary> Gets status type </summary>
        /// <returns>Status Type</returns>
        public StatusTypeEnum GetStatusType()
        {
            return _statusType;
        }

        #endregion

        #region Private Methods
        /// <summary> Get icon based upon status type </summary>
        /// <param name="statusType">Status Type</param>
        /// <returns>Icon</returns>
        private Icon GetIcon(StatusTypeEnum statusType)
        {
            var retVal = Properties.Resources.Blank;

            switch (statusType)
            {
                case StatusTypeEnum.None:
                    retVal = Properties.Resources.Blank;
                    break;
                case StatusTypeEnum.Success:
                    retVal = Properties.Resources.Success;
                    break;
                case StatusTypeEnum.Error:
                    retVal = Properties.Resources.Error;
                    break;
            }

            return retVal;
        }
        #endregion
    }
}
