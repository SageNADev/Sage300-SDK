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

namespace ValuedParter.TU.Models
{
    /// <summary>
    /// Contains list of ReceiptDetail Constants
    /// </summary>
    public partial class ReceiptDetail
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public const string EntityName = "IC0580";


        #region Properties

        /// <summary>
        /// Contains list of ReceiptDetail Field Constants
        /// </summary>
        public class Fields
        {
            /// <summary>
            /// Property for SequenceNumber
            /// </summary>
            public const string SequenceNumber = "SEQUENCENO";

            /// <summary>
            /// Property for LineNumber
            /// </summary>
            public const string LineNumber = "LINENO";

            /// <summary>
            /// Property for ItemNumber
            /// </summary>
            public const string ItemNumber = "ITEMNO";

            /// <summary>
            /// Property for ItemDescription
            /// </summary>
            public const string ItemDescription = "ITEMDESC";

            /// <summary>
            /// Property for Category
            /// </summary>
            public const string Category = "CATEGORY";

            /// <summary>
            /// Property for Location
            /// </summary>
            public const string Location = "LOCATION";

            /// <summary>
            /// Property for QuantityReceived
            /// </summary>
            public const string QuantityReceived = "RECPQTY";

            /// <summary>
            /// Property for QuantityReturned
            /// </summary>
            public const string QuantityReturned = "RETURNQTY";

            /// <summary>
            /// Property for UnitOfMeasure
            /// </summary>
            public const string UnitOfMeasure = "RECPUNIT";

            /// <summary>
            /// Property for ConversionFactor
            /// </summary>
            public const string ConversionFactor = "CONVERSION";

            /// <summary>
            /// Property for ProratedAddlCostFunc
            /// </summary>
            public const string ProratedAddlCostFunc = "ADDCSTHM";

            /// <summary>
            /// Property for ProratedAddlCostSrc
            /// </summary>
            public const string ProratedAddlCostSrc = "ADDCSTSRC";

            /// <summary>
            /// Property for UnitCost
            /// </summary>
            public const string UnitCost = "UNITCOST";

            /// <summary>
            /// Property for AdjustedUnitCost
            /// </summary>
            public const string AdjustedUnitCost = "ADJUNITCST";

            /// <summary>
            /// Property for AdjustedCost
            /// </summary>
            public const string AdjustedCost = "ADJCOST";

            /// <summary>
            /// Property for AdjustedCostFunctional
            /// </summary>
            public const string AdjustedCostFunctional = "ADJCOSTHM";

            /// <summary>
            /// Property for ExtendedCost
            /// </summary>
            public const string ExtendedCost = "RECPCOST";

            /// <summary>
            /// Property for ExtendedCostFunctional
            /// </summary>
            public const string ExtendedCostFunctional = "RECPCOSTHM";

            /// <summary>
            /// Property for ReturnCost
            /// </summary>
            public const string ReturnCost = "RETURNCOST";

            /// <summary>
            /// Property for CostingDate
            /// </summary>
            public const string CostingDate = "COSTDATE";

            /// <summary>
            /// Property for CostingSequenceNo
            /// </summary>
            public const string CostingSequenceNo = "COSTSEQNO";

            /// <summary>
            /// Property for OriginalReceiptQty
            /// </summary>
            public const string OriginalReceiptQty = "ORIGQTY";

            /// <summary>
            /// Property for OriginalUnitCost
            /// </summary>
            public const string OriginalUnitCost = "ORIGUNTCST";

            /// <summary>
            /// Property for OriginalExtendedCost
            /// </summary>
            public const string OriginalExtendedCost = "ORIGEXTCST";

            /// <summary>
            /// Property for OriginalExtendedCostFunc
            /// </summary>
            public const string OriginalExtendedCostFunc = "ORIGEXTHM";

            /// <summary>
            /// Property for Comments
            /// </summary>
            public const string Comments = "COMMENTS";

            /// <summary>
            /// Property for Labels
            /// </summary>
            public const string Labels = "LABELS";

            /// <summary>
            /// Property for StockItem
            /// </summary>
            public const string StockItem = "STOCKITEM";

            /// <summary>
            /// Property for ManufacturersItemNumber
            /// </summary>
            public const string ManufacturersItemNumber = "MANITEMNO";

            /// <summary>
            /// Property for VendorItemNumber
            /// </summary>
            public const string VendorItemNumber = "VENDITEMNO";

            /// <summary>
            /// Property for DetailLineNumber
            /// </summary>
            public const string DetailLineNumber = "DETAILNUM";

            /// <summary>
            /// Property for QuantityReturnedToDate
            /// </summary>
            public const string QuantityReturnedToDate = "RETQTY";

            /// <summary>
            /// Property for ReturnedExtCostToDate
            /// </summary>
            public const string ReturnedExtCostToDate = "RETEXTCST";

            /// <summary>
            /// Property for ReturnedExtCostFuncToDate
            /// </summary>
            public const string ReturnedExtCostFuncToDate = "RETEXTHM";

            /// <summary>
            /// Property for AdjustedExtCostToDate
            /// </summary>
            public const string AdjustedExtCostToDate = "ADJEXTCST";

            /// <summary>
            /// Property for AdjustedExtCostFuncToDate
            /// </summary>
            public const string AdjustedExtCostFuncToDate = "ADJEXTHM";

            /// <summary>
            /// Property for PreviousDayEndReceiptQty
            /// </summary>
            public const string PreviousDayEndReceiptQty = "PREVQTY";

            /// <summary>
            /// Property for PreviousDayEndUnitCost
            /// </summary>
            public const string PreviousDayEndUnitCost = "PREVUNTCST";

            /// <summary>
            /// Property for PreviousDayEndExtCost
            /// </summary>
            public const string PreviousDayEndExtCost = "PREVEXTCST";

            /// <summary>
            /// Property for PreviousDayEndExtCostFunc
            /// </summary>
            public const string PreviousDayEndExtCostFunc = "PREVEXTHM";

            /// <summary>
            /// Property for UnformattedItemNumber
            /// </summary>
            public const string UnformattedItemNumber = "UNFMTITMNO";

            /// <summary>
            /// Property for CheckBelowZero
            /// </summary>
            public const string CheckBelowZero = "CHKBELZERO";

            /// <summary>
            /// Property for RevisionListLineNumber
            /// </summary>
            public const string RevisionListLineNumber = "REVLINE";

            /// <summary>
            /// Property for InterprocessCommID
            /// </summary>
            public const string InterprocessCommID = "IPCID";

            /// <summary>
            /// Property for ForcePopupSN
            /// </summary>
            public const string ForcePopupSN = "FORCEPOPSN";

            /// <summary>
            /// Property for PopupSN
            /// </summary>
            public const string PopupSN = "POPUPSN";

            /// <summary>
            /// Property for CloseSN
            /// </summary>
            public const string CloseSN = "CLOSESN";

            /// <summary>
            /// Property for LTSetID
            /// </summary>
            public const string LTSetID = "LTSETID";

            /// <summary>
            /// Property for ForcePopupLT
            /// </summary>
            public const string ForcePopupLT = "FORCEPOPLT";

            /// <summary>
            /// Property for PopupLT
            /// </summary>
            public const string PopupLT = "POPUPLT";

            /// <summary>
            /// Property for CloseLT
            /// </summary>
            public const string CloseLT = "CLOSELT";

            /// <summary>
            /// Property for OptionalFields
            /// </summary>
            public const string OptionalFields = "VALUES";

            /// <summary>
            /// Property for ProcessCommand
            /// </summary>
            public const string ProcessCommand = "PROCESSCMD";

            /// <summary>
            /// Property for SerialQuantity
            /// </summary>
            public const string SerialQuantity = "SERIALQTY";

            /// <summary>
            /// Property for LotQuantity
            /// </summary>
            public const string LotQuantity = "LOTQTY";

            /// <summary>
            /// Property for SerialQuantityReturned
            /// </summary>
            public const string SerialQuantityReturned = "SQTYMOVED";

            /// <summary>
            /// Property for LotQuantityReturned
            /// </summary>
            public const string LotQuantityReturned = "LQTYMOVED";

            /// <summary>
            /// Property for SerialLotQuantityToProcess
            /// </summary>
            public const string SerialLotQuantityToProcess = "XGENALCQTY";

            /// <summary>
            /// Property for NumberOfLotsToGenerate
            /// </summary>
            public const string NumberOfLotsToGenerate = "XLOTMAKQTY";

            /// <summary>
            /// Property for QuantityperLot
            /// </summary>
            public const string QuantityperLot = "XPERLOTQTY";

            /// <summary>
            /// Property for ReceiptType
            /// </summary>
            public const string ReceiptType = "RECPTYPE";

            /// <summary>
            /// Property for AllocateFromSerial
            /// </summary>
            public const string AllocateFromSerial = "SALLOCFROM";

            /// <summary>
            /// Property for AllocateFromLot
            /// </summary>
            public const string AllocateFromLot = "LALLOCFROM";

            /// <summary>
            /// Property for SerialLotWindowHandle
            /// </summary>
            public const string SerialLotWindowHandle = "METERHWND";

        }

        #endregion
        #region Properties

        /// <summary>
        /// Contains list of ReceiptDetail Index Constants
        /// </summary>
        public class Index
        {
            /// <summary>
            /// Property Indexer for SequenceNumber
            /// </summary>
            public const int SequenceNumber = 1;

            /// <summary>
            /// Property Indexer for LineNumber
            /// </summary>
            public const int LineNumber = 2;

            /// <summary>
            /// Property Indexer for ItemNumber
            /// </summary>
            public const int ItemNumber = 3;

            /// <summary>
            /// Property Indexer for ItemDescription
            /// </summary>
            public const int ItemDescription = 4;

            /// <summary>
            /// Property Indexer for Category
            /// </summary>
            public const int Category = 5;

            /// <summary>
            /// Property Indexer for Location
            /// </summary>
            public const int Location = 6;

            /// <summary>
            /// Property Indexer for QuantityReceived
            /// </summary>
            public const int QuantityReceived = 7;

            /// <summary>
            /// Property Indexer for QuantityReturned
            /// </summary>
            public const int QuantityReturned = 8;

            /// <summary>
            /// Property Indexer for UnitOfMeasure
            /// </summary>
            public const int UnitOfMeasure = 9;

            /// <summary>
            /// Property Indexer for ConversionFactor
            /// </summary>
            public const int ConversionFactor = 10;

            /// <summary>
            /// Property Indexer for ProratedAddlCostFunc
            /// </summary>
            public const int ProratedAddlCostFunc = 11;

            /// <summary>
            /// Property Indexer for ProratedAddlCostSrc
            /// </summary>
            public const int ProratedAddlCostSrc = 12;

            /// <summary>
            /// Property Indexer for UnitCost
            /// </summary>
            public const int UnitCost = 13;

            /// <summary>
            /// Property Indexer for AdjustedUnitCost
            /// </summary>
            public const int AdjustedUnitCost = 14;

            /// <summary>
            /// Property Indexer for AdjustedCost
            /// </summary>
            public const int AdjustedCost = 15;

            /// <summary>
            /// Property Indexer for AdjustedCostFunctional
            /// </summary>
            public const int AdjustedCostFunctional = 16;

            /// <summary>
            /// Property Indexer for ExtendedCost
            /// </summary>
            public const int ExtendedCost = 17;

            /// <summary>
            /// Property Indexer for ExtendedCostFunctional
            /// </summary>
            public const int ExtendedCostFunctional = 18;

            /// <summary>
            /// Property Indexer for ReturnCost
            /// </summary>
            public const int ReturnCost = 19;

            /// <summary>
            /// Property Indexer for CostingDate
            /// </summary>
            public const int CostingDate = 20;

            /// <summary>
            /// Property Indexer for CostingSequenceNo
            /// </summary>
            public const int CostingSequenceNo = 21;

            /// <summary>
            /// Property Indexer for OriginalReceiptQty
            /// </summary>
            public const int OriginalReceiptQty = 22;

            /// <summary>
            /// Property Indexer for OriginalUnitCost
            /// </summary>
            public const int OriginalUnitCost = 23;

            /// <summary>
            /// Property Indexer for OriginalExtendedCost
            /// </summary>
            public const int OriginalExtendedCost = 24;

            /// <summary>
            /// Property Indexer for OriginalExtendedCostFunc
            /// </summary>
            public const int OriginalExtendedCostFunc = 25;

            /// <summary>
            /// Property Indexer for Comments
            /// </summary>
            public const int Comments = 26;

            /// <summary>
            /// Property Indexer for Labels
            /// </summary>
            public const int Labels = 27;

            /// <summary>
            /// Property Indexer for StockItem
            /// </summary>
            public const int StockItem = 28;

            /// <summary>
            /// Property Indexer for ManufacturersItemNumber
            /// </summary>
            public const int ManufacturersItemNumber = 29;

            /// <summary>
            /// Property Indexer for VendorItemNumber
            /// </summary>
            public const int VendorItemNumber = 30;

            /// <summary>
            /// Property Indexer for DetailLineNumber
            /// </summary>
            public const int DetailLineNumber = 31;

            /// <summary>
            /// Property Indexer for QuantityReturnedToDate
            /// </summary>
            public const int QuantityReturnedToDate = 32;

            /// <summary>
            /// Property Indexer for ReturnedExtCostToDate
            /// </summary>
            public const int ReturnedExtCostToDate = 33;

            /// <summary>
            /// Property Indexer for ReturnedExtCostFuncToDate
            /// </summary>
            public const int ReturnedExtCostFuncToDate = 34;

            /// <summary>
            /// Property Indexer for AdjustedExtCostToDate
            /// </summary>
            public const int AdjustedExtCostToDate = 35;

            /// <summary>
            /// Property Indexer for AdjustedExtCostFuncToDate
            /// </summary>
            public const int AdjustedExtCostFuncToDate = 36;

            /// <summary>
            /// Property Indexer for PreviousDayEndReceiptQty
            /// </summary>
            public const int PreviousDayEndReceiptQty = 37;

            /// <summary>
            /// Property Indexer for PreviousDayEndUnitCost
            /// </summary>
            public const int PreviousDayEndUnitCost = 38;

            /// <summary>
            /// Property Indexer for PreviousDayEndExtCost
            /// </summary>
            public const int PreviousDayEndExtCost = 39;

            /// <summary>
            /// Property Indexer for PreviousDayEndExtCostFunc
            /// </summary>
            public const int PreviousDayEndExtCostFunc = 40;

            /// <summary>
            /// Property Indexer for UnformattedItemNumber
            /// </summary>
            public const int UnformattedItemNumber = 41;

            /// <summary>
            /// Property Indexer for CheckBelowZero
            /// </summary>
            public const int CheckBelowZero = 42;

            /// <summary>
            /// Property Indexer for RevisionListLineNumber
            /// </summary>
            public const int RevisionListLineNumber = 43;

            /// <summary>
            /// Property Indexer for InterprocessCommID
            /// </summary>
            public const int InterprocessCommID = 44;

            /// <summary>
            /// Property Indexer for ForcePopupSN
            /// </summary>
            public const int ForcePopupSN = 45;

            /// <summary>
            /// Property Indexer for PopupSN
            /// </summary>
            public const int PopupSN = 46;

            /// <summary>
            /// Property Indexer for CloseSN
            /// </summary>
            public const int CloseSN = 47;

            /// <summary>
            /// Property Indexer for LTSetID
            /// </summary>
            public const int LTSetID = 48;

            /// <summary>
            /// Property Indexer for ForcePopupLT
            /// </summary>
            public const int ForcePopupLT = 49;

            /// <summary>
            /// Property Indexer for PopupLT
            /// </summary>
            public const int PopupLT = 50;

            /// <summary>
            /// Property Indexer for CloseLT
            /// </summary>
            public const int CloseLT = 51;

            /// <summary>
            /// Property Indexer for OptionalFields
            /// </summary>
            public const int OptionalFields = 52;

            /// <summary>
            /// Property Indexer for ProcessCommand
            /// </summary>
            public const int ProcessCommand = 53;

            /// <summary>
            /// Property Indexer for SerialQuantity
            /// </summary>
            public const int SerialQuantity = 54;

            /// <summary>
            /// Property Indexer for LotQuantity
            /// </summary>
            public const int LotQuantity = 55;

            /// <summary>
            /// Property Indexer for SerialQuantityReturned
            /// </summary>
            public const int SerialQuantityReturned = 56;

            /// <summary>
            /// Property Indexer for LotQuantityReturned
            /// </summary>
            public const int LotQuantityReturned = 57;

            /// <summary>
            /// Property Indexer for SerialLotQuantityToProcess
            /// </summary>
            public const int SerialLotQuantityToProcess = 58;

            /// <summary>
            /// Property Indexer for NumberOfLotsToGenerate
            /// </summary>
            public const int NumberOfLotsToGenerate = 59;

            /// <summary>
            /// Property Indexer for QuantityperLot
            /// </summary>
            public const int QuantityperLot = 60;

            /// <summary>
            /// Property Indexer for ReceiptType
            /// </summary>
            public const int ReceiptType = 61;

            /// <summary>
            /// Property Indexer for AllocateFromSerial
            /// </summary>
            public const int AllocateFromSerial = 62;

            /// <summary>
            /// Property Indexer for AllocateFromLot
            /// </summary>
            public const int AllocateFromLot = 63;

            /// <summary>
            /// Property Indexer for SerialLotWindowHandle
            /// </summary>
            public const int SerialLotWindowHandle = 64;


        }

        #endregion

    }
}