
// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

#region Namespace

#endregion

namespace ValuedPartner.TU.Models
{
    /// <summary>
    /// Contains list of TaxAuthorities Constants
    /// </summary>
    public partial class TaxAuthorities
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public const string EntityName = "TX0002";


        #region Properties

        /// <summary>
        /// Contains list of TaxAuthorities Field Constants
        /// </summary>
        public class Fields
        {
            /// <summary>
            /// Property for TaxAuthority
            /// </summary>
            public const string TaxAuthority = "AUTHORITY";

            /// <summary>
            /// Property for Description
            /// </summary>
            public const string Description = "DESC";

            /// <summary>
            /// Property for TaxReportingCurrency
            /// </summary>
            public const string TaxReportingCurrency = "SCURN";

            /// <summary>
            /// Property for MaximumTaxAllowable
            /// </summary>
            public const string MaximumTaxAllowable = "MAXTAX";

            /// <summary>
            /// Property for NoTaxChargedBelow
            /// </summary>
            public const string NoTaxChargedBelow = "MINTAX";

            /// <summary>
            /// Property for TaxBase
            /// </summary>
            public const string TaxBase = "TXBASE";

            /// <summary>
            /// Property for AllowTaxInPrice
            /// </summary>
            public const string AllowTaxInPrice = "INCLUDABLE";

            /// <summary>
            /// Property for TaxLiabilityAccount
            /// </summary>
            public const string TaxLiabilityAccount = "LIABILITY";

            /// <summary>
            /// Property for ReportLevel
            /// </summary>
            public const string ReportLevel = "AUDITLEVEL";

            /// <summary>
            /// Property for TaxRecoverable
            /// </summary>
            public const string TaxRecoverable = "RECOVERABL";

            /// <summary>
            /// Property for RecoverableRate
            /// </summary>
            public const string RecoverableRate = "RATERECOV";

            /// <summary>
            /// Property for RecoverableTaxAccount
            /// </summary>
            public const string RecoverableTaxAccount = "ACCTRECOV";

            /// <summary>
            /// Property for ExpenseSeparately
            /// </summary>
            public const string ExpenseSeparately = "EXPSEPARTE";

            /// <summary>
            /// Property for ExpenseAccount
            /// </summary>
            public const string ExpenseAccount = "ACCTEXP";

            /// <summary>
            /// Property for LastMaintained
            /// </summary>
            public const string LastMaintained = "LASTMAINT";

            /// <summary>
            /// Property for TaxType
            /// </summary>
            public const string TaxType = "TAXTYPE";

            /// <summary>
            /// Property for ReportTaxonRetainageDocument
            /// </summary>
            public const string ReportTaxonRetainageDocument = "TXRTGCTL";

        }

        #endregion
        #region Properties

        /// <summary>
        /// Contains list of TaxAuthorities Index Constants
        /// </summary>
        public class Index
        {
            /// <summary>
            /// Property Indexer for TaxAuthority
            /// </summary>
            public const int TaxAuthority = 1;

            /// <summary>
            /// Property Indexer for Description
            /// </summary>
            public const int Description = 2;

            /// <summary>
            /// Property Indexer for TaxReportingCurrency
            /// </summary>
            public const int TaxReportingCurrency = 3;

            /// <summary>
            /// Property Indexer for MaximumTaxAllowable
            /// </summary>
            public const int MaximumTaxAllowable = 4;

            /// <summary>
            /// Property Indexer for NoTaxChargedBelow
            /// </summary>
            public const int NoTaxChargedBelow = 5;

            /// <summary>
            /// Property Indexer for TaxBase
            /// </summary>
            public const int TaxBase = 6;

            /// <summary>
            /// Property Indexer for AllowTaxInPrice
            /// </summary>
            public const int AllowTaxInPrice = 7;

            /// <summary>
            /// Property Indexer for TaxLiabilityAccount
            /// </summary>
            public const int TaxLiabilityAccount = 9;

            /// <summary>
            /// Property Indexer for ReportLevel
            /// </summary>
            public const int ReportLevel = 13;

            /// <summary>
            /// Property Indexer for TaxRecoverable
            /// </summary>
            public const int TaxRecoverable = 14;

            /// <summary>
            /// Property Indexer for RecoverableRate
            /// </summary>
            public const int RecoverableRate = 15;

            /// <summary>
            /// Property Indexer for RecoverableTaxAccount
            /// </summary>
            public const int RecoverableTaxAccount = 16;

            /// <summary>
            /// Property Indexer for ExpenseSeparately
            /// </summary>
            public const int ExpenseSeparately = 17;

            /// <summary>
            /// Property Indexer for ExpenseAccount
            /// </summary>
            public const int ExpenseAccount = 18;

            /// <summary>
            /// Property Indexer for LastMaintained
            /// </summary>
            public const int LastMaintained = 19;

            /// <summary>
            /// Property Indexer for TaxType
            /// </summary>
            public const int TaxType = 20;

            /// <summary>
            /// Property Indexer for ReportTaxonRetainageDocument
            /// </summary>
            public const int ReportTaxonRetainageDocument = 21;


        }

        #endregion

    }
}