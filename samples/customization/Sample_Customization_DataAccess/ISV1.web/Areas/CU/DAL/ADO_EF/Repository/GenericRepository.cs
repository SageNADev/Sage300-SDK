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

using ISV1.web.Areas.CU.DAL.ADO_EF.Interface;
using ISV1.web.Areas.CU.DAL.ADO_EF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.ADO_EF.Repository
{
    /// <summary>
    /// Repository Implementaion for Data Access layer using Entity Framework
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private CustomDbContext db = null;
        private DbSet<T> table = null;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericRepository()
        {
            this.db = new CustomDbContext();
            table = db.Set<T>();
        }

        /// <summary>
        /// Constructor for pass db context
        /// </summary>
        /// <param name="db"></param>
        public GenericRepository(CustomDbContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        /// <summary>
        /// Get list of records based filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> GetListByFilter(Expression<Func<T, bool>> filter)
        {
            return table.Where(filter).ToList();
        }

        /// <summary>
        /// Get record by key, supports multiple keys(composite key)
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T GetByID(params object[] keyValues)
        {
            return table.Find(keyValues);
        }

        /// <summary>
        /// Insert record
        /// </summary>
        /// <param name="obj"></param>
        public void Insert(T obj)
        {
            table.Add(obj);
            Save();
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="obj"></param>
        public void Update(T obj)
        {
            table.Attach(obj);
            db.Entry(obj).State = EntityState.Modified;
            Save();
        }

        /// <summary>
        /// Delete record based on keys, support multiple key
        /// </summary>
        /// <param name="keyValues"></param>
        public void Delete(params object[] keyValues)
        {
            T existing = table.Find(keyValues);
            table.Remove(existing);
            Save();
        }

        /// <summary>
        /// Save the changes to database
        /// </summary>
        public void Save()
        {
            db.SaveChanges();
        }
    }
}