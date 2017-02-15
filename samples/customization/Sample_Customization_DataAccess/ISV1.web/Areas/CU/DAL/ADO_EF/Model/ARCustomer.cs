//Can be generated by ADO.NET Entity Data Model by code first. Refactoring to required entity class
//

namespace ISV1.web.Areas.CU.DAL.ADO_EF.Model
{
    using Sage.CA.SBS.ERP.Sage300.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARCUS")]
    public partial class ARCustomer 
    {
        [Key]
        [StringLength(12)]
        [Column("IDCUST")]
        public string CustomerNumber { get ; set; }

        [Required]
        [StringLength(60)]
        [Column("NAMECUST")]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(60)]
        [Column("EMAIL2")]
        public string Email { get; set; }

        [Required]
        [StringLength(60)]
        [Column("TEXTPHON2")]
        public string FaxNumber { get; set; }

        [Required]
        [StringLength(60)]
        [Column("NAMECTAC")]
        public string ContactName { get; set; }

        [Required]
        [StringLength(60)]
        [Column("EMAIL1")]
        public string ContactsEmail { get; set; }

        [Required]
        [StringLength(6)]
        [Column("IDACCTSET")]
        public string AccountSet { get; set; }

        [Required]
        [StringLength(100)]
        [Column("WEBSITE")]
        public string WebSite { get; set; }

        [Column("BILLMETHOD")]
        public short BillMethod { get; set; }

        [Required]
        [StringLength(12)]
        [Column("PAYMCODE")]
        public string PaymentCode { get; set; }

        public IList<ARCustomerOptionalField> ARCustomerOptionalFields { get; set; }
    }
}