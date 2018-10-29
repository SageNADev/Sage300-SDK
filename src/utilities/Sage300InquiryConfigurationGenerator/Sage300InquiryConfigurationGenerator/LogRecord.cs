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

#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Sage300InquiryConfigurationGenerator
{
    public class LogRecord
    {
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Transaction Detail
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Transaction Start Time
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Transaction End Time
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Time span in milliseconds
        /// </summary>
        public Double Timespan
        {
            get
            {
                return ((TimeSpan)(End - Start)).TotalMilliseconds;
            }
        }

        /// <summary>
        /// Status of the transaction
        /// </summary>
        public string Status { get; set; }

        public LogRecord(DateTime start)
        {
            Description = "";
            Detail = "";
            ErrorMessage = "";
            Start = start;
            End = DateTime.Now;
            Status = "";
        }
        public LogRecord(DateTime start, string status, string description, string detail, string error)
        {
            Description = description;
            Detail = detail;
            ErrorMessage = error;
            Start = start;
            End = DateTime.Now;
            Status = status;
        }
    }
}
