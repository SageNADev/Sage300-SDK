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

namespace Sage.CA.SBS.ERP.Sage300.SubclassPrep
{
    /// <summary>
    /// Enum for Module Types
    /// </summary>
    public enum ModuleType
    {
        /// <summary> Common Services </summary>
        CS = 0,

        /// <summary> Accounts Payable </summary>
        AP = 1,

        /// <summary> Accounts Receivable </summary>
        AR = 2,

        /// <summary> Administrative Services </summary>
        AS = 3,

        /// <summary> General Ledger </summary>
        GL = 4,

        /// <summary> Inventory Control </summary>
        IC = 5,

        /// <summary> Project and Job Costing </summary>
        PM = 6,

        /// <summary> Order Entry </summary>
        OE = 7,

        /// <summary> Key Performance Indictators </summary>
        KPI = 8,

        /// <summary> Multiple Contacts </summary>
        MT = 9,

        /// <summary> Notes </summary>
        KN = 10,

        /// <summary> Purchase Order </summary>
        PO = 11,

        /// <summary> Visual Process FLow </summary>
        VPF = 12,

        /// <summary> Payroll </summary>
        PR = 13,

        /// <summary> Bank Services </summary>
        BK = 14,

        /// <summary> Tax Services </summary>
        TX = 15,

        /// <summary> Tax - Malaysia </summary>
        TM = 16,

        /// <summary> Tax - Singapore </summary>
        TS = 17,

        /// <summary> Tax - ? </summary>
        TW =18

        /// <summary> Tax - United Kingdom </summary>
        //TK = 19
    }
}
