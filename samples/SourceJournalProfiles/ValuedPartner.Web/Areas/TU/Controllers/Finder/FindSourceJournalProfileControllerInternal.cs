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

#region Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Finder;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers.Finder
{
    /// <summary>
    /// Finder class for SourceJournalProfile
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public class FindSourceJournalProfileControllerInternal<T> : BaseFindControllerInternal<T, ISourceJournalProfileService<T>>, IFinder
        where T : SourceJournalProfile, new()
    {
        #region Private variables

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfile
        /// </summary>
        /// <param name="context">Context</param>
        public FindSourceJournalProfileControllerInternal(Context context)
            : base(context)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get first or default SourceJournalProfile
        /// </summary>
        /// <param name="id">Id for SourceJournalProfile</param>
        /// <returns>Get first or default SourceJournalProfile</returns>
        public virtual ModelBase Get(string id)
        {
            Expression<Func<T, bool>> filter = param => param.SourceJournalName == id;
            Service.IsReadOnly = true;
            var model = Service.FirstOrDefault(filter);
            Service.IsReadOnly = false;
			return model;
        }

        /// <summary>
        /// Get the default columns
        /// </summary>
        /// <returns>Default columns</returns>
        public override List<string> GetDefaultColumns()
        {
            return new List<string> { "SourceJournalName" };
        }

        /// <summary>
        /// Get all columns
        /// </summary>
        /// <returns>All columns</returns>
        public override IEnumerable<ModelBase> GetAllColumns()
        {

            var columns = new List<GridField>
            {
                new GridField
                {
                    field = "SourceJournalName",
                    title = SourceJournalProfileResx.SourceJournalName,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "60"}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID01",
                    title = SourceJournalProfileResx.SourceCodeID01,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.AttributeClass, FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {FinderConstant.CustomAtrributeFormatTextBox, "alpha"}
                        }
                },
                new GridField
                {
                    field = "SourceType01",
                    title = SourceJournalProfileResx.SourceType01,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID02",
                    title = SourceJournalProfileResx.SourceCodeID02,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.AttributeClass, "txt-upper"},	
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {FinderConstant.CustomAtrributeFormatTextBox, "alpha"}
                        }
                },
                new GridField
                {
                    field = "SourceType02",
                    title = SourceJournalProfileResx.SourceType02,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID03",
                    title = SourceJournalProfileResx.SourceCodeID03,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.AttributeClass, "txt-upper"},					
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {FinderConstant.CustomAtrributeFormatTextBox, "alpha"}
                        }
                },
                new GridField
                {
                    field = "SourceType03",
                    title = SourceJournalProfileResx.SourceType03,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID04",
                    title = SourceJournalProfileResx.SourceCodeID04,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType04",
                    title = SourceJournalProfileResx.SourceType04,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID05",
                    title = SourceJournalProfileResx.SourceCodeID05,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType05",
                    title = SourceJournalProfileResx.SourceType05,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID06",
                    title = SourceJournalProfileResx.SourceCodeID06,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType06",
                    title = SourceJournalProfileResx.SourceType06,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID07",
                    title = SourceJournalProfileResx.SourceCodeID07,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType07",
                    title = SourceJournalProfileResx.SourceType07,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID08",
                    title = SourceJournalProfileResx.SourceCodeID08,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType08",
                    title = SourceJournalProfileResx.SourceType08,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID09",
                    title = SourceJournalProfileResx.SourceCodeID09,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType09",
                    title = SourceJournalProfileResx.SourceType09,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID10",
                    title = SourceJournalProfileResx.SourceCodeID10,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType10",
                    title = SourceJournalProfileResx.SourceType10,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID11",
                    title = SourceJournalProfileResx.SourceCodeID11,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType11",
                    title = SourceJournalProfileResx.SourceType11,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID12",
                    title = SourceJournalProfileResx.SourceCodeID12,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType12",
                    title = SourceJournalProfileResx.SourceType12,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID13",
                    title = SourceJournalProfileResx.SourceCodeID13,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType13",
                    title = SourceJournalProfileResx.SourceType13,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID14",
                    title = SourceJournalProfileResx.SourceCodeID14,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType14",
                    title = SourceJournalProfileResx.SourceType14,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID15",
                    title = SourceJournalProfileResx.SourceCodeID15,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType15",
                    title = SourceJournalProfileResx.SourceType15,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID16",
                    title = SourceJournalProfileResx.SourceCodeID16,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType16",
                    title = SourceJournalProfileResx.SourceType16,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID17",
                    title = SourceJournalProfileResx.SourceCodeID17,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType17",
                    title = SourceJournalProfileResx.SourceType17,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID18",
                    title = SourceJournalProfileResx.SourceCodeID18,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType18",
                    title = SourceJournalProfileResx.SourceType18,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID19",
                    title = SourceJournalProfileResx.SourceCodeID19,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType19",
                    title = SourceJournalProfileResx.SourceType19,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID20",
                    title = SourceJournalProfileResx.SourceCodeID20,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType20",
                    title = SourceJournalProfileResx.SourceType20,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID21",
                    title = SourceJournalProfileResx.SourceCodeID21,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType21",
                    title = SourceJournalProfileResx.SourceType21,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID22",
                    title = SourceJournalProfileResx.SourceCodeID22,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType22",
                    title = SourceJournalProfileResx.SourceType22,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID23",
                    title = SourceJournalProfileResx.SourceCodeID23,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType23",
                    title = SourceJournalProfileResx.SourceType23,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID24",
                    title = SourceJournalProfileResx.SourceCodeID24,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType24",
                    title = SourceJournalProfileResx.SourceType24,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID25",
                    title = SourceJournalProfileResx.SourceCodeID25,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType25",
                    title = SourceJournalProfileResx.SourceType25,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID26",
                    title = SourceJournalProfileResx.SourceCodeID26,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType26",
                    title = SourceJournalProfileResx.SourceType26,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID27",
                    title = SourceJournalProfileResx.SourceCodeID27,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType27",
                    title = SourceJournalProfileResx.SourceType27,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID28",
                    title = SourceJournalProfileResx.SourceCodeID28,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType28",
                    title = SourceJournalProfileResx.SourceType28,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID29",
                    title = SourceJournalProfileResx.SourceCodeID29,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType29",
                    title = SourceJournalProfileResx.SourceType29,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID30",
                    title = SourceJournalProfileResx.SourceCodeID30,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType30",
                    title = SourceJournalProfileResx.SourceType30,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID31",
                    title = SourceJournalProfileResx.SourceCodeID31,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType31",
                    title = SourceJournalProfileResx.SourceType31,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID32",
                    title = SourceJournalProfileResx.SourceCodeID32,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType32",
                    title = SourceJournalProfileResx.SourceType32,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID33",
                    title = SourceJournalProfileResx.SourceCodeID33,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType33",
                    title = SourceJournalProfileResx.SourceType33,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID34",
                    title = SourceJournalProfileResx.SourceCodeID34,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType34",
                    title = SourceJournalProfileResx.SourceType34,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID35",
                    title = SourceJournalProfileResx.SourceCodeID35,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType35",
                    title = SourceJournalProfileResx.SourceType35,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID36",
                    title = SourceJournalProfileResx.SourceCodeID36,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType36",
                    title = SourceJournalProfileResx.SourceType36,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID37",
                    title = SourceJournalProfileResx.SourceCodeID37,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType37",
                    title = SourceJournalProfileResx.SourceType37,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID38",
                    title = SourceJournalProfileResx.SourceCodeID38,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType38",
                    title = SourceJournalProfileResx.SourceType38,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID39",
                    title = SourceJournalProfileResx.SourceCodeID39,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType39",
                    title = SourceJournalProfileResx.SourceType39,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID40",
                    title = SourceJournalProfileResx.SourceCodeID40,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType40",
                    title = SourceJournalProfileResx.SourceType40,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID41",
                    title = SourceJournalProfileResx.SourceCodeID41,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType41",
                    title = SourceJournalProfileResx.SourceType41,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID42",
                    title = SourceJournalProfileResx.SourceCodeID42,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType42",
                    title = SourceJournalProfileResx.SourceType42,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID43",
                    title = SourceJournalProfileResx.SourceCodeID43,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType43",
                    title = SourceJournalProfileResx.SourceType43,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID44",
                    title = SourceJournalProfileResx.SourceCodeID44,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType44",
                    title = SourceJournalProfileResx.SourceType44,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID45",
                    title = SourceJournalProfileResx.SourceCodeID45,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType45",
                    title = SourceJournalProfileResx.SourceType45,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID46",
                    title = SourceJournalProfileResx.SourceCodeID46,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType46",
                    title = SourceJournalProfileResx.SourceType46,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID47",
                    title = SourceJournalProfileResx.SourceCodeID47,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType47",
                    title = SourceJournalProfileResx.SourceType47,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID48",
                    title = SourceJournalProfileResx.SourceCodeID48,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType48",
                    title = SourceJournalProfileResx.SourceType48,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID49",
                    title = SourceJournalProfileResx.SourceCodeID49,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType49",
                    title = SourceJournalProfileResx.SourceType49,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "SourceCodeID50",
                    title = SourceJournalProfileResx.SourceCodeID50,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"}
                        }
                },
                new GridField
                {
                    field = "SourceType50",
                    title = SourceJournalProfileResx.SourceType50,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "2"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "RESERVEDFunctionalReportName",
                    title = SourceJournalProfileResx.RESERVEDFunctionalReportName,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "8"}
                        }
                },
                new GridField
                {
                    field = "RESERVEDSourceReportName",
                    title = SourceJournalProfileResx.RESERVEDSourceReportName,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "8"}
                        }
                }
            };

            return columns.AsEnumerable();
        }
        #endregion

    }
}