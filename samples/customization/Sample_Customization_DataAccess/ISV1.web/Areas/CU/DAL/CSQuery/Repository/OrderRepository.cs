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
    public class OrderRepository
    {
    }

    public class OrderRepository<T> : DynamicQueryRepository<T> where T : Order, new()
    {
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
            var fields = typeof(OrderFields).GetFields( BindingFlags.Public | BindingFlags.Static);
            bool headerMappered = false;
            var model = new T();

            // Map business CS query entity value to model value
            while (BusinessEntity.Fetch(false))
            {
                var fieldValue = "";
                var propTypeName = "";
                
                //Set model(header) value 
                if (!headerMappered)
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
                    headerMappered = true;
                }
                
                //Set sub model(detail) value
                var subModel = new OrderDetail();
                var subModelProperties = subModel.GetType().GetProperties();
                foreach (var prop in subModelProperties)
                {
                    var field = fields.Where(f => f.Name == prop.Name).FirstOrDefault();
                    if (field != null)
                    {
                        propTypeName = prop.PropertyType.Name;
                        fieldValue = field.GetRawConstantValue().ToString();
                        SetModelValue(prop, subModel, propTypeName, fieldValue);
                    }
                }
                model.OrderDetails.Add(subModel);
                items.Add(model);
            }

            response.Items = items;
            return response;
        }

        /// <summary>
        /// Get model by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(string id)
        {
            id = "ORD000000000001";
            var model = new T();
            if (!string.IsNullOrEmpty(id))
            {
                var sql = "select * from  SAMLTD.dbo.OEORDH o inner join SAMLTD.dbo.OEORDD od on o.ORDUNIQ = od.ORDUNIQ where ORDNUMBER = '{0}'";
                var response = ExecuteSQL(sql,id);
                model = response.Items.Any() ? response.Items.ToList<T>()[0] : model;
            }
            return model;
        }
        
        /// <summary>
        /// Save/Update model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Save(T model) 
        {
            return null;
        }
        
        /// <summary>
        /// Delete by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Delete(string id) 
        {
            return null;
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