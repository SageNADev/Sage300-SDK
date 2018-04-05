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
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.Web.Areas.TU.Models;
using Sage.CA.SBS.ERP.Sage300.CS.Models;
using Sage.CA.SBS.ERP.Sage300.CS.Interfaces.Services;
using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.GL.Models;
using Sage.CA.SBS.ERP.Sage300.GL.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.CS.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers
{
    /// <summary>
    /// TaxAuthorities Internal Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="TaxAuthorities"/></typeparam>
    public class TaxAuthoritiesControllerInternal<T> : BaseExportImportControllerInternal<T, ITaxAuthoritiesService<T>>
        where T : TaxAuthorities, new()
    {
        #region Private variables

        #endregion

        #region Constructor

        /// <summary>
        /// New instance of <see cref="TaxAuthoritiesControllerInternal{T}"/>
        /// </summary>
        /// <param name="context">Context</param>
        public TaxAuthoritiesControllerInternal(Context context)
            : base(context)
        {
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Create a TaxAuthorities
        /// </summary>
        /// <returns>JSON object for TaxAuthorities</returns>
        internal TaxAuthoritiesViewModel<T> Create()
        {
            var viewModel = GetViewModel(new T(), null);
            viewModel.Data.RecoverableRate = 100;
            return viewModel;
        }

        /// <summary>
        /// Get a TaxAuthorities
        /// </summary>
        /// <param name="id">Id for TaxAuthorities</param>
        /// <returns>JSON object for TaxAuthorities</returns>
        internal TaxAuthoritiesViewModel<T> GetById(string id)
        {
            var data = Service.GetById(id);
            var userMessage = new UserMessage(data);
            var viewModel = GetViewModel(data, userMessage);
            return viewModel;
        }

        /// <summary>
        /// Add a TaxAuthorities
        /// </summary>
        /// <param name="model">Model for TaxAuthorities</param>
        /// <returns>JSON object for TaxAuthorities</returns>
        internal TaxAuthoritiesViewModel<T> Add(T model)
        {
            var data = Service.Add(model);

            var userMessage = new UserMessage(data,
                string.Format(CommonResx.AddSuccessMessage, TaxAuthoritiesResx.TaxAuthority, data.TaxAuthority));

            return GetViewModel(data, userMessage);
       }

        /// <summary>
        /// Update a TaxAuthorities
        /// </summary>
        /// <param name="model">Model for TaxAuthorities</param>
        /// <returns>JSON object for TaxAuthorities</returns>
        internal TaxAuthoritiesViewModel<T> Save(T model)
        {
            var data = Service.Save(model);
            var userMessage = new UserMessage(data, CommonResx.SaveSuccessMessage);

            return GetViewModel(data, userMessage);
        }

        /// <summary>
        /// Delete a TaxAuthorities
        /// </summary>
        /// <param name="id">Id for TaxAuthorities</param>
        /// <returns>JSON object for TaxAuthorities</returns>
        internal TaxAuthoritiesViewModel<T> Delete(string id)
        {
            Expression<Func<T, bool>> filter = param => param.TaxAuthority == id;
            var data = Service.Delete(filter);

            var userMessage = new UserMessage(data,
                string.Format(CommonResx.DeleteSuccessMessage, TaxAuthoritiesResx.TaxAuthority, data.TaxAuthority));

            return GetViewModel(data, userMessage);
        }

        /// <summary>
        /// Gets the GL account details. 
        /// </summary>
        /// <param name="id">Account Number</param>
        /// <returns>GL Account </returns>
        internal Account GetGlAccount(string id)
        {
            var accountService = Context.Container.Resolve<IAccountService<Account>>(new ParameterOverride("context", Context));
            Expression<Func<Account, bool>> filter = accountNo => accountNo.UnformattedAccount == id.ToUpper() || accountNo.AccountNumber == id.ToUpper();
            var account = accountService.Get(filter);
            if (account != null && account.Items != null && account.Items.Any())
            {
                return account.Items.First();
            }
            else
            {
                var error = GenerateAccountDescriptionEntityError(LockedFiscalPeriod.Error, CommonResx.RecordNotFoundMessage, id);
                throw new BusinessException(error.Message, null, new List<EntityError> { error });
            }

        }

        /// <summary>
        /// Get the currecy description. 
        /// </summary>
        /// <param name="id">currency code</param>
        /// <returns>currecy description </returns>
        internal string GetCurrencyDescription(string id)
        {
            var currencyService = Context.Container.Resolve<ICurrencyCodeService<CurrencyCode>>(new ParameterOverride("context", Context));
            Expression<Func<CurrencyCode, bool>> filter = currencyCodes => currencyCodes.CurrencyCodeId == id;
            var currency = currencyService.Get(filter);

            if (currency != null && currency.Items != null && currency.Items.Any())
            {
                var firstOrDefault = currency.Items.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    return firstOrDefault.Description;
                }
            }

            var errorMsg = string.Format(CommonResx.RecordNotFoundMessage, TaxAuthoritiesResx.TxRepCurr, id);
            var errorList = new List<EntityError>(); 
            var entityError = new EntityError { Message = errorMsg, Priority = Priority.Error };
            errorList.Add(entityError);
            throw new BusinessException(errorMsg, errorList);
        }


        #endregion

        #region Private methods

        /// <summary>
        /// Generic routine to return a view model for TaxAuthorities
        /// </summary>
        /// <param name="model">Model for TaxAuthorities</param>
        /// <param name="userMessage">User Message for TaxAuthorities</param>
        /// <returns>View Model for TaxAuthorities</returns>
        private TaxAuthoritiesViewModel<T> GetViewModel(T model, UserMessage userMessage)
        {
            var viewModel = new TaxAuthoritiesViewModel<T>
            {
                Data = model,
                UserMessage = userMessage
            };
            var profile = GetCompanyProfile();
            viewModel.UserAccess = GetAccessRights();
            viewModel.CompanyProfile = profile;
            if (model == null)
            {
                return viewModel;
            }

            viewModel.Data.IsMultiCurrency = profile.CompanyProfileOptions.IsMulticurrency;
            if (string.IsNullOrEmpty(viewModel.Data.TaxReportingCurrency))
            {
                viewModel.Data.TaxReportingCurrency = profile.CompanyProfileOptions.FunctionalCurrency;
            }
            var currency = GetCurrency(viewModel.Data.TaxReportingCurrency);
            viewModel.CurrencyDescription = currency.Description;
            viewModel.CurrencyDecimals = currency.DecimalPlacesString;

            if (!string.IsNullOrEmpty(viewModel.Data.ExpenseAccount))
            {
                viewModel.ExpenseAccountDescription = GetGlAccount(viewModel.Data.ExpenseAccount).Description;
            }
            if (!string.IsNullOrEmpty(viewModel.Data.RecoverableTaxAccount))
            {
                viewModel.RecoverableTaxAccountDescription = GetGlAccount(viewModel.Data.RecoverableTaxAccount).Description;
            }
            if (!string.IsNullOrEmpty(viewModel.Data.TaxLiabilityAccount))
            {
                viewModel.LiabilityAccountDescription = GetGlAccount(viewModel.Data.TaxLiabilityAccount).Description;
            }

            return viewModel;
        }

        /// <summary>
        /// Get Company profile
        /// </summary>
        /// <returns>CompanyProfile</returns>
        private CompanyProfile GetCompanyProfile()
        {
            var companyProfileService = Context.Container.Resolve<ICompanyProfileService<CompanyProfile>>(new ParameterOverride("context", Context));
            var companyProfile = companyProfileService.Get();
            
            return (companyProfile.Items != null && companyProfile.Items.Any() ? companyProfile.Items.First() : new CompanyProfile());
        }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <param name="id">The currency code.</param>
        /// <returns></returns>
        private CurrencyCode GetCurrency(string id)
        {
            var currencyService = Context.Container.Resolve<ICurrencyCodeService<CurrencyCode>>(new ParameterOverride("context", Context));
            Expression<Func<CurrencyCode, bool>> filter = currencyCodes => currencyCodes.CurrencyCodeId == id.ToUpper() || currencyCodes.CurrencyCodeId == id.ToUpper();
            var currency = currencyService.Get(filter);
            return (currency.Items.FirstOrDefault());
        }


        ///// <summary>
        ///// Get account formatted number and description
        ///// </summary>
        ///// <param name="id"> account code</param>
        ///// <param name="nonExistent"> account code not exist</param>
        ///// <param name="inactive"> account code inactive</param>
        ///// <returns>list of account number and description</returns>
        //private List<string> GetAccountDescription(string id, LockedFiscalPeriod nonExistent, LockedFiscalPeriod inactive)
        //{
        //    var accountService = Context.Container.Resolve<IAccountService<Account>>(new ParameterOverride("context", Context));
        //    var account = accountService.GetById(id);

        //    if (string.IsNullOrEmpty(account.AccountNumber)) // case of account id does not exist
        //    {
        //        var error = GenerateAccountDescriptionEntityError(nonExistent, CommonResx.RecordNotFoundMessage, id);

        //        if (error.Priority == Priority.Error || error.Priority == Priority.Warning)
        //        {
        //            throw new BusinessException(error.Message, null, new List<EntityError> { error });
        //        }
        //    }
        //    else if (account.Status == Sage.CA.SBS.ERP.Sage300.GL.Models.Enums.Status.Inactive) // case of account exist but inactive
        //    {
        //        var error = GenerateAccountDescriptionEntityError(inactive, CommonResx.InactiveErrorMessage, id);
        //        error.Tag = account.AccountNumber + "|" + account.Description;
        //        if (error.Priority == Priority.Error || error.Priority == Priority.Warning)
        //        {
        //            throw new BusinessException(error.Message, null, new List<EntityError> { error });
        //        }
        //    }

        //    //return id as passed in if non-existent account is allowed.
        //    if (string.IsNullOrEmpty(account.AccountNumber)) return new List<string>() { id, string.Empty };

        //    // valid case
        //    var description = account.Description ?? string.Empty;
        //    return new List<string>() { account.AccountNumber, description };
        //}

        private static EntityError GenerateAccountDescriptionEntityError(LockedFiscalPeriod period, string messageTemplate, string id)
        {
            var entityError = new EntityError { Message = string.Format(messageTemplate, "Account", id.ToUpper()) };

            if (period == LockedFiscalPeriod.Warning)
            {
                entityError.Priority = Priority.Warning;
            }
            else if (period == LockedFiscalPeriod.Error)
            {
                entityError.Priority = Priority.Error;
            }
            else
            {
                entityError.Priority = Priority.Message;
            }

            return entityError;
        }

        #endregion
	}
}