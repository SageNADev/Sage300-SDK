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

using System.Globalization;
using System.Linq;

namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
{
    /// <summary>
    /// Static helper class to assist with source
    /// </summary>
    public static class SourceHelper
    {
        #region Private Constants

        #endregion

        #region Public Methods

        /// <summary>
        /// Helper method that removes and replaces unwanted characters
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Replaced string</returns>
        public static string Replace(string value)
        {
            if (value == string.Empty)
            {
                return string.Empty;
            }

            // Convert to Pascal Case First, but only if there are spaces in value (else it has already been done)
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var pascalCase = value.Contains(" ") ? textInfo.ToTitleCase(value) : value;

            var newString = pascalCase
                .Replace("Add'l", "Additional")
                .Replace("Addt'l", "Additional")
                .Replace("Ret'd", "Returned")
                .Replace("State/Prov.", "StateProvince")
                .Replace("Company/Org.", "CompanyOrganization")
                .Replace("Distrib.", "Distribution")
                .Replace("Insuff.", "Insufficient")
                .Replace("Prepay.", "Prepayment")
                .Replace("Unreal.", "Unrealized")
                .Replace("Alloc.", "Allocated")
                .Replace("Avail.", "Available")
                .Replace("Jrnls.", "Journals")
                .Replace("Quant.", "Quantity")
                .Replace("Reval.", "Revaluation")
                .Replace("Recon.", "Reconcilation")
                .Replace("Sched.", "Schedule")
                .Replace("Trans.", "Transaction")
                .Replace("Acct.", "Account")
                .Replace("Auth.", "Authority")
                .Replace("Calc.", "Calculation")
                .Replace("Curr.", "Currency")
                .Replace("Cust.", "Customer")
                .Replace("Desc.", "Description")
                .Replace("Dest.", "Destination")
                .Replace("Dist.", "Distribution")
                .Replace("Fisc.", "Fiscal")
                .Replace("Func.", "Functional")
                .Replace("incl.", "Included")
                .Replace("Exch.", "Exchange")
                .Replace("excl.", "Excluded")
                .Replace("Info.", "Information")
                .Replace("Lgst.", "Largest")
                .Replace("Larg.", "Largest")
                .Replace("Misc.", "Miscellaneous")
                .Replace("Orig.", "Original")
                .Replace("Prov.", "Provisional")
                .Replace("Rcpt.", "Receipt")
                .Replace("Rtng.", "Retainage")
                .Replace("Srce.", "Source")
                .Replace("Stats.", "Statistic")
                .Replace("Vend.", "Vendor")
                .Replace("Warr.", "Warranty")
                .Replace("Adj.", "Adjustment")
                .Replace("Amt.", "Amount")
                .Replace("Bal.", "Balance")
                .Replace("Chk.", "Check")
                .Replace("Clr.", "Clearing")
                .Replace("Cur.", "Currency")
                .Replace("Dep.", "Deposit")
                .Replace("Doc.", "Document")
                .Replace("Exp.", "Expense")
                .Replace("Fwd.", "Forward")
                .Replace("diff.", "Difference")
                .Replace("Inc.", "Include")
                .Replace("Inv.", "Invoice")
                .Replace("Int.", "Interest")
                .Replace("Opt.", "Optional")
                .Replace("Ord.", "Ordered")
                .Replace("Per.", "Period")
                .Replace("Qty.", "Quantity")
                .Replace("Rtg.", "Retainage")
                .Replace("Src.", "Source")
                .Replace("Sep.", "Separately")
                .Replace("Seq.", "Sequence")
                .Replace("Sys.", "System")
                .Replace("Cr.", "Credit")
                .Replace("Dr.", "Debit")
                .Replace("Ex.", "Exchange")
                .Replace("No.", "Number")
                .Replace("Pd.", "Paid")
                .Replace("Yr.", "Year")
                .Replace("C.C.", "CreditCard")
                .Replace("w/ ", "With")
                .Replace("w/o ", "Without")
                .Replace("->", "To")
                .Replace(" ", "")
                .Replace("/", "")
                .Replace(@"\", "")
                .Replace("*", "")
                .Replace("#", "")
                .Replace("-", "")
                .Replace(".", "")
                .Replace("'", "")
                .Replace(":", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace(",", "")
                .Replace("&", "");

            if (newString.Length > 0)
            {
                var num = newString.ToArray()[0];
                if (char.IsNumber(num))
                {
                    newString = "Num" + newString;
                }

            }

            if (string.CompareOrdinal(newString, "OptionalFields") == 0)
            {
                return "NumberOfOptionalFields";
            }

            return newString;
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
