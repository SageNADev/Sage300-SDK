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
using Microsoft.Practices.Unity;
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;
using System.Globalization;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SourceJournalProfile Internal Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public class SourceJournalProfileControllerInternal<T> : InternalControllerBase<ISourceJournalProfileService<T>>
        where T : SourceJournalProfile, new()
    {
        #region Private variables

        /// <summary>
        /// Source Journal Name
        /// </summary>
        private string _sourceJournalName = string.Empty;

        /// <summary>
        /// _getSourceCodePaged
        /// </summary>
        private Func<int, int, EnumerableResponse<SourceCode>> _getSourceCodePaged;

        /// <summary>
        /// GridKey
        /// </summary>
        private const string GridKey = "SerialNumber";

        /// <summary>
        /// Source Code Cache Key
        /// </summary>
        private readonly string _sourceJournalProfileCacheKey;

        /// <summary>
        /// Key Source Code GridFilter
        /// </summary>
        private readonly Func<SourceCode, long> _keySourceCodeGridFilter;

        /// <summary>
        /// SourceJournalProfileCacheKey
        /// </summary>
        private const string SourceJournalProfileCacheKey = "sourceJournalProfileSourceCode";

        #endregion

        #region Constructor

        /// <summary>
        /// New instance of <see cref="SourceJournalProfileControllerInternal{T}"/>
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileControllerInternal(Context context)
            : base(context)
        {
            _sourceJournalProfileCacheKey = CreateSessionKey<T>(SourceJournalProfileCacheKey);
            _keySourceCodeGridFilter = source => source.SerialNumber;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Create a SourceJournalProfile
        /// </summary>
        /// <returns>JSON object for SourceJournalProfile</returns>
        internal SourceJournalProfileViewModel<T> Create()
        {
            SessionHelper.Remove(_sourceJournalProfileCacheKey);

            return new SourceJournalProfileViewModel<T>
            {
                Data = new T(),
                UserMessage = new UserMessage { IsSuccess = true }
            };
        }

        /// <summary>
        /// Get a SourceJournalProfile
        /// </summary>
        /// <param name="id">Id for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        internal SourceJournalProfileViewModel<T> Get(string id)
        {
            var data = Service.GetById(id);
            data.SourceCodeList=new EnumerableResponse<SourceCode>();

            return new SourceJournalProfileViewModel<T>
            {
                Data = data,
                UserMessage = new UserMessage { IsSuccess = true }
            };
        }

        /// <summary>
        /// Clone Source Journal
        /// </summary>
        /// <param name="insertIndex">insertIndex</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="model">model</param>
        /// <returns>SourceJournalProfileViewModel</returns>
        internal SourceJournalProfileViewModel<T> CloneSourceJournal(int insertIndex, int pageNumber, int pageSize,
            T model)
        {
            //If Source Journal Exist get the data
            if (model.Exist)
            {
                //Remove previous Source Journal data from Cache
                SessionHelper.Remove(_sourceJournalProfileCacheKey);

                _sourceJournalName = model.SourceJournalName;

                var sourceJournalData = GetSourcePaged(-1, -1);

                if (sourceJournalData.Items.Any())
                {
                    SessionHelper.Set(_sourceJournalProfileCacheKey, sourceJournalData.Items.ToList());
                }

                return GetSourceJournal(insertIndex, pageNumber, pageSize, model);
            }

            var cacheData = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey);

            if (cacheData != null)
            {
                //Merge Latest changed data to cache
                MergeChangeItemToCache(model.SourceCodeList, 0, 0, 0, GridKey, _sourceJournalProfileCacheKey, _getSourceCodePaged,
                    _keySourceCodeGridFilter, null);

                //Get Data from cache to clone
                var cachedData = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey);
                List<SourceCode> cloneData;

                //remove deleted records from cache
                if (cachedData.Any())
                {
                    cloneData = cachedData.ToList().Where(s => s.IsDeleted != true).ToList();

                    var displayIndex = 1;
                    //Update Clone data - IsNewLine and HasChanged to True
                    foreach (var item in cloneData)
                    {
                        item.IsNewLine = true;
                        item.HasChanged = true;
                        item.DisplayIndex = displayIndex++;
                    }

                    //Set Clone Data to Cache
                    SessionHelper.Set(_sourceJournalProfileCacheKey, cloneData);
                }

                return GetSourceJournal(insertIndex, pageNumber, pageSize, model);
            }

            //New Record return new Enumerable Response
            var enumerableResponse = new EnumerableResponse<SourceCode>
              {
                  Items = new List<SourceCode>(),
                  TotalResultsCount = 0,
                  CachedListCount = 0
              };

            var viewmodel = new SourceJournalProfileViewModel<T>
            {
                Data = new T { SourceJournalName = model.SourceJournalName, SourceCodeList = enumerableResponse },
                UserMessage = new UserMessage(enumerableResponse),
                UserAccess = Service.GetAccessRights()
            };

            return viewmodel;
        }

        /// <summary>
        /// GetSourceJournal
        /// </summary>
        /// <param name="insertIndex">insertIndex</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="model">model</param>
        /// <returns>SourceJournalProfileViewModel</returns>
        internal SourceJournalProfileViewModel<T> GetSourceJournal(int insertIndex, int pageNumber, int pageSize, T model)
        {
            var viewmodel = new SourceJournalProfileViewModel<T>();

            if (string.IsNullOrEmpty(model.SourceJournalName))
            {
                return new SourceJournalProfileViewModel<T>();
            }

            _sourceJournalName = model.SourceJournalName;
            _getSourceCodePaged = GetSourcePaged;

            var newRecord = new SourceCode();

            if (insertIndex > 0)
            {
                newRecord = GetSourceCodeLine(model);
            }

            var mergedList = MergeChangeItemToCache(model.SourceCodeList, pageNumber, pageSize,
                insertIndex, GridKey, _sourceJournalProfileCacheKey, _getSourceCodePaged, _keySourceCodeGridFilter, newRecord);

            viewmodel.Data = new T
               {
                   SourceJournalName = model.SourceJournalName,
                   SourceCodeList = mergedList
               };

            viewmodel.UserMessage = new UserMessage(mergedList);
            viewmodel.UserAccess = Service.GetAccessRights();
            return viewmodel;
        }


        /// <summary>
        /// Deletes a Source Journal Profile 
        /// </summary>
        /// <param name="id">SourceJournalName</param>
        /// <returns>Json object for Source Journal Profile </returns>
        internal SourceJournalProfileViewModel<T> Delete(string id)
        {
            var cachedItems = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey);

            if (cachedItems == null)
            {
                _sourceJournalName = id;

                var data = GetSourcePaged(-1, -1);

                if (data.Items.Any())
                {
                    SessionHelper.Set(_sourceJournalProfileCacheKey, data.Items.ToList());
                }
            }

            var sourceJournals = Service.Delete(id);
            sourceJournals.SourceJournalName = id;

            cachedItems = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey);

            //Update deleted Clone data - IsNewLine and HasChanged to True
            if (cachedItems != null && cachedItems.Any())
            {
                foreach (var item in cachedItems)
                {
                    item.IsNewLine = true;
                    item.HasChanged = true;
                }

                SessionHelper.Set(_sourceJournalProfileCacheKey, cachedItems);
            }

            return new SourceJournalProfileViewModel<T>
            {
                Data = sourceJournals,
                UserMessage = new UserMessage(sourceJournals, string.Format(CommonResx.DeleteSuccessMessage,
                    SourceJournalProfileResx.SourceJournalProfile, sourceJournals.SourceJournalName))
            };
        }

        /// <summary>
        /// Saves a Source Journal Profile
        /// </summary>
        /// <param name="model">Source Journal Profile Model</param>
        /// <returns>Json object for Source Code </returns>
        internal SourceJournalProfileViewModel<T> Save(T model)
        {
            bool hasDataChanged = false;
            var recordExist = model.Exist;

            if (model.SourceCodeList.Items != null)
            {
                var items = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey);

                if (items != null)
                {
                    MergeChangeItemToCache(model.SourceCodeList, 0, 0, 0, GridKey, _sourceJournalProfileCacheKey, _getSourceCodePaged,
                        _keySourceCodeGridFilter, null);
                    items = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey);
                    if (items.Count > 10)
                        items = items.OrderBy(o => o.DisplayIndex).ToList();
                }
                else
                {
                    if (model.SourceCodeList.Items.Any())
                    {
                        items = model.SourceCodeList.Items.ToList();
                    }
                }

                if (items != null && items.Any())
                {
                    items.RemoveAll(c => (c.IsDeleted && c.IsNewLine));

                    var emptyItems = items.Where(e => (string.IsNullOrEmpty(e.Source))).ToList();
                    var deletedItems = items.Where(d => d.IsDeleted).ToList();
                    items.RemoveAll(e => (string.IsNullOrEmpty(e.Source)));
                    items.RemoveAll(c => (c.IsDeleted));
                    items.AddRange(deletedItems);
                    items.AddRange(emptyItems);

                    if (items.Any(c => c.HasChanged) || items.Any(c => c.IsNewLine) || items.Any(c => c.IsDeleted))
                    {
                        hasDataChanged = true;
                    }
                }

                if (items == null)
                {
                    items=new List<SourceCode>();
                }

                model.SourceCodeList = new EnumerableResponse<SourceCode> { Items = items };
            }

            T sourceJournalProfile;
            SessionHelper.Remove(_sourceJournalProfileCacheKey);

            //For existing record check data change then save data.
            if (recordExist)
            {
                sourceJournalProfile = hasDataChanged ? Service.Save(model) : model;
            }
            else
            {
                sourceJournalProfile = Service.Save(model);
            }

            var cloneData = sourceJournalProfile.SourceCodeList.Items.Where(d => d.IsDeleted != true).ToList();
            cloneData = cloneData.Where(s => !string.IsNullOrEmpty(s.Source)).ToList();
            var displayIndex = 1;

            //Update Clone data - IsNewLine and HasChanged to True
            foreach (var item in cloneData)
            {
                item.IsNewLine = false;
                item.HasChanged = false;
                item.DisplayIndex = displayIndex++;
            }

            //Set new Source Journal Profile data to cache for clone
            SessionHelper.Set(_sourceJournalProfileCacheKey, cloneData);

            sourceJournalProfile.SourceCodeList = new EnumerableResponse<SourceCode>();

            return new SourceJournalProfileViewModel<T>
            {
                Data = sourceJournalProfile,
                UserMessage = recordExist ? new UserMessage(sourceJournalProfile, CommonResx.SaveSuccessMessage) :
                new UserMessage(sourceJournalProfile, string.Format(CommonResx.AddSuccessMessage, SourceJournalProfileResx.SourceJournalProfile,
                    sourceJournalProfile.SourceJournalName))
            };
        }

        /// <summary>
        /// Get Source Code By Id
        /// </summary>
        /// <param name="sourceLedger">sourceLedger</param>
        /// <param name="sourceType">sourceType</param>
        /// <returns>Json object for Source Journal Profile </returns>
        internal SourceJournalProfileViewModel<T> GetSourceCodeById(string sourceLedger, string sourceType)
        {
            var sourceCode = new SourceCode();

            var valid = Service.CheckValidSourceCode(sourceLedger.ToUpper(CultureInfo.InvariantCulture), sourceType.ToUpper(CultureInfo.InvariantCulture));

            if (valid)
            {
                var sourceCodeService = Context.Container.Resolve<ISourceCodeService<SourceCode>>(new ParameterOverride("context", Context));
                sourceCode = sourceCodeService.GetByIds(sourceLedger, sourceType);
            }

            return new SourceJournalProfileViewModel<T>
            {
                SourceCode = sourceCode,
                UserMessage = new UserMessage { IsSuccess = true }
            };
        }

        /// <summary>
        /// Checks if Exists the specified Source Journal Name.
        /// </summary>
        /// <param name="sourceCode">Source Journal Name</param>
        /// <param name="id">SourceJournalName</param>
        /// <returns>True if Source Journal Name exists.</returns>
        internal SourceJournalProfileViewModel<T> IsExist(string sourceCode, string id)
        {
            var exist = false;

            var model = new T
            {
                SourceJournalName = id
            };

            if (SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey) != null)
            {
                exist = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey)
                    .Any(selectionCriteria => (selectionCriteria.Source == sourceCode && selectionCriteria.IsDeleted != true));
            }
            else
            {
                var viewModel = GetSourceJournal(-1, -1, -1, model);

                if (viewModel != null && viewModel.Data.SourceCodeList != null && viewModel.Data.SourceCodeList.Items.Any())
                {
                    SessionHelper.Set(_sourceJournalProfileCacheKey, viewModel.Data.SourceCodeList.Items.ToList());

                    exist = SessionHelper.Get<List<SourceCode>>(_sourceJournalProfileCacheKey)
                    .Any(selectioncriteria => (selectioncriteria.Source == sourceCode && selectioncriteria.IsDeleted != true));
                }
            }

            UserMessage userMessage;
            var errorList = new List<EntityError>();

            if (exist)
            {
                //Build custom error message.

                var entityError = new EntityError
                {
                    Message = string.Format(CommonResx.DuplicateMessage, SourceJournalProfileResx.SourceJournalProfile, sourceCode),
                    Priority = Priority.Error
                };

                errorList.Add(entityError);
                userMessage = new UserMessage { IsSuccess = false, Errors = errorList };
            }
            else
            {
                userMessage = new UserMessage { IsSuccess = true };
            }

            return new SourceJournalProfileViewModel<T> { IsSourceCodeExists = exist, UserMessage = userMessage };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// GetSourcePaged
        /// </summary>
        /// <param name="pageNumber">Current Page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of SourceCode</returns>
        private EnumerableResponse<SourceCode> GetSourcePaged(int pageNumber, int pageSize)
        {
            return Service.GetSourceJournal(_sourceJournalName, pageNumber, pageSize);
        }

        /// <summary>
        /// GetSourceCodeLine
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>AccountAllocationInstructions Line</returns>
        private static SourceCode GetSourceCodeLine(T model)
        {
            return new SourceCode
            {
                IsNewLine = true,
                SerialNumber = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond,
                DisplayIndex = model.SourceCodeList.TotalResultsCount + 1
            };
        }

        #endregion

    }
}