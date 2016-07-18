// The MIT License (MIT) 
// Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved.
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

using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using System.ComponentModel.Composition;
using ValuedParter.TU.Interfaces.Services;
using ValuedParter.TU.Models;
using ValuedParter.TU.Interfaces.BusinessRepository;
using ValuedParter.TU.BusinessRepository;
using ValuedParter.TU.Services;

namespace ValuedParter.TU.Services
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
            UnityUtil.RegisterType<IReceiptService<ReceiptHeader, ReceiptDetail, ReceiptOptionalField, ReceiptDetailOptionalField, ReceiptDetailLotNumber, ReceiptDetailSerialNumber>, ReceiptEntityService<ReceiptHeader, ReceiptDetail, ReceiptOptionalField, ReceiptDetailOptionalField, ReceiptDetailLotNumber, ReceiptDetailSerialNumber>>(container);
        }

        /// <summary>
        /// Register repositories 
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterRepositories(IUnityContainer container)
        {
            UnityUtil.RegisterType(container, typeof(IReceiptEntity<ReceiptHeader, ReceiptDetail, ReceiptOptionalField, ReceiptDetailOptionalField, ReceiptDetailLotNumber, ReceiptDetailSerialNumber>), typeof(ReceiptRepository<ReceiptHeader,ReceiptDetail, ReceiptOptionalField, ReceiptDetailOptionalField, ReceiptDetailLotNumber, ReceiptDetailSerialNumber>), UnityInjectionType.Default, new InjectionConstructor(typeof(Context)));
            UnityUtil.RegisterType(container, typeof(IReceiptEntity<ReceiptHeader, ReceiptDetail, ReceiptOptionalField, ReceiptDetailOptionalField, ReceiptDetailLotNumber, ReceiptDetailSerialNumber>), typeof(ReceiptRepository<ReceiptHeader, ReceiptDetail, ReceiptOptionalField, ReceiptDetailOptionalField,ReceiptDetailLotNumber, ReceiptDetailSerialNumber>), UnityInjectionType.Session, new InjectionConstructor(typeof(Context), typeof(IBusinessEntitySession)));
        }
    }
}
