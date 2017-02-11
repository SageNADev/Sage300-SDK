using ISV1.web.Areas.CU.DAL.CSQuery.Model;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.CSQuery.ViewModel
{
       public class OrderViewModel<T> : ViewModelBase<T> where T : Order, new()
    {
        /// <summary>
        /// Constructor Customer
        /// </summary>
        public OrderViewModel()
        {
        }

        #region UI Properties

        public string OrderCurrencyCode { get; set; }

        #endregion
    }

}