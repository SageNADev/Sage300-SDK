using ISV1.web.Areas.CU.DAL.SageViews.Mapper;
using ISV1.web.Areas.CU.DAL.SageViews.Model;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
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
        //entity view IDs
        private const string HeaderEntityName = "AR0024";
        private const string DetailEntityName = "AR0400";

        private IBusinessEntity _header;
        private IBusinessEntity _detail;

        /// <summary>
        /// Mappers
        /// </summary>
        private ModelMapper<Customer> _headerMapper;
        private ModelMapper<CustomerOptionalField> _detailMapper;

        public CustomerRepository(Context context)
            : base(context, new CustomerMapper<T>(context), ActiveFilter)
        {
            _headerMapper = new CustomerMapper<Customer>(context);
            _detailMapper = new CustomerOptionalFieldMapper<CustomerOptionalField>(context);
        }
        /// <summary>
        /// Open entity views
        /// </summary>
        /// <returns></returns>
        protected override IBusinessEntity CreateBusinessEntities()
        {
            _header = OpenEntity(HeaderEntityName);
            _detail = OpenEntity(DetailEntityName);
            _header.Compose(new[] { _detail.View });
            return _header;
        }

        /// <summary>
        /// Get header and related details data by Id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Customer GetById(string id)
        {
            CreateBusinessEntities();
            var filter = "(IDCUST = \"" + id + "\")";
            _header.Browse(filter, true);
            var headerData = (_header.Fetch(false)) ? _headerMapper.Map(_header) : null;
            
            _detail.ClearRecord();
            _detail.Browse(filter, true);
            // Add details records
            while (_detail.Fetch(false))
            {
                var detailData = _detailMapper.Map(_detail);
                headerData.CustomerOptionalFields.Add(detailData);
            }

            return headerData;
        }

        protected override Expression<Func<T, bool>> GetUpdateExpression(T model)
        {
            return entity => entity.CustomerNumber == model.CustomerNumber;
        }

        private static Expression<Func<T, Boolean>> ActiveFilter
        {
            get { return null; }
        }

        /// <summary>
        /// Get all header records Id list
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAll()
        {
            var customerEntity = OpenEntity(HeaderEntityName);
            var list = new List<string>();
            customerEntity.Top();
            while (customerEntity.Fetch(false))
	        {
                list.Add(customerEntity.GetValue<string>(FieldsIndex.CustomerNumber));
	        }
            return list;
        }
    }
}