// The MIT License (MIT) 
// Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.
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
     /// Contains list of ReceiptDetailSerialNumber Constants
     /// </summary>
     public partial class ReceiptDetailSerialNumber
     {
          /// <summary>
          /// View Name
          /// </summary>
          public const string EntityName = "IC0587";

          #region Properties
          /// <summary>
          /// Contains list of ReceiptDetailSerialNumber Constants
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
               /// Property for SerialNumber
               /// </summary>
               public const string SerialNumber = "SERIALNUMF";

               /// <summary>
               /// Property for SerialReturned
               /// </summary>
               public const string SerialReturned = "MOVED";

               /// <summary>
               /// Property for TransactionQuantity
               /// </summary>
               public const string TransactionQuantity = "QTY";

               /// <summary>
               /// Property for SerialQuantityReturned
               /// </summary>
               public const string SerialQuantityReturned = "QTYMOVED";

          }
          #endregion

          #region Properties
          /// <summary>
          /// Contains list of ReceiptDetailSerialNumber Constants
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
               /// Property Indexer for SerialNumber
               /// </summary>
               public const int SerialNumber = 3;

               /// <summary>
               /// Property Indexer for SerialReturned
               /// </summary>
               public const int SerialReturned = 4;

               /// <summary>
               /// Property Indexer for TransactionQuantity
               /// </summary>
               public const int TransactionQuantity = 51;

               /// <summary>
               /// Property Indexer for SerialQuantityReturned
               /// </summary>
               public const int SerialQuantityReturned = 52;

          }
          #endregion

     }
}
