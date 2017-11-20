// The MIT License (MIT) 
// Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved.
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

#region Namespace

using Microsoft.Practices.Unity;
<<<<<<< HEAD
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;
=======
using ValuedParter.TU.Interfaces.Services;
using ValuedParter.TU.Models;
using ValuedParter.TU.Models.Enums;
using ValuedParter.TU.Resources.Forms;
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Finder;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Finder;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


#endregion

<<<<<<< HEAD
namespace ValuedPartner.Web.Areas.TU.Controllers.Finder
=======
namespace ValuedParter.Web.Areas.TU.Controllers.Finder
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
{
    /// <summary>
    /// Find Receipt Number Controller Internal
    /// </summary>
    /// <typeparam name="T">ReceiptHeader</typeparam>
    public class FindReceiptNumberControllerInternal : BaseFindControllerInternal<ReceiptHeader, IReceiptHeaderService>, IFinder
    {
        #region Private Members

        #endregion

        /// <summary>
        /// Constructor method to instantiate the object with Context
        /// </summary>
        /// <param name="context">A context object</param>
        public FindReceiptNumberControllerInternal(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets ModelBase
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>First or Default Price List Code</returns>
        public virtual ModelBase Get(string id)
        {
            Expression<Func<ReceiptHeader, bool>> filter = receipt => receipt.ReceiptNumber == id;
            return Service.FirstOrDefault(filter);
        }
        
        /// <summary>
        /// Fetches Columns
        /// </summary>
        /// <returns>List of Receipt Number fields</returns>
        public override IEnumerable<ModelBase> GetAllColumns()
        {
            var column = new List<ModelBase>
            { 
               
                new GridField
                {
                    field = "ReceiptNumber",
                    title = ReceiptHeaderResx.ReceiptNumber ,
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "22"},
							{"class", FinderConstant.CssClassTxtUpper},
							{FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.DataTypeString}
						}
               },
                new GridField
                {
                    field = "Description",
                    title = ReceiptHeaderResx.Description, 
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string",
                    
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "60"}
						}
              },
                 new GridField
                {
                    field = "ReceiptDate",
                    title =  ReceiptHeaderResx.ReceiptDate,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeDate,
                    template = Utilities.GetGridTemplate("ReceiptDate"), 
                },
                  new GridField
                {
                    field = "FiscalYear",
                    title = CommonResx.FiscalYear,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes =
                        new Dictionary<string, string>
                        {
                            {"class", FinderConstant.CssClassInputNumberRightAlign},
                            {FinderConstant.CustomAttributeMaximumLength, "4"},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric}
                        }
                },
                new GridField
                {
                    field = "FiscalPeriodString",
                    fieldToQuery = "FiscalPeriod",
                    title = CommonResx.FiscalPeriod,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
<<<<<<< HEAD
                    PresentationList = EnumUtility.GetItemsList<ValuedPartner.TU.Models.Enums.FiscalPeriod>()
=======
                    PresentationList = EnumUtility.GetItemsList<ValuedParter.TU.Models.Enums.FiscalPeriod>()
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
                },
                
                 new GridField
                {
                    field = "PurchaseOrderNumber",
                    title = ReceiptHeaderResx.PurchaseOrderNumber,
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string", 
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "22"},
							{"class", FinderConstant.CssClassTxtUpper}
						}
                },

                 new GridField
                {
                    field = "Reference",
                    title = ReceiptHeaderResx.Reference,
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string",
                    customAttributes = new Dictionary<string, string>
                    {
                        {"maxLength", "60"}
                    }
                },
                new GridField
                {
                    field = "ReceiptTypeString", 
                    fieldToQuery="ReceiptType",
                    title = ReceiptHeaderResx.ReceiptType, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<ReceiptType>()
                },
                new GridField
                {
                    field = "RateOperationString",
                    fieldToQuery="RateOperation",
                    title = ReceiptHeaderResx.RateOperation, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<RateOperation>()
                },
                 new GridField
                {
                    field = "VendorNumber",
                    title = ReceiptHeaderResx.VendorNumber, 
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string", 
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "12"},
							{"class", FinderConstant.CssClassTxtUpper}
						}
                },
                  new GridField
                {
                    field = "ReceiptCurrency",
                    title = ReceiptHeaderResx.ReceiptCurrency,
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string",
                     customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "3"},
							{"class", FinderConstant.CssClassTxtUpper},
							{FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
						}
                },
                 
                 
            new GridField  
            {
                field = "ExchangeRate",
                title = ReceiptHeaderResx.ExchangeRate,
                attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                template = Utilities.GetFormattedDecimal("ExchangeRate", "7"),
                dataType = FinderConstant.DataTypeNumber,
                customAttributes =
                    new Dictionary<string, string>
                    {
                        {"class", FinderConstant.CssClassInputNumberRightAlign},
                        {FinderConstant.CustomAttributeMaximumLength, "15"},
                        {"decimal", "7"}
                    }
            },


                 new GridField
                {
                    field = "RateType",
                    title = ReceiptHeaderResx.RateType, 
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string",
                    customAttributes =
                        new Dictionary<string, string>
                        { 
                            { FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric},
                            {"class", FinderConstant.CssClassTxtUpper},
                            { FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                 new GridField
                {
                    field = "RateDate",
                    title = ReceiptHeaderResx.RateDate,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeDate,
                    template = Utilities.GetGridTemplate("RateDate"), 
                }, 
                 new GridField
                {
                    field = "RateOverrideString", 
                    fieldToQuery="RateOverride",
                    title = ReceiptHeaderResx.RateOverride, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<RateOverride>()
                },  
                 new GridField
                {
                    field = "AdditionalCost",  
                    title = ReceiptHeaderResx.AdditionalCost, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(AdditionalCost,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },  

                 new GridField
                {
                    field = "OrigAdditionalCostFunc",  
                    title = ReceiptHeaderResx.OrigAdditionalCostFunc, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(OrigAdditionalCostFunc,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                }, 
                
                 new GridField
                {
                    field = "OrigAdditionalCostSource",  
                    title = ReceiptHeaderResx.OrigAdditionalCostSource, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(OrigAdditionalCostSource,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                }, 
                  new GridField
                {
                    field = "AdditionalCostCurrency",  
                    title = ReceiptHeaderResx.AdditionalCostCurrency, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                     customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "3"},
							{"class", FinderConstant.CssClassTxtUpper},
							{FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
						}
                },
                  new GridField
                {
                    field = "TotalExtendedCostFunctional",  
                    title = ReceiptHeaderResx.TotalExtendedCostFunctional, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(TotalExtendedCostFunctional,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },
                  new GridField
                {
                    field = "TotalExtendedCostSource",  
                    title = ReceiptHeaderResx.TotalExtendedCostSource, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(TotalExtendedCostSource,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },
                  new GridField
                {
                    field = "NumberOfDetailswithCost",  
                    title = ReceiptHeaderResx.NumberOfDetailswithCost, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeDecimal,
                    customAttributes =
                        new Dictionary<string, string> {
                            { FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            { FinderConstant.CustomAttributeMaximumLength, "5"},
                            { "class", FinderConstant.CssClassInputNumberRightAlign}
                    }  
                },
                  new GridField
                {
                    field = "RequireLabelsString", 
                    fieldToQuery="RequireLabels",
                    title = ReceiptHeaderResx.RequireLabels, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<RequireLabels>()
                },
                    new GridField
                {
                    field = "AddlCostAllocationTypeString", 
                    fieldToQuery="AdditionalCostAllocationType",
                    title = ReceiptHeaderResx.AdditionalCostAllocationType, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<AddlCostonRcptReturns>()
                }, 
                
                  new GridField
                {
                    field = "OriginalTotalCostSource",  
                    title = ReceiptHeaderResx.OriginalTotalCostSource, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(OriginalTotalCostSource,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },

                  new GridField
                {
                    field = "OriginalTotalCostFunctional",  
                    title = ReceiptHeaderResx.OriginalTotalCostFunctional, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(OriginalTotalCostFunctional,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },
                   new GridField
                {
                    field = "AdditionalCostFunctional",  
                    title = ReceiptHeaderResx.AdditionalCostFunctional, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(AdditionalCostFunctional,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },
                   new GridField
                {
                    field = "TotalCostReceiptAdditional",  
                    title = ReceiptHeaderResx.TotalCostReceiptAdditional, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(TotalCostReceiptAdditional,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },
                   new GridField
                {
                    field = "ReceiptCurrencyDecimals",  
                    title = ReceiptHeaderResx.ReceiptCurrencyDecimals, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                      dataType = FinderConstant.DataTypeDecimal,
                       customAttributes =
                        new Dictionary<string, string> {
                            { FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            { FinderConstant.CustomAttributeMaximumLength, "5"},
                            {"class", FinderConstant.CssClassInputNumberRightAlign}
                        }  
                },

                   new GridField
                {
                    field = "VendorShortName",  
                    title = ReceiptHeaderResx.VendorShortName, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "10"}
						}
                },
                    new GridField
                {
                    field = "VendorExistsString",  
                   fieldToQuery="VendorExists",
                    title = ReceiptHeaderResx.VendorExists, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10, 
                    PresentationList = EnumUtility.GetItemsList<VendorExists>()
                },
                   new GridField
                {
                    field = "RecordStatusString", 
                    fieldToQuery="RecordStatus",
                    title = ReceiptHeaderResx.RecordStatus,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<RecordStatus>()
                },
                  new GridField
                {
                    field = "SequenceNumber",
                    title = ReceiptHeaderResx.SequenceNumber,
                    dataType = FinderConstant.DataTypeInt,
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    customAttributes =
                        new Dictionary<string, string>
                        {
                            { "class", FinderConstant.CssClassInputNumberRightAlign },
                            { FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            { FinderConstant.CustomAttributeMaximumLength, "10"}
                        }
                }, 
                   new GridField
                {
                    field = "TotalExtendedCostAdjusted",  
                    title = ReceiptHeaderResx.TotalExtendedCostAdjusted, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(TotalExtendedCostAdjusted,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                }, 
                   new GridField
                {
                    field = "TotalAdjustedCostFunctional",  
                    title = ReceiptHeaderResx.TotalAdjustedCostFunctional, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(TotalAdjustedCostFunctional,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },
                    new GridField
                {
                    field = "TotalReturnCost",  
                    title = ReceiptHeaderResx.TotalReturnCost, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(TotalReturnCost,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                },
                    new GridField
                {
                    field = "CompleteString", 
                    fieldToQuery="Complete",
                    title = ReceiptHeaderResx.Complete, 
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<Complete>()
                }, 
                     new GridField
                {
                    field = "TotalAdjCostReceiptAddl",  
                    title = ReceiptHeaderResx.TotalAdjCostReceiptAddl, 
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
					headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    template = "#= kendo.toString(TotalAdjCostReceiptAddl,'n' + 3) #",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "16"},
							{"class", FinderConstant.CssClassInputNumberRightAlign}
						}
                }, 
                 new GridField
                {
                    field = "ICUniqueDocumentNumber",
                    title = ReceiptHeaderResx.ICUniqueDocumentNumber,
                    attributes = FinderConstant.CssClassGridColumn15NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn15, 
                    dataType = FinderConstant.DataTypeDecimal,
                    customAttributes =
                        new Dictionary<string, string>
                        {
                            {"class", FinderConstant.CssClassInputNumberRightAlign},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            {FinderConstant.CustomAttributeMaximumLength, "16"}
                        }
                },  
                 new GridField
                {
                    field = "RecordDeletedString", 
                    fieldToQuery="RecordDeleted",
                    title = ReceiptHeaderResx.RecordDeleted,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<RecordDeleted>()
                },

                  new GridField
                {
                    field = "TransactionNumber",
                    title = ReceiptHeaderResx.TransactionNumber,
                    dataType = FinderConstant.DataTypeNumber,
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    customAttributes =
                        new Dictionary<string, string>
                        {
                            { "class", FinderConstant.CssClassInputNumberRightAlign },
                            { FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            { FinderConstant.CustomAttributeMaximumLength, "16"}
                        }
                }, 
                 new GridField
                {
                    field = "NextDetailLineNumber",
                    title = ReceiptHeaderResx.NextDetailLineNumber,
                    attributes =  FinderConstant.CssClassGridColumn10NumRightAlign,
                    headerAttributes =  FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeDecimal,
                    customAttributes =
                        new Dictionary<string, string>
                        { 
                           {"class", FinderConstant.CssClassInputNumberRightAlign},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            {FinderConstant.CustomAttributeMaximumLength, "5"}
                       }
                },

                  new GridField
                {
                    field = "RecordPrintedString", 
                    fieldToQuery="RecordPrinted",
                    title = ReceiptHeaderResx.RecordPrinted,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<RecordPrinted>()
                },

                  new GridField
                {
                    field = "PostSequenceNumber",
                    title =ReceiptHeaderResx.PostSequenceNumber,
                   attributes = FinderConstant.CssClassGridColumn12NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn12NumRightAlign,
                    dataType = FinderConstant.DataTypeInt,
                    customAttributes =
                        new Dictionary<string, string>
                        {
                            {"class", FinderConstant.CssClassInputNumberRightAlign},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            {FinderConstant.CustomAttributeMaximumLength, "10"}
                        }
                },

                 new GridField
                {
                    field = "OptionalFields",
                    title = CommonResx.OptionalFields,
                   attributes = FinderConstant.CssClassGridColumn12NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn12NumRightAlign,
                    dataType = FinderConstant.DataTypeInt,
                    customAttributes =
                        new Dictionary<string, string>
                        {
                            {"class", FinderConstant.CssClassInputNumberRightAlign},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.Numeric},
                            {FinderConstant.CustomAttributeMaximumLength, "10"}
                        }
                },
                   new GridField
                {
                    field = "ProcessCommandString", 
                    fieldToQuery="ProcessCommand",
                    title = ReceiptHeaderResx.ProcessCommand,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<ProcessCommand>()
                },

                  new GridField
                {
                    field = "VendorName",
                    title = ReceiptHeaderResx.VendorName, 
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "60"},
						}
                },

                 new GridField
                {
                    field = "EnteredBy",
                    title = ReceiptHeaderResx.EnteredBy,
                    attributes = FinderConstant.CssClassGridColumn13,
                    headerAttributes = FinderConstant.CssClassGridColumn13,
                    dataType = "string",
                    customAttributes = 
						new Dictionary<string, string>
						{
							{FinderConstant.CustomAttributeMaximumLength, "8"},
							{"class", FinderConstant.CssClassTxtUpper},
							{FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
						}
                },
                 new GridField
                {
                    field = "PostingDate",
                    title = ReceiptHeaderResx.PostingDate,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeDate,
                    template = Utilities.GetGridTemplate("PostingDate"), 
                }, 
               
            };

            return column.AsEnumerable();
        }

        /// <summary>
        /// Get all the default columns to be displayed
        /// </summary>
        /// <returns>Returns the list of all Default Columns</returns>
        public override List<string> GetDefaultColumns()
        {
            return new List<string>
            {
                "ReceiptNumber", "Description", "ReceiptDate", "FiscalYear", "FiscalPeriodString", "PurchaseOrderNumber", "Reference", "ReceiptTypeString", "RateOperationString","VendorNumber", "ReceiptCurrency", "ExchangeRate",
                "RateType", "RateDate", "RateOverrideString", "AdditionalCost","OrigAdditionalCostFunc","OrigAdditionalCostSource","AdditionalCostCurrency","TotalExtendedCostFunctional","TotalExtendedCostSource",
                "NumberOfDetailswithCost","RequireLabelsString","AddlCostAllocationTypeString","OriginalTotalCostSource","OriginalTotalCostFunctional","AdditionalCostFunctional","TotalCostReceiptAdditional",
                "ReceiptCurrencyDecimals","VendorShortName","VendorExistsString","RecordStatusString"
            };
        }
    }
}