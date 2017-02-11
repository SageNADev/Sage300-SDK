using ISV1.web.Areas.CU.DAL.SageViews.Model;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.SageViews.ViewModel
{
    public class CustomerViewModel<T> : ViewModelBase<T> where T : Customer, new()
    {
        /// <summary>
        /// Constructor Customer
        /// </summary>
        public CustomerViewModel()
        {
        }

        #region UI Properties

        public string CurrencyCodeDescription { get; set; }

        public bool IsMultiCurrency { get; set; }

        #endregion
    }
}