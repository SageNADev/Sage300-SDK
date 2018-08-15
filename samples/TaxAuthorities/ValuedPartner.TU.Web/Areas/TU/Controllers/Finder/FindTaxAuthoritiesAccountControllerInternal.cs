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

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.GL.Models;
using Sage.CA.SBS.ERP.Sage300.GL.Web.Controllers.Finder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;

namespace ValuedPartner.TU.Web.Areas.TU.Controllers.Finder
{
    /// <summary>
    /// Account Finder Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FindTaxAuthoritiesAccountControllerInternal<T> : FindAccountTypeControllerInternal<T> where T : Account, new()
    {
        /// <summary>
        /// Constructor to initialize FindTaxAuthoritiesAccountControllerInternal
		/// </summary>
		/// <param name="context">Request context</param>
        public FindTaxAuthoritiesAccountControllerInternal(Context context) : base(context) { }

		/// <summary>
		/// Gets ModelBase
		/// </summary>
		/// <param name="id">Id</param>
		/// <returns>First or Default Currency Code</returns>
        public override ModelBase Get(string id)
		{
            Expression<Func<T, bool>> filter = acct => acct.UnformattedAccount == id;
            return Service.FirstOrDefault(filter);
		}


        /// <summary>
        /// Returns mandatory columns (other than keys)
        /// </summary>
        /// <returns>List of mandatory columns</returns>
        public override List<string> GetMandatoryColumns()
        {
            return new List<string> { "AccountNumber", "Description" };
        }

        /// <summary>
        /// Returns the default columns 
        /// </summary>
        /// <returns>List of Default Columns</returns>
        public override List<string> GetDefaultColumns()
        {
            var defaultColumns = new List<string>{
                "AccountNumber", "Description", "StatusDescription","AccountTypeString",
                "StructureCode","AllocationDescription","QuantitiesString","UnitofMeasure"};
            SetColumnVisibilityParameter();
            if (_isMulticurrency)
            {
                defaultColumns.Add("MulticurrencyString");
            }
            return defaultColumns;
        }
    }
}