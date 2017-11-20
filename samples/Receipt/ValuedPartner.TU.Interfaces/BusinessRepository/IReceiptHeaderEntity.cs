// The MIT License (MIT) 
// Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved.
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

#region

using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
<<<<<<< HEAD
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Models;
using System;
using System.Linq.Expressions;

#endregion

namespace ValuedPartner.TU.Interfaces.BusinessRepository
=======
using ValuedParter.TU.Models;

#endregion

namespace ValuedParter.TU.Interfaces.BusinessRepository
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
{
    /// <summary>
    /// An Interface for TU Receipt Finder
    /// </summary>
<<<<<<< HEAD
    /// <typeparam name="T">Model of type Receipt Header</typeparam>
=======
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
    public interface IReceiptHeaderEntity: IBusinessRepository<ReceiptHeader>, ISecurity  
    {
    }
}
