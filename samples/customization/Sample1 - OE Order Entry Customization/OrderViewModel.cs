using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISV1.web.Areas.CU.Models
{
    /// <summary>
    /// ToDo: Add custom view model for screen customiztion
    /// </summary>
    public class OrderViewModel
    {
        public List<string> CustomOrderType
        {
            get
            {
                return new List<string> { "Order Type 1", "Order Type 2", "Order Type 3", "Order Type 4" };
            }
        }
        public string CustomOrderNumber { get; set; }
        public string CustomOrderComments { get; set; }
        public DateTime CustomOrderDate { get; set; }
        public double CustomOrdNumberAmount { get; set; }
        public bool CustomIsActiveOrder { get; set; }
        public string CustomOrderCurrency { get; set; }
    }
}