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
using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.Web.Areas.TU.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using Sage.CA.SBS.ERP.Sage300.IC.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.IC.Models;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SegmentCodes Internal Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SegmentCodes"/></typeparam>
    public class SegmentCodesControllerInternal<T> : BaseExportImportControllerInternal<T, ISegmentCodesService<T>>
        where T : SegmentCodes, new()
    {
        #region Private variables
        
        private readonly string _segmentCodesCacheKey;
        private readonly Func<T, long> _keySegmentFilter;

        private Func<int, int, EnumerableResponse<T>> _getSegments;

        private const string GridKey = "SerialNumber";

        private Expression<Func<T, bool>> _filterExpression;

        #endregion

        #region Constructor

        /// <summary>
        /// New instance of <see cref="SegmentCodesControllerInternal{T}"/>
        /// </summary>
        /// <param name="context">Context</param>
        public SegmentCodesControllerInternal(Context context)
            : base(context)
        {
            _segmentCodesCacheKey = CreateSessionKey<T>("TUSegmentCodes");
            _keySegmentFilter = segment => segment.SerialNumber;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Create a SegmentCodes
        /// </summary>
        /// <returns>JSON object for SegmentCodes</returns>
        internal SegmentCodesViewModel<T> Create()
        {
            var viewModel = GetViewModel(new T(), null);
            viewModel.UserAccess = GetAccessRights();
            viewModel.SegmentCodes.Items = new List<T>();
            return viewModel;
        }

        /// <summary>
        /// Get Paged Segment Codes saving in cache.
        /// </summary>
        /// <param name="insertIndex"></param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="finderOptions">The segment number.</param>
        /// <param name="model"></param>
        /// <param name="isCacheRemovable">Set true while dropdown segment name change</param>
        /// <returns>EnumerableResponse</returns>
        internal SegmentCodesViewModel<T> Get(SegmentCodesViewModel<T> model, int insertIndex, int pageNumber, int pageSize,
            IList<IList<Filter>> finderOptions, bool isCacheRemovable)
        {
            // Remove the segment code details from cache while changing the segment name from drop down.
            if (isCacheRemovable)
            {
                SessionHelper.Remove(_segmentCodesCacheKey);
            }

            return GetSegments(model, insertIndex, pageNumber, pageSize, finderOptions);
        }

        /// <summary>
        /// Check whether segmentcode can be deletable or not.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="deletableSegmentCodes"></param>
        /// <returns></returns>
        internal SegmentCodesViewModel<T> AreSegmentCodesDeletable(IEnumerable<T> model, ref List<string> deletableSegmentCodes)
        {
            // Loop out the model items and add the first items list till the record is found an existing one
            // This is the scenario in which the user adds multiple records and try deleting the same along with the existing record
            // which is used and cannot be deleted. In this case, the first set of newly added records should be removed from the grid
            foreach (var segmentCode in model)
            {
                if (segmentCode.IsNewLine)
                    deletableSegmentCodes.Add(segmentCode.SegmentCode);
                else
                    break;
            }

            // Then call the service to find whether the segment code records can be deleted
            var isSegmentUsed = Service.AreSegmentCodesDeletable(model, ref deletableSegmentCodes);

            return new SegmentCodesViewModel<T> { IsSegmentCodeUsed = isSegmentUsed, DeletedSegmentCodes = deletableSegmentCodes };
        }

        /// <summary>
        /// Checks for the segment code duplication
        /// </summary>
        /// <param name="model"></param>
        /// <param name="segmentNumber">The segment number.</param>
        /// <param name="segmentCode">The segment code.</param>
        /// <returns>SegmentCodeViewModel</returns>
        internal SegmentCodesViewModel<T> Exists(EnumerableResponse<T> model, string segmentNumber, string segmentCode)
        {
            var isRecordValid = Service.Exists(segmentNumber, segmentCode);
            EnumerableResponse<T> cachedSegmentCodes = GetDataForSave(model, _segmentCodesCacheKey, GridKey, _keySegmentFilter);

            var items = cachedSegmentCodes.Items;

            if (!isRecordValid && items.Any()) 
            {
                isRecordValid = items.Where(r => r.SegmentCode == segmentCode && r.IsDeleted).Count() > 0 ;
            }
            if (isRecordValid && items.Any())
            {
                isRecordValid = items.Where(r => r.SegmentCode == segmentCode && r.IsNewLine).Count() == 0;
            }
            return new SegmentCodesViewModel<T>
            {
                IsValidSegmentCode = isRecordValid,
                ErrorMessage = CommonResx.DuplicateMessage
            };
        }

        /// <summary>
        /// Save Segment Codes
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>SegmentCodeViewModel.</returns>
        internal SegmentCodesViewModel<T> Save(EnumerableResponse<T> model)
        {
            model = GetDataForSave(model, _segmentCodesCacheKey, GridKey, _keySegmentFilter);

            SegmentCodesViewModel<T> viewModel = null;

            model = Service.Save(model);

            SessionHelper.Remove(_segmentCodesCacheKey);
            
            foreach (var segment in model.Items)
            {
                segment.HasChanged = false;
                segment.IsDeleted = false;
                segment.IsNewLine = false;
            }
            
            var message = CommonResx.SaveSuccessMessage;

            viewModel = new SegmentCodesViewModel<T>
            {
                SegmentCodes = model,
                UserMessage = new UserMessage(model, message),
                TotalResultsCount = model.TotalResultsCount
            };

            return viewModel;
        }

        /// <summary>
        /// Get a SegmentCodes
        /// </summary>
        /// <param name="id">Id for SegmentCodes</param>
        /// <returns>JSON object for SegmentCodes</returns>
        internal SegmentCodesViewModel<T> GetById(string id)
        {
            var viewModel = new SegmentCodesViewModel<T>();

            // Remove cached data while page refreshing
            SessionHelper.Remove(_segmentCodesCacheKey);
            viewModel.UserAccess = GetAccessRights();
            var itemSegmentService =
                Context.Container.Resolve<IItemSegmentService<ItemSegment>>(new ParameterOverride("context", Context));

            var segmentname = itemSegmentService.Get();

            if (segmentname.Items != null && segmentname.Items.Any())
            {
                foreach (var items in segmentname.Items)
                {
                    viewModel.Segments.Add(new SegmentName
                    {
                        Text = items.Description,
                        Value = items.SegmentNumber,
                        SegmentLength = items.Length,
                        SegmentNumber = items.SegmentNumber
                    });
                }

                viewModel.SegmentNameLength = Convert.ToInt32(segmentname.Items.First().Length);
                viewModel.SegmentNumber = segmentname.Items.First().SegmentNumber;

                viewModel.UserMessage = new UserMessage(segmentname);
                viewModel.TotalResultsCount = viewModel.TotalResultsCount;
                viewModel.Data = viewModel.Data ?? new T();
            }
            else
            {
                var errorList = new List<EntityError>();

                var entityError = new EntityError { Message = SegmentCodesResx.NoSegments, Priority = Priority.Error };
                errorList.Add(entityError);

                viewModel.UserMessage = new UserMessage { IsSuccess = false, Errors = errorList };
            }

            return viewModel;

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generic routine to return a view model for SegmentCodes
        /// </summary>
        /// <param name="model">Model for SegmentCodes</param>
        /// <param name="userMessage">User Message for SegmentCodes</param>
        /// <returns>View Model for SegmentCodes</returns>
        private static SegmentCodesViewModel<T> GetViewModel(T model, UserMessage userMessage)
        {
            return new SegmentCodesViewModel<T>
            {
                Data = model,
                UserMessage = userMessage
            };
        }

        /// <summary>
        /// Get Segment Code Details 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="insertIndex"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="finderOptions"></param>
        /// <returns></returns>
        private SegmentCodesViewModel<T> GetSegments(SegmentCodesViewModel<T> model, int insertIndex, int pageNumber, int pageSize, IList<IList<Filter>> finderOptions)
        {
            model = model ?? new SegmentCodesViewModel<T>();

            if (finderOptions != null)
            {
                _filterExpression = ExpressionBuilder<T>.CreateExpression(finderOptions);
            }
            // If finder option is null then set filter from model
            else
            {
                var items = model.SegmentCodes.Items;
                if (items != null && items.Any())
                {
                    var firstOrDefault = items.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        _filterExpression = segment => segment.SegmentNumber == firstOrDefault.SegmentNumber;
                    }
                }
                else
                {
                    model.SegmentCodes = new EnumerableResponse<T> { Items = new List<T>() };
                }
            }

            if (model.SegmentCodes != null && model.SegmentCodes.Items.Any())
            {
                // Check for valid segment length
                var invalidSegmentCode = model.SegmentCodes.Items.Where(s =>
                    ((s.SegmentCode == null) || (s.SegmentCode.Length != model.SegmentNameLength)) && (!s.IsDeleted && s.IsNewLine)).Select(segment => segment);
                // If there any invalid segment code, then throw the exception and stop the pagination.
                if (invalidSegmentCode.Any())
                {
                    var errors = new List<EntityError>
                    {
                        new EntityError { Message = string.Format(SegmentCodesResx.InvalidLength,SegmentCodesResx.SegmentCode, model.SegmentNameLength) }
                    };
                    throw new BusinessException(string.Empty, null, errors);
                }
            }

            _getSegments = GetSegmentCodes;
            var newRecord = new T();

            // If insert index is great than zero then create new line, server side add new line
            if (insertIndex > 0 && model.SegmentCodes != null)
            {
                if (model.SegmentNumber == null && model.SegmentCodes.Items.Any())
                {
                    model.SegmentNumber = model.SegmentCodes.Items.FirstOrDefault().SegmentNumber;
                }                
                newRecord = NewSegmentCode(model.SegmentCodes.Items.Count(), model.SegmentNumber);
            }

            // Merge the model data with cached data
            var mergerdList = MergeChangeItemToCache(model.SegmentCodes, pageNumber, pageSize, insertIndex, GridKey, _segmentCodesCacheKey, _getSegments, _keySegmentFilter, newRecord);

            return new SegmentCodesViewModel<T>
            {
                SegmentCodes = mergerdList,
                UserMessage = new UserMessage(mergerdList),
            };
        }

        /// <summary>
        /// Get Segment Details for Grid
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private EnumerableResponse<T> GetSegmentCodes(int pageNumber, int pageSize)
        {
            // Get the segment code from service
            var segmentCodes = Service.Get(pageNumber, pageSize, _filterExpression);

            var firstOrDefault = segmentCodes.Items.FirstOrDefault();

            if (firstOrDefault != null && (segmentCodes.Items.Any() && firstOrDefault.SegmentCode == null))
            {
                segmentCodes = new EnumerableResponse<T> { Items = new List<T>() };
            }

            var dataIndex = (pageSize > 0 && pageNumber > 0) ? (pageSize * pageNumber) + 1 : 1;

            foreach (var segmentCode in segmentCodes.Items)
            {
                segmentCode.SerialNumber = dataIndex;
                segmentCode.DisplayIndex = dataIndex;
                dataIndex++;
            }

            return segmentCodes;
        }

        /// <summary>
        /// New segment line for grid
        /// </summary>
        /// <param name="displayIndex">The display index at which the new record should be placed</param>
        /// <param name="segmentNumber">Segment number to which the Segment Code should be mapped to</param>
        /// <returns>Returns of type SegmentCode</returns>
        private static T NewSegmentCode(int displayIndex, string segmentNumber)
        {
            var segment = new T
            {
                IsNewLine = true,
                IsDeleted = false,
                SerialNumber = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond,
                DisplayIndex = displayIndex,
                SegmentNumber = segmentNumber
            };

            return segment;
        }
        #endregion

	}
}