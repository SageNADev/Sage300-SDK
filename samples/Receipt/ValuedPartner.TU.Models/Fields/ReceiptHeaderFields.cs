// The MIT License (MIT) 
// Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved.
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

using Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes;
using System.Collections.Generic;
namespace ValuedParter.TU.Models
{
    /// <summary>
    /// Contains list of ReceiptHeader Constants
    /// </summary>
    public partial class ReceiptHeader
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public const string EntityName = "IC0590";

        /// <summary>
        /// DynamicAttributes
        /// </summary>
        [IgnoreExportImport]
        public static Dictionary<string, string> DynamicAttributes
        {
            get { return new Dictionary<string, string> { { "ADDCOST", "AdditionalCost" }}; }
        }

        #region Properties

        /// <summary>
        /// Contains list of ReceiptHeader Field Constants
        /// </summary>
        public class Fields
        {
            /// <summary>
            /// Property for SequenceNumber
            /// </summary>
            public const string SequenceNumber = "SEQUENCENO";

            /// <summary>
            /// Property for Description
            /// </summary>
            public const string Description = "RECPDESC";

            /// <summary>
            /// Property for ReceiptDate
            /// </summary>
            public const string ReceiptDate = "RECPDATE";

            /// <summary>
            /// Property for FiscalYear
            /// </summary>
            public const string FiscalYear = "FISCYEAR";

            /// <summary>
            /// Property for FiscalPeriod
            /// </summary>
            public const string FiscalPeriod = "FISCPERIOD";

            /// <summary>
            /// Property for PurchaseOrderNumber
            /// </summary>
            public const string PurchaseOrderNumber = "PONUM";

            /// <summary>
            /// Property for Reference
            /// </summary>
            public const string Reference = "REFERENCE";

            /// <summary>
            /// Property for ReceiptType
            /// </summary>
            public const string ReceiptType = "RECPTYPE";

            /// <summary>
            /// Property for RateOperation
            /// </summary>
            public const string RateOperation = "RATEOP";

            /// <summary>
            /// Property for VendorNumber
            /// </summary>
            public const string VendorNumber = "VENDNUMBER";

            /// <summary>
            /// Property for ReceiptCurrency
            /// </summary>
            public const string ReceiptCurrency = "RECPCUR";

            /// <summary>
            /// Property for ExchangeRate
            /// </summary>
            public const string ExchangeRate = "RECPRATE";

            /// <summary>
            /// Property for RateType
            /// </summary>
            public const string RateType = "RATETYPE";

            /// <summary>
            /// Property for RateDate
            /// </summary>
            public const string RateDate = "RATEDATE";

            /// <summary>
            /// Property for RateOverride
            /// </summary>
            public const string RateOverride = "RATEOVRRD";

            /// <summary>
            /// Property for AdditionalCost
            /// </summary>
            public const string AdditionalCost = "ADDCOST";

            /// <summary>
            /// Property for OrigAdditionalCostFunc
            /// </summary>
            public const string OrigAdditionalCostFunc = "ADDCOSTHM";

            /// <summary>
            /// Property for OrigAdditionalCostSource
            /// </summary>
            public const string OrigAdditionalCostSource = "ADDCOSTSRC";

            /// <summary>
            /// Property for AdditionalCostCurrency
            /// </summary>
            public const string AdditionalCostCurrency = "ADDCUR";

            /// <summary>
            /// Property for TotalExtendedCostFunctional
            /// </summary>
            public const string TotalExtendedCostFunctional = "TOTCSTHM";

            /// <summary>
            /// Property for TotalExtendedCostSource
            /// </summary>
            public const string TotalExtendedCostSource = "TOTCSTSRC";

            /// <summary>
            /// Property for TotalExtendedCostAdjusted
            /// </summary>
            public const string TotalExtendedCostAdjusted = "TOTCSTADJ";

            /// <summary>
            /// Property for TotalAdjustedCostFunctional
            /// </summary>
            public const string TotalAdjustedCostFunctional = "TOTADJHM";

            /// <summary>
            /// Property for TotalReturnCost
            /// </summary>
            public const string TotalReturnCost = "TOTCSTRET";

            /// <summary>
            /// Property for NumberOfDetailswithCost
            /// </summary>
            public const string NumberOfDetailswithCost = "NUMCSTDETL";

            /// <summary>
            /// Property for RequireLabels
            /// </summary>
            public const string RequireLabels = "LABELS";

            /// <summary>
            /// Property for AdditionalCostAllocationType
            /// </summary>
            public const string AdditionalCostAllocationType = "ADDCSTTYPE";

            /// <summary>
            /// Property for Complete
            /// </summary>
            public const string Complete = "COMPLETE";

            /// <summary>
            /// Property for OriginalTotalCostSource
            /// </summary>
            public const string OriginalTotalCostSource = "ORIGTOTSRC";

            /// <summary>
            /// Property for OriginalTotalCostFunctional
            /// </summary>
            public const string OriginalTotalCostFunctional = "ORIGTOTHM";

            /// <summary>
            /// Property for AdditionalCostFunctional
            /// </summary>
            public const string AdditionalCostFunctional = "ADDCSTHOME";

            /// <summary>
            /// Property for TotalCostReceiptAdditional
            /// </summary>
            public const string TotalCostReceiptAdditional = "TOTALCOST";

            /// <summary>
            /// Property for TotalAdjCostReceiptAddl
            /// </summary>
            public const string TotalAdjCostReceiptAddl = "TOTCOSTADJ";

            /// <summary>
            /// Property for ReceiptCurrencyDecimals
            /// </summary>
            public const string ReceiptCurrencyDecimals = "RECPDECIML";

            /// <summary>
            /// Property for VendorShortName
            /// </summary>
            public const string VendorShortName = "VENDNAME";

            /// <summary>
            /// Property for ICUniqueDocumentNumber
            /// </summary>
            public const string ICUniqueDocumentNumber = "DOCUNIQ";

            /// <summary>
            /// Property for VendorExists
            /// </summary>
            public const string VendorExists = "VENDEXISTS";

            /// <summary>
            /// Property for RecordDeleted
            /// </summary>
            public const string RecordDeleted = "DELETED";

            /// <summary>
            /// Property for TransactionNumber
            /// </summary>
            public const string TransactionNumber = "TRANSNUM";

            /// <summary>
            /// Property for RecordStatus
            /// </summary>
            public const string RecordStatus = "STATUS";

            /// <summary>
            /// Property for ReceiptNumber
            /// </summary>
            public const string ReceiptNumber = "RECPNUMBER";

            /// <summary>
            /// Property for NextDetailLineNumber
            /// </summary>
            public const string NextDetailLineNumber = "NEXTDTLNUM";

            /// <summary>
            /// Property for RecordPrinted
            /// </summary>
            public const string RecordPrinted = "PRINTED";

            /// <summary>
            /// Property for PostSequenceNumber
            /// </summary>
            public const string PostSequenceNumber = "POSTSEQNUM";

            /// <summary>
            /// Property for OptionalFields
            /// </summary>
            public const string OptionalFields = "VALUES";

            /// <summary>
            /// Property for ProcessCommand
            /// </summary>
            public const string ProcessCommand = "PROCESSCMD";

            /// <summary>
            /// Property for VendorName
            /// </summary>
            public const string VendorName = "VDLONGNAME";

            /// <summary>
            /// Property for EnteredBy
            /// </summary>
            public const string EnteredBy = "ENTEREDBY";

            /// <summary>
            /// Property for PostingDate
            /// </summary>
            public const string PostingDate = "DATEBUS";

        }

        #endregion
        #region Properties

        /// <summary>
        /// Contains list of ReceiptHeader Index Constants
        /// </summary>
        public class Index
        {
            /// <summary>
            /// Property Indexer for SequenceNumber
            /// </summary>
            public const int SequenceNumber = 1;

            /// <summary>
            /// Property Indexer for Description
            /// </summary>
            public const int Description = 2;

            /// <summary>
            /// Property Indexer for ReceiptDate
            /// </summary>
            public const int ReceiptDate = 3;

            /// <summary>
            /// Property Indexer for FiscalYear
            /// </summary>
            public const int FiscalYear = 4;

            /// <summary>
            /// Property Indexer for FiscalPeriod
            /// </summary>
            public const int FiscalPeriod = 5;

            /// <summary>
            /// Property Indexer for PurchaseOrderNumber
            /// </summary>
            public const int PurchaseOrderNumber = 6;

            /// <summary>
            /// Property Indexer for Reference
            /// </summary>
            public const int Reference = 7;

            /// <summary>
            /// Property Indexer for ReceiptType
            /// </summary>
            public const int ReceiptType = 8;

            /// <summary>
            /// Property Indexer for RateOperation
            /// </summary>
            public const int RateOperation = 9;

            /// <summary>
            /// Property Indexer for VendorNumber
            /// </summary>
            public const int VendorNumber = 10;

            /// <summary>
            /// Property Indexer for ReceiptCurrency
            /// </summary>
            public const int ReceiptCurrency = 11;

            /// <summary>
            /// Property Indexer for ExchangeRate
            /// </summary>
            public const int ExchangeRate = 12;

            /// <summary>
            /// Property Indexer for RateType
            /// </summary>
            public const int RateType = 13;

            /// <summary>
            /// Property Indexer for RateDate
            /// </summary>
            public const int RateDate = 14;

            /// <summary>
            /// Property Indexer for RateOverride
            /// </summary>
            public const int RateOverride = 15;

            /// <summary>
            /// Property Indexer for AdditionalCost
            /// </summary>
            public const int AdditionalCost = 16;

            /// <summary>
            /// Property Indexer for OrigAdditionalCostFunc
            /// </summary>
            public const int OrigAdditionalCostFunc = 17;

            /// <summary>
            /// Property Indexer for OrigAdditionalCostSource
            /// </summary>
            public const int OrigAdditionalCostSource = 18;

            /// <summary>
            /// Property Indexer for AdditionalCostCurrency
            /// </summary>
            public const int AdditionalCostCurrency = 19;

            /// <summary>
            /// Property Indexer for TotalExtendedCostFunctional
            /// </summary>
            public const int TotalExtendedCostFunctional = 20;

            /// <summary>
            /// Property Indexer for TotalExtendedCostSource
            /// </summary>
            public const int TotalExtendedCostSource = 21;

            /// <summary>
            /// Property Indexer for TotalExtendedCostAdjusted
            /// </summary>
            public const int TotalExtendedCostAdjusted = 22;

            /// <summary>
            /// Property Indexer for TotalAdjustedCostFunctional
            /// </summary>
            public const int TotalAdjustedCostFunctional = 23;

            /// <summary>
            /// Property Indexer for TotalReturnCost
            /// </summary>
            public const int TotalReturnCost = 24;

            /// <summary>
            /// Property Indexer for NumberOfDetailswithCost
            /// </summary>
            public const int NumberOfDetailswithCost = 25;

            /// <summary>
            /// Property Indexer for RequireLabels
            /// </summary>
            public const int RequireLabels = 26;

            /// <summary>
            /// Property Indexer for AdditionalCostAllocationType
            /// </summary>
            public const int AdditionalCostAllocationType = 27;

            /// <summary>
            /// Property Indexer for Complete
            /// </summary>
            public const int Complete = 28;

            /// <summary>
            /// Property Indexer for OriginalTotalCostSource
            /// </summary>
            public const int OriginalTotalCostSource = 29;

            /// <summary>
            /// Property Indexer for OriginalTotalCostFunctional
            /// </summary>
            public const int OriginalTotalCostFunctional = 30;

            /// <summary>
            /// Property Indexer for AdditionalCostFunctional
            /// </summary>
            public const int AdditionalCostFunctional = 31;

            /// <summary>
            /// Property Indexer for TotalCostReceiptAdditional
            /// </summary>
            public const int TotalCostReceiptAdditional = 32;

            /// <summary>
            /// Property Indexer for TotalAdjCostReceiptAddl
            /// </summary>
            public const int TotalAdjCostReceiptAddl = 33;

            /// <summary>
            /// Property Indexer for ReceiptCurrencyDecimals
            /// </summary>
            public const int ReceiptCurrencyDecimals = 34;

            /// <summary>
            /// Property Indexer for VendorShortName
            /// </summary>
            public const int VendorShortName = 35;

            /// <summary>
            /// Property Indexer for ICUniqueDocumentNumber
            /// </summary>
            public const int ICUniqueDocumentNumber = 36;

            /// <summary>
            /// Property Indexer for VendorExists
            /// </summary>
            public const int VendorExists = 37;

            /// <summary>
            /// Property Indexer for RecordDeleted
            /// </summary>
            public const int RecordDeleted = 38;

            /// <summary>
            /// Property Indexer for TransactionNumber
            /// </summary>
            public const int TransactionNumber = 39;

            /// <summary>
            /// Property Indexer for RecordStatus
            /// </summary>
            public const int RecordStatus = 40;

            /// <summary>
            /// Property Indexer for ReceiptNumber
            /// </summary>
            public const int ReceiptNumber = 41;

            /// <summary>
            /// Property Indexer for NextDetailLineNumber
            /// </summary>
            public const int NextDetailLineNumber = 42;

            /// <summary>
            /// Property Indexer for RecordPrinted
            /// </summary>
            public const int RecordPrinted = 43;

            /// <summary>
            /// Property Indexer for PostSequenceNumber
            /// </summary>
            public const int PostSequenceNumber = 44;

            /// <summary>
            /// Property Indexer for OptionalFields
            /// </summary>
            public const int OptionalFields = 45;

            /// <summary>
            /// Property Indexer for ProcessCommand
            /// </summary>
            public const int ProcessCommand = 46;

            /// <summary>
            /// Property Indexer for VendorName
            /// </summary>
            public const int VendorName = 47;

            /// <summary>
            /// Property Indexer for EnteredBy
            /// </summary>
            public const int EnteredBy = 48;

            /// <summary>
            /// Property Indexer for PostingDate
            /// </summary>
            public const int PostingDate = 49;


        }

        #endregion
        public class Keys
        {
            /// <summary>
            /// Property Indexer for ReceiptNumber
            /// </summary>
            public const int ReceiptNumber = 2;
        }
    }
}