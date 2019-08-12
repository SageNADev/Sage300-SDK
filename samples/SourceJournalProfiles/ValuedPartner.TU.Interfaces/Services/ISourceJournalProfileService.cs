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

using ValuedPartner.TU.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Service;
using Sage.CA.SBS.ERP.Sage300.Common.Models;

#endregion

namespace ValuedPartner.TU.Interfaces.Services
{
    /// <summary>
    /// Interface ISourceJournalProfileService
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public interface ISourceJournalProfileService<T> : IEntityService<T>, ISecurityService 
        where T : SourceJournalProfile, new()
    {
        /// <summary>
        /// Gets SourceJournal Detail
        /// </summary>
        /// <param name="id">sourceJournalName</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        EnumerableResponse<SourceCode> GetSourceJournal(string id, int pageNumber, int pageSize);

        /// <summary>
        /// Check valid Source Code
        /// </summary>
        /// <param name="sourceLedger">sourceLedger</param>
        /// <param name="sourceType">sourceType</param>
        /// <returns>True/False</returns>
        bool CheckValidSourceCode(string sourceLedger, string sourceType);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SourceJournalProfile</returns>
        T Delete(string id);
    }
}