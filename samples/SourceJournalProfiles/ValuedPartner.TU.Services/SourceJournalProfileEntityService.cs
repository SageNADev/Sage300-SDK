// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Services.Base;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;

#endregion

namespace ValuedPartner.TU.Services
{
    /// <summary>
    /// Class for SourceJournalProfileEntityService
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public class SourceJournalProfileEntityService<T> : FlatService<T, ISourceJournalProfileEntity<T>>, ISourceJournalProfileService<T>
        where T : SourceJournalProfile, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfile
        /// </summary>
        /// <param name="context">Request Context</param>
        public SourceJournalProfileEntityService(Context context)
            : base(context)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Source Journal
        /// </summary>
        /// <param name="id">Source Journal Name</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of SourceCode</returns>
        public EnumerableResponse<SourceCode> GetSourceJournal(string id, int pageNumber, int pageSize)
        {
            using (var repository = Resolve<ISourceJournalProfileEntity<T>>())
            {
                return repository.GetSourceJournal(id, pageNumber, pageSize);
            }
        }

        /// <summary>
        /// CheckValidSourceCode
        /// </summary>
        /// <param name="sourceLedger">SourceLedger</param>
        /// <param name="sourceType">SourceType</param>
        /// <returns>True/False</returns>
        public bool CheckValidSourceCode(string sourceLedger, string sourceType)
        {
            using (var repository = Resolve<ISourceJournalProfileEntity<T>>())
            {
                return repository.CheckValidSourceCode(sourceLedger, sourceType);
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">SourceJournalName</param>
        /// <returns>SourceJournalProfile</returns>
        public T Delete(string id)
        {
            using (var repository = Resolve<ISourceJournalProfileEntity<T>>())
            {
                return repository.Delete(id);
            }
        }

        #endregion
    }
}