
// The MIT License (MIT) 
// Copyright (c) 1994-2025 The Sage Group plc or its licensors.  All rights reserved.
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

using Unity;
using Unity.Injection;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using System.ComponentModel.Composition;

namespace ValuedPartner.TU.Services
{
    /// <summary>
    /// TU Bootstrapper Class
    /// </summary>
    [Export(typeof(IBootstrapperTask))]
    [BootstrapMetadataExport("TU", new[] { BootstrapAppliesTo.Web, BootstrapAppliesTo.Worker, BootstrapAppliesTo.WebApi }, 10)]
    public class TUBootstrapper : IBootstrapperTask
    {
        /// <summary>
        /// Bootstrap activity execution
        /// </summary>
        /// <param name="container">The Unity container</param>
        public void Execute(IUnityContainer container)
        {
            RegisterService(container);
            RegisterRepositories(container);
        }

        /// <summary>
        /// Register services 
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterService(IUnityContainer container)
        {
			UnityUtil.RegisterType<Interfaces.Services.ITaxAuthoritiesService<Models.TaxAuthorities>, TaxAuthoritiesEntityService<Models.TaxAuthorities>>(container);

        }

        /// <summary>
        /// Register repositories 
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterRepositories(IUnityContainer container)
        {
			UnityUtil.RegisterType<IExportImportRepository, BusinessRepository.TaxAuthoritiesRepository<Models.TaxAuthorities>>(container, "tutaxauthorities", new InjectionConstructor(typeof(Context)));
			UnityUtil.RegisterType(container, typeof(Interfaces.BusinessRepository.ITaxAuthoritiesEntity<Models.TaxAuthorities>), typeof(BusinessRepository.TaxAuthoritiesRepository<Models.TaxAuthorities>), UnityInjectionType.Default, new InjectionConstructor(typeof(Context)));
			UnityUtil.RegisterType(container, typeof(Interfaces.BusinessRepository.ITaxAuthoritiesEntity<Models.TaxAuthorities>), typeof(BusinessRepository.TaxAuthoritiesRepository<Models.TaxAuthorities>), UnityInjectionType.Session, new InjectionConstructor(typeof(Context), typeof(IBusinessEntitySession)));

        }
    }
}
