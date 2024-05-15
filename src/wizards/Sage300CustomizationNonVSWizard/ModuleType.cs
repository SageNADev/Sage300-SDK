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

namespace Sage.CA.SBS.ERP.Sage300.CustomizationNonVSWizard
{
    /// <summary>
    /// Enum for Module Types
    /// </summary>
    public enum ModuleType
    {
        /// <summary> Accounts Payable </summary>
        AP = 0,

        /// <summary> Accounts Receivable </summary>
        AR = 1,

        /// <summary> Administrative Services </summary>
        AS = 2,

        /// <summary> Bank Services </summary>
        BK = 3,

        /// <summary> Tax Services </summary>
        TX = 4,

        /// <summary> Common Services </summary>
        CS = 5,

        /// <summary> General Ledger </summary>
        GL = 6,

        /// <summary> Inventory Control </summary>
        IC = 7,

        /// <summary> Order Entry </summary>
        OE = 8,

        /// <summary> Purchase Order </summary>
        PO = 9,

        /// <summary> Tax - Australia </summary>
        TA = 10,

        /// <summary> Tax - Singapore </summary>
        TS = 11,

        /// <summary> Tax - United Kingdom </summary>
        //TK = 12,

        /// <summary> PJC - Project and Job Costing </summary>
        //PM = 13,
    }
}
