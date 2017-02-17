// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.CSQuery.Model
{
    /// <summary>
    /// Order(header) model class
    /// </summary>
    public class Order : ModelBase
    {
        public const string EntityName = "OE0520";
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public class Keys
        {
            public const int OrderUniquifier = 0;
            public const int OrderNumber = 1;
        }
        [Key]
        public decimal OrderUniquifier{get;set;}
        [Key]
        public string OrderNumber {get;set;}
        public DateTime OrderDate { get; set; }
        public string OrderComment { get; set; }
        public string CustomerNumber {get;set;}
        public string CustomerGroupCode {get;set;}
        public string BillToName {get;set;}
        public string BillToAddressLine1 {get;set;}
        public string ShipToName {get;set;}
        public string ShipToCity {get;set;}
        public string ShipToStateProvince {get;set;}
        public string ShipToZipPostalCode {get;set;}
        public string ShipToCountry {get;set;}
        public string ShipToPhoneNumber {get;set;}
        public string ShipToFaxNumber {get;set;}
        public string ShipToContact {get;set;}

        public List<OrderDetail> OrderDetails { get; set; }

    }
    /// <summary>
    /// OrderDetail(detail entity) model class
    /// </summary>
    public class OrderDetail : ModelBase
    {
        [Key]
        public decimal OrderUniquifier { get; set; }
        [Key]
        public int LineNumber { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string ItemAccountSet { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal OrderUnitPrice { get; set; }
        public decimal OrderUnitCost { get; set; }
    }

    /// <summary>
    /// View entity fields name, the name value can get from Sage 300 SDK view or database table column name or using code generation wizard 
    /// </summary>
    public class OrderFields
    {
        //Header field constant mapping fields
        public const string OrderUniquifier = "ORDUNIQ";

        public const string OrderNumber = "ORDNUMBER";

        public const string CustomerNumber = "CUSTOMER";

        public const string CustomerGroupCode = "CUSTGROUP";

        public const string BillToName = "BILNAME";

        public const string BillToAddressLine1 = "BILADDR1";

        public const string BillToCity = "BILCITY";

        public const string BillToStateProvince = "BILSTATE";

        public const string BillToZipPostalCode = "BILZIP";

        public const string ShipToLocationCode = "SHIPTO";

        public const string ShipToName = "SHPNAME";

        public const string ShipToAddressLine1 = "SHPADDR1";

        public const string ShipToAddressLine2 = "SHPADDR2";

        public const string ShipToAddressLine3 = "SHPADDR3";

        public const string ShipToAddressLine4 = "SHPADDR4";

        public const string ShipToCity = "SHPCITY";

        public const string ShipToStateProvince = "SHPSTATE";

        public const string ShipToZipPostalCode = "SHPZIP";

        public const string ShipToCountry = "SHPCOUNTRY";

        public const string ShipToPhoneNumber = "SHPPHONE";

        public const string ShipToFaxNumber = "SHPFAX";

        public const string ShipToContact = "SHPCONTACT";

        public const string CustomerDiscountLevel = "CUSTDISC";

        public const string DefaultPriceListCode = "PRICELIST";

        public const string PurchaseOrderNumber = "PONUMBER";

        public const string Territory = "TERRITORY";

        public const string TermsCode = "TERMS";

        public const string TotalTermsAmountDue = "TERMTTLDUE";

        public const string DiscountAvailable = "DISCAVAIL";

        public const string TermsRateOverride = "TERMOVERRD";

        public const string OrderReference = "REFERENCE";

        public const string OrderType = "TYPE";

        public const string OrderDate = "ORDDATE";

        public const string ExpectedShipDate = "EXPDATE";

        public const string QuoteExpirationDate = "QTEXPDATE";

        public const string OrderFiscalYear = "ORDFISCYR";

        public const string OrderFiscalPeriod = "ORDFISCPER";

        public const string ShipViaCode = "SHIPVIA";

        public const string ShipViaCodeDescription = "VIADESC";

        public const string LastInvoiceNumber = "LASTINVNUM";

        public const string NumberOfInvoices = "NUMINVOICE";

        public const string FreeOnBoardPoint = "FOB";

        public const string TemplateCode = "TEMPLATE";

        public const string DefaultLocationCode = "LOCATION";

        public const string OnHold = "ONHOLD";

        public const string OrderDescription = "DESC";

        public const string OrderComment = "COMMENT";
        
        //Details field constant mapping fields
        public const string LineNumber = "LINENUM";
        
        public const string Item = "ITEM";
        
        public const string Description = "DESC";
        
        public const string ItemAccountSet = "ACCTSET";
        
        public const string QuantityOrdered = "QTYORDERED";
        
        public const string OrderUnitPrice = "UNITPRICE";
        
        public const string OrderUnitCost = "UNITCOST";
    }

    /// <summary>
    /// View entity field index, the index value can get from Sage 300 SDK view or using code generation wizard 
    /// </summary>
    public class FieldsIndex
    {
        public const int OrderUniquifier = 1;

        public const int OrderNumber = 2;

        public const int CustomerNumber = 3;

        public const int CustomerGroupCode = 4;

        public const int BillToName = 5;

        public const int BillToAddressLine1 = 6;

        public const int ShipToLocationCode = 17;

        public const int ShipToName = 18;

        public const int ShipToAddressLine1 = 19;

        public const int ShipToCity = 23;

        public const int ShipToStateProvince = 24;

        public const int ShipToZipPostalCode = 25;

        public const int ShipToCountry = 26;

        public const int ShipToPhoneNumber = 27;

        public const int ShipToFaxNumber = 28;

        public const int ShipToContact = 29;

        public const int OrderDate = 40;

        public const int OrderDescription = 53;

        public const int OrderComment = 54;

    }
}