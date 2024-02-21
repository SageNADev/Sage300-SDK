using System;
using System.Web.OData.Builder;
using System.Web.OData.Routing;
using Microsoft.Practices.Unity;
using Microsoft.Web.Http;
using Sage.CA.SBS.ERP.Sage300.%BASEMODULE%.WebApi.Controllers;
using Sage.CA.SBS.ERP.Sage300.%BASEMODULE%.WebApi.Models;
using Sage.CA.SBS.ERP.Sage300.Common.WebApi.Attributes;
using Sage.CA.SBS.ERP.Sage300.Common.WebApi.Controllers;
using Sage.CA.SBS.ERP.Sage300.Common.WebApi.Interfaces;

namespace Sage.CA.SBS.ERP.Sage300.%MODULE%.WebApi.Controllers
{
    public class %BASEMODEL%Ext : %BASEMODEL%
    {
        /*
        private decimal _extFieldMoney;
        [ViewField(Id = 999999999, Name = "EXTFLD", Size = 10, Precision = 3, Type = ViewFieldType.Decimal)]
        public decimal extFieldMoney { get { return _extFieldMoney; } set { _extFieldMoney = value; PropertySet(); } }
        */
    }

    /// <summary>
    /// Class %CONTROLLER%
    /// </summary>
    [ControllerName("%CONTROLLER%")]
    [ODataRoutePrefix("%CONTROLLER%")]
    %RESTRICTEDVIEWRESOURCECONTROLLER%
    public partial class %CONTROLLER%Controller : ViewResourceController<%BASEMODEL%>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="%CONTROLLER%"/> class.
        /// </summary>
        /// <param name="container">The container</param>
        public %CONTROLLER%Controller(IUnityContainer container) : base(container)
        {
        }

        /// <summary>
        /// Registers this resource as a Web API endpoint
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterODataEntity(ODataModelBuilder builder)
        {
            %BASECONTROLLER%Controller.RegisterODataEntityBase<%REGISTER_ODATA_ENTITY_PARAMETERS%>(builder, "%CONTROLLER%");
        }

        protected override IViewResourceEntity GetViewEntityHierarchy()
        {
            return %BASECONTROLLER%Controller.GetViewEntityHierarchyBase<%GET_VIEW_ENTITY_HIERARCHY_PARAMETERS%>();
        }
    }
}

