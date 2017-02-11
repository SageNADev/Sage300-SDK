using ISV1.web.Areas.CU.DAL.ADO_EF.Model;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.ADO_EF.ViewModel
{
    public class ARCustomerViewModel<T> : ViewModelBase<T> where T : ARCustomer, new()
    {
        /// <summary>
        /// Constructor Customer
        /// </summary>
        public ARCustomerViewModel()
        {
        }

        #region UI Properties

        public string CurrencyCodeDescription { get; set; }

        public bool IsMultiCurrency { get; set; }

        #endregion
    }
}