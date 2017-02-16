using ISV1.web.Areas.CU.DAL.CSQuery.Model;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.CSQuery.Repository
{
    public class OrderRepository<T> : DynamicQueryRepository<T> where T : Order, new()
    {
        bool _mapHeaderOnly = false;
        bool _getUniqueHeader = true;

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        ///<param name="context">context</param>
        public OrderRepository(Context context) : base(context, null)
        {
        }

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="session">session</param>
        public OrderRepository(Context context, IBusinessEntitySession session) : base(context, session)
        {
        }

        #endregion

        protected override DynamicQueryEnumerableResponse<T> Map()
        {
            // Init response and items for mapping
            var response = new DynamicQueryEnumerableResponse<T>();
            var items = new List<T>();
            var model = new T();
            var fields = typeof(OrderFields).GetFields( BindingFlags.Public | BindingFlags.Static);
            bool getHeader = true;

            // Map business CS query entity value to model value
            while (BusinessEntity.Fetch(false))
            {
                
                var fieldValue = "";
                var propTypeName = "";

                if (!_getUniqueHeader) 
                {
                    model = new T();
                }
                //Set model(header) value 
                if (getHeader)
                {
                    var modelProperties = model.GetType().GetProperties();
                    foreach (var prop in modelProperties)
                    {
                        var field = fields.Where(f => f.Name == prop.Name).FirstOrDefault();
                        if (field != null)
                        {
                            propTypeName = prop.PropertyType.Name;
                            fieldValue = field.GetRawConstantValue().ToString();
                            SetModelValue(prop, model, propTypeName, fieldValue);
                        }
                    }
                    getHeader = ! _getUniqueHeader;
                }

                //Set detail model value
                if (!_mapHeaderOnly)
                {
                    var detailModel = new OrderDetail();
                    var detailModelProperties = detailModel.GetType().GetProperties();
                    foreach (var prop in detailModelProperties)
                    {
                        var field = fields.Where(f => f.Name == prop.Name).FirstOrDefault();
                        if (field != null)
                        {
                            propTypeName = prop.PropertyType.Name;
                            fieldValue = field.GetRawConstantValue().ToString();
                            SetModelValue(prop, detailModel, propTypeName, fieldValue);
                        }
                    }
                    model.OrderDetails.Add(detailModel);
                }

                items.Add(model);
            }

            response.Items = items;
            return response;
        }

        /// <summary>
        /// Get model by Id. For simple, it's join two tables to get data, it can join more tables to get required fields
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(string id)
        {
            var model = new T();
            if (!string.IsNullOrEmpty(id))
            {
                var sql = "select * from  SAMLTD.dbo.OEORDH o inner join SAMLTD.dbo.OEORDD od on o.ordUniq = od.ordUniq where o.ordNumber = '{0}'";
                var response = ExecuteSQL(sql,id);
                model = response.Items.Any() ? response.Items.ToList<T>()[0] : model;
            }
            return model;
        }

        /// <summary>
        /// Get all records, return key column
        /// </summary>
        /// <returns></returns>
        public List<string> GetAll()
        {
            var sql = "select * from  SAMLTD.dbo.OEORDH";
            _mapHeaderOnly = true;
            _getUniqueHeader = false;
            var response = ExecuteSQL(sql);
            return response.Items.Select(r => r.OrderNumber).ToList();
        }

        /// <summary>
        /// Save/Update model. For simple just save/update Order Comment, ShiptoContact and BillToName fields, modify sql query to support more fields update.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Save(T model) 
        {
            if (model != null)
            {
                var sql = "update SAMLTD.dbo.OEORDH set Comment = '{1}', ShpContact = '{2}', BilName = '{3}' where ordNumber = '{0}'";
                var response = ExecuteSQL(sql, model.OrderNumber, model.OrderComment, model.ShipToContact, model.BillToName);
            }
            return model;
        }
        
        /// <summary>
        /// Delete by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual void Delete(string id) 
        {
            if (!string.IsNullOrEmpty(id))
            {
                var sql = "delete from SAMLTD.dbo.OEORDH where ordNumber = '{0}'";
                var response = ExecuteSQL(sql, id);
            }
        }
        /// <summary>
        /// Set model value based on entity fields type
        /// Todo : Expand this function to support object, enum and other type
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="propTypeName">entity field type name</param>
        private void SetModelValue(PropertyInfo prop, dynamic model, string propTypeName, string fieldValue)
        {
            switch (propTypeName)
            {
                case "String":
                    prop.SetValue(model, BusinessEntity.GetValue<string>(fieldValue));
                    break;
                case "int":
                    prop.SetValue(model, BusinessEntity.GetValue<int>(fieldValue));
                    break;
                case "Int16":
                    prop.SetValue(model, BusinessEntity.GetValue<int>(fieldValue));
                    break;
                case "Int32":
                    prop.SetValue(model, BusinessEntity.GetValue<int>(fieldValue));
                    break;
                case "Byte":
                    prop.SetValue(model, BusinessEntity.GetValue<byte>(fieldValue));
                    break;
                case "Boolean":
                    prop.SetValue(model, BusinessEntity.GetValue<bool>(fieldValue));
                    break;
                case "Single":
                    prop.SetValue(model, BusinessEntity.GetValue<float>(fieldValue));
                    break;
                case "Int64":
                    prop.SetValue(model, BusinessEntity.GetValue<long>(fieldValue));
                    break;
                case "Decimal":
                    prop.SetValue(model, BusinessEntity.GetValue<decimal>(fieldValue));
                    break;
                case "Double":
                    prop.SetValue(model, BusinessEntity.GetValue<double>(fieldValue));
                    break;
                case "DateTime":
                    //database saved datetime field as decimal(yyyyMMdd), need convert to DateTime
                    var dt = BusinessEntity.GetValue<decimal>(fieldValue).ToString();
                    DateTime dtValue = DateTime.ParseExact(dt, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    prop.SetValue(model, dtValue);
                    break;
                default:
                    prop.SetValue(model, BusinessEntity.GetValue<string>(fieldValue));
                    break;
            }
        }

    }
}