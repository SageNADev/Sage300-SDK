// The MIT License (MIT) 
// Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved.
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

namespace ValuedPartner.TU.Models
{
     /// <summary>
     /// Contains list of ReceiptDetailLotNumber Constants
     /// </summary>
     public partial class ReceiptDetailLotNumber
     {
          /// <summary>
          /// View Name
          /// </summary>
          public const string EntityName = "IC0582";

          #region Properties
          /// <summary>
          /// Contains list of ReceiptDetailLotNumber Constants
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
               /// Property for LotNumber
               /// </summary>
               public const string LotNumber = "LOTNUMF";

               /// <summary>
               /// Property for ExpiryDate
               /// </summary>
               public const string ExpiryDate = "EXPIRYDATE";

               /// <summary>
               /// Property for TransactionQuantity
               /// </summary>
               public const string TransactionQuantity = "QTY";

               /// <summary>
               /// Property for LotQuantityInStockingUOM
               /// </summary>
               public const string LotQuantityInStockingUOM = "QTYSQ";

               /// <summary>
               /// Property for LotQuantityReturned
               /// </summary>
               public const string LotQuantityReturned = "QTYMOVED";

               /// <summary>
               /// Property for LotQtyReturnedInStockingUOM
               /// </summary>
               public const string LotQtyReturnedInStockingUOM = "QTYMOVEDSQ";

          }
          #endregion

          #region Properties
          /// <summary>
          /// Contains list of ReceiptDetailLotNumber Constants
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
               /// Property Indexer for LotNumber
               /// </summary>
               public const int LotNumber = 3;

               /// <summary>
               /// Property Indexer for ExpiryDate
               /// </summary>
               public const int ExpiryDate = 4;

               /// <summary>
               /// Property Indexer for TransactionQuantity
               /// </summary>
               public const int TransactionQuantity = 5;

               /// <summary>
               /// Property Indexer for LotQuantityInStockingUOM
               /// </summary>
               public const int LotQuantityInStockingUOM = 6;

               /// <summary>
               /// Property Indexer for LotQuantityReturned
               /// </summary>
               public const int LotQuantityReturned = 7;

               /// <summary>
               /// Property Indexer for LotQtyReturnedInStockingUOM
               /// </summary>
               public const int LotQtyReturnedInStockingUOM = 8;

          }
          #endregion

     }
}
