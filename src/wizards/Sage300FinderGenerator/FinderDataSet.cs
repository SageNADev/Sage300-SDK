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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sage300FinderGenerator
{
    internal class FinderDataSet : INotifyPropertyChanged
    {
        #region Class constant
        public static string ViewID_FieldName = "viewID";
        public static string ViewOrder_FieldName = "viewOrder";
        public static string ParentValAsInitKey_FieldName = "parentValAsInitKey";
        public static string ReturnFieldNames_FieldName = "returnFieldNames";
        public static string DisplayFieldNames_FieldName = "displayFieldNames";

        public static char ModuleNameSeparator = '.';
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region private members
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IDictionary<string, object> finderDetailLookup;
        private string finderName;
        private string finderModule;

        private bool isViewIDUpdate;

        #endregion

        public bool IsViewIDUpdate
        {
            get => isViewIDUpdate;
            set { isViewIDUpdate = value; }
        }

        public IDictionary<string, object> FinderDetailLookup => finderDetailLookup;
        
        public string ViewID
        {
            get
            {
                return (string)finderDetailLookup?[ViewID_FieldName];
            }
            set
            {
                if (value != (string)finderDetailLookup[ViewID_FieldName])
                {
                    finderDetailLookup[ViewID_FieldName] = value;
                    isViewIDUpdate = true;
                    NotifyPropertyChanged();
                }
            }
        }

        public int ViewOrder
        {
            get
            {
                return (int)(finderDetailLookup?[ViewOrder_FieldName] ?? -1);
            }
            set
            {
                if (value != (int)finderDetailLookup[ViewOrder_FieldName])
                {
                    finderDetailLookup[ViewOrder_FieldName] = value;
                    NotifyPropertyChanged();
                }
            }

        }

        public string FinderName
        {
            get
            {
                return finderName;
            }
            set
            {
                if (value != finderName)
                {
                    finderName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string FinderModule
        {
            get
            {
                return finderModule;
            }
            set
            {
                if (value != finderModule)
                {
                    finderModule = value;
                    NotifyPropertyChanged();
                }
            }
        }

        internal void SetDataValue(IDictionary<string, object> finderInfoLookup, string finderName, string finderModule)
        {
            finderDetailLookup = finderInfoLookup;
            this.finderName = finderName;
            this.finderModule = finderModule;
            isViewIDUpdate = false;

            NotifyPropertyChanged(nameof(ViewID));
        }
    }
}
