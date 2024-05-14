// The MIT License (MIT) 
// Copyright (c) 1994-2024 The Sage Group plc or its licensors.  All rights reserved.
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

using System.ComponentModel;

namespace Sage.CA.SBS.ERP.Sage300.SyncAssembliesWizard
{
    /// <summary> Settings class to hold info UI Settings </summary>
    [System.SerializableAttribute]
    public class Settings
    {
        #region Constructor
        #endregion

        #region Public Properties

        /// <summary> Source Type </summary>
        public SourceType SourceType { get; set; }
        /// <summary> Include PDB Files </summary>
        public bool IncludePdbFiles { get; set; }
        /// <summary> Destination </summary>
        public string Destination { get; set; }
        /// <summary> Assemblies </summary>
        public BindingList<AssemblyInfo> Assemblies { get; set; }
        /// <summary> Initial Sync </summary>
        public bool InitialSync { get; set; }
        /// <summary> Destination Web</summary>
        public string DestinationWeb{ get; set; }
        #endregion
    }

}
