using ISV1.web.Areas.CU.DAL.SageViews.Mapper;
using ISV1.web.Areas.CU.DAL.SageViews.Model;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.SageViews.Repository
{
    public class CustomerRepository<T> : FlatRepository<T> where T : Customer, new()
    {
        public const string EntityName = "AR0024";
        private IBusinessEntity _businessEntity;

        public CustomerRepository(Context context)
            : base(context, new CustomerMapper<T>(context), ActiveFilter)
        {
        }
        protected override IBusinessEntity CreateBusinessEntities()
        {
            _businessEntity = OpenEntity(EntityName);
            return _businessEntity;
        }
        protected override Expression<Func<T, bool>> GetUpdateExpression(T model)
        {
            return entity => entity.CustomerNumber == model.CustomerNumber;
        }

        private static Expression<Func<T, Boolean>> ActiveFilter
        {
            get { return null; }
        }
    }
}