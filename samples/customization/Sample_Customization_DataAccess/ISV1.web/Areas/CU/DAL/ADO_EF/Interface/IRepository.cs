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