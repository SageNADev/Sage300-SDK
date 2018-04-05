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
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;

#endregion

namespace ValuedPartner.TU.Interfaces.BusinessRepository
{
    /// <summary>
    /// Interface ISourceJournalProfileEntity
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public interface ISourceJournalProfileEntity<T> : IBusinessRepository<T>, ISecurity 
        where T : SourceJournalProfile, new()
    {
        /// <summary>
        /// Gets SourceJournal Detail
        /// </summary>
        /// <param name="id">SourceJournalName</param>
        /// <param name="pageNumber">PageNumber</param>
        /// <param name="pageSize">PageSize</param>
        EnumerableResponse<SourceCode> GetSourceJournal(string id, int pageNumber, int pageSize);

        /// <summary>
        /// Check Source Code
        /// </summary>
        /// <param name="sourceLedger">SourceLedger</param>
        /// <param name="sourceType">SourceType</param>
        /// <returns>True/False</returns>
        bool CheckValidSourceCode(string sourceLedger, string sourceType);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">SourceJournalName</param>
        /// <returns>SourceJournalProfile</returns>
        T Delete(string id);
    }
}
