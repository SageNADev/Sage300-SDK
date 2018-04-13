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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.ADO_EF.Interface
{
    //Interface for Generic Repository for Entity Framework  
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        
        /// <summary>
        /// Get records by key values, supports composite keys
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        T GetByID(params object[] keyValues);
        
        /// <summary>
        /// Insert new record
        /// </summary>
        /// <param name="obj"></param>
        void Insert(T obj);
        
        /// <summary>
        /// Update exsiting record
        /// </summary>
        /// <param name="obj"></param>
        void Update(T obj);
        
        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="keyValues"></param>
        void Delete(params object[] keyValues);
        
        /// <summary>
        /// Save the changes
        /// </summary>
        void Save();
    }
}