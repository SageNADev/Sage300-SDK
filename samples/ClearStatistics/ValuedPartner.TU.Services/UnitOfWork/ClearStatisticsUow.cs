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

#region Namespace

using Unity;
using ValuedPartner.TU.Interfaces.BusinessRepository.Process;
using ValuedPartner.TU.Models.Process;
using Sage.CA.SBS.ERP.Sage300.Common.Services.UnitOfWork;
using Sage.CA.SBS.ERP.Sage300.Core.Interfaces.Storage.Queue;
using Sage.CA.SBS.ERP.Sage300.Workflow.Models;
using System;

#endregion

namespace ValuedPartner.TU.Services.UnitOfWork
{
    class ClearStatisticsUow : ProcessUow<ClearStatistics, IClearStatisticsEntity<ClearStatistics>>
    {
        /// <summary>
        /// Constructor to initialize base class ProcessUow
        /// </summary>
        /// <param name="workflowInstance">WorkFlowInstance</param>
        /// <param name="unitOfWorkInstance">UnitOfWorkInstance</param>
        /// <param name="queue">Azure Queue</param>
        /// <param name="keepAlive">KeepAlive</param>
        /// <param name="container">Unity Container</param>
        public ClearStatisticsUow(
            WorkflowInstance workflowInstance, UnitOfWorkInstance unitOfWorkInstance,
            IQueue queue, Action keepAlive, IUnityContainer container) :
            base(workflowInstance, unitOfWorkInstance, queue, keepAlive, container) { }
    }

}
