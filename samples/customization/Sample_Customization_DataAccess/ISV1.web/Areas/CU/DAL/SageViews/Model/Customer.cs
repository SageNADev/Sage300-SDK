
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.SageViews.Model
{
    public class Customer : ModelBase
    {
        public Customer()
        {
            CustomerOptionalFields = new List<CustomerOptionalField>();
        }
        [Key]
        public string CustomerNumber { get; set; }
        public string ShortName { get; set; }
        public string GroupCode { get; set; }
        public string CustomerName { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string StateOrProv { get; set; }
        public string ZipOrPostalCode { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }

        public List<CustomerOptionalField> CustomerOptionalFields { get; set; }
    }

    public class CustomerOptionalField : ModelBase
    {
        [Key]
        public string CustomerNumber { get; set; }
        [Key]
        public string OptionalField { get; set; }
        public string OptionalFieldDescription { get; set; }
        public string ValueDescription { get; set; }
        public string Value { get; set; }
        public string ValueSet { get; set; }
        public short Type { get; set; }
        public short Length { get; set; }
        public short Decimals { get; set; }
        public short AllowNull { get; set; }
        public short Validate { get; set; }
    }

    public class FieldsIndex
    {
        public const int CustomerNumber = 1;
        public const int ShortName = 2;
        public const int GroupCode = 3;
        public const int NationalAccount = 4;
        public const int Status = 5;
        public const int CustomerName = 14;
        public const int AddressLine1 = 15;
        public const int City = 19;
        public const int ContactName = 23;
        public const int ContactsEmail = 128;
        public const int ContactsFax = 142;
        public const int ContactsPhone = 141;
        public const int Country = 22;
        public const int Email = 129;
        public const int FaxNumber = 25;
        public const int StateOrProv = 20;
        public const int ZipOrPostalCode = 21;
        public const int PhoneNumber = 24;
        public const int BusinessRegistrationNumber = 174;

        public const int OptionalField = 2;
        public const int Value = 3;
        public const int Type = 4;
        public const int Length = 5;
        public const int Decimals = 6;
        public const int AllowNull = 7;
        public const int Validate = 8;
        public const int ValueSet = 9;
        public const int OptionalFieldDescription = 28;
        public const int ValueDescription = 29;

    }
}