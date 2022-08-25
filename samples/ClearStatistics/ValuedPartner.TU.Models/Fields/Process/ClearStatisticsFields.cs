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

#region Namespace

#endregion

namespace ValuedPartner.TU.Models.Process
{
    /// <summary>
    /// Contains list of ClearStatistics Constants
    /// </summary>
    public partial class ClearStatistics
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public const string EntityName = "AR0065";

        #region Properties

        /// <summary>
        /// Contains list of ClearStatistics Field Constants
        /// </summary>
        public class Fields
        {

            /// <summary>
            /// Property for From Customer No
            /// </summary>
            public const string FromCustomerNumber = "STRTCUSID";

            /// <summary>
            /// Property for To Customer No
            /// </summary>
            public const string ToCustomerNumber = "ENDCUSID";

            /// <summary>
            /// Property for From Group Code
            /// </summary>
            public const string FromGroupCode = "STRTGRPID";

            /// <summary>
            /// Property for To Group Code
            /// </summary>
            public const string ToGroupCode = "ENDGRPID";

            /// <summary>
            /// Property for From National Account
            /// </summary>
            public const string FromNationalAccount = "STRTNATID";

            /// <summary>
            /// Property for To National Account
            /// </summary>
            public const string ToNationalAccount = "ENDNATID";

            /// <summary>
            /// Property for From Sales Person
            /// </summary>
            public const string FromSalesperson = "STRTSPSID";

            /// <summary>
            /// Property for To Sales Person
            /// </summary>
            public const string ToSalesperson = "ENDSPSID";

            /// <summary>
            /// Property for From Item Number
            /// </summary>
            public const string FromItemNumber = "STRTITSID";

            /// <summary>
            /// Property for To Item Number
            /// </summary>
            public const string ToItemNumber = "ENDITSID";

            /// <summary>
            /// Property for Clear Customer Statistics
            /// </summary>
            public const string ClearCustomerStatistics = "SWPRGCSM";

            /// <summary>
            /// Property for Clear Group Statistics
            /// </summary>
            public const string ClearGroupStatistics = "SWPRGGSM";

            /// <summary>
            /// Property for Clear National Acct Statistics
            /// </summary>
            public const string ClearNationalAccountStatistics = "SWPRGNSM";

            /// <summary>
            /// Property for Clear Sales Person Statistics 
            /// </summary>
            public const string ClearSalespersonStatistics = "SWPRGSPS";

            /// <summary>
            /// Property for Clear Item Statistics
            /// </summary>
            public const string ClearItemStatistics = "SWPRGITS";

            /// <summary>
            /// Property for Through Customer Year
            /// </summary>
            public const string ThroughCustomerYear = "THRUCUSYR";

            /// <summary>
            /// Property for Through Customer Period
            /// </summary>
            public const string ThroughCustomerPeriod = "THRUCUSPER";

            /// <summary>
            /// Property for Through National Acct Year
            /// </summary>
            public const string ThroughNationalAccountYear = "THRUNATYR";

            /// <summary>
            /// Property for Through National Acct Period
            /// </summary>
            public const string ThroughNationalAccountPeriod = "THRUNATPER";

            /// <summary>
            /// Property for Through Group Year
            /// </summary>
            public const string ThroughGroupYear = "THRUGRPYR";

            /// <summary>
            /// Property for Through Group Period
            /// </summary>
            public const string ThroughGroupPeriod = "THRUGRPPER";

            /// <summary>
            /// Property for Through Sales Person Year
            /// </summary>
            public const string ThroughSalespersonYear = "THRUSAPYR";

            /// <summary>
            /// Property for Through Sales Person Period
            /// </summary>
            public const string ThroughSalespersonPeriod = "THRUSAPPER";

            /// <summary>
            /// Property for Through Item Year
            /// </summary>
            public const string ThroughItemYear = "THRUITSYR";

            /// <summary>
            /// Property for Through Item Period
            /// </summary>
            public const string ThroughItemPeriod = "THRUITSPER";

        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains list of Clear Statistics Index Constants
        /// </summary>
        public class Index
        {

            /// <summary>
            /// Property Indexer for From Customer No
            /// </summary>
            public const int FromCustomerNumber = 1;

            /// <summary>
            /// Property Indexer for To Customer No
            /// </summary>
            public const int ToCustomerNumber = 2;

            /// <summary>
            /// Property Indexer for From Group Code
            /// </summary>
            public const int FromGroupCode = 3;

            /// <summary>
            /// Property Indexer for To Group Code
            /// </summary>
            public const int ToGroupCode = 4;

            /// <summary>
            /// Property Indexer for From National Account
            /// </summary>
            public const int FromNationalAccount = 5;

            /// <summary>
            /// Property Indexer for To National Account
            /// </summary>
            public const int ToNationalAccount = 6;

            /// <summary>
            /// Property Indexer for From Sales Person
            /// </summary>
            public const int FromSalesperson = 7;

            /// <summary>
            /// Property Indexer for To Sales Person
            /// </summary>
            public const int ToSalesperson = 8;

            /// <summary>
            /// Property Indexer for From Item Number
            /// </summary>
            public const int FromItemNumber = 11;

            /// <summary>
            /// Property Indexer for To Item Number
            /// </summary>
            public const int ToItemNumber = 12;

            /// <summary>
            /// Property Indexer for Clear Customer Statistics
            /// </summary>
            public const int ClearCustomerStatistics = 13;

            /// <summary>
            /// Property Indexer for Clear Group Statistics
            /// </summary>
            public const int ClearGroupStatistics = 14;

            /// <summary>
            /// Property Indexer for Clear National Acct Statistics
            /// </summary>
            public const int ClearNationalAccountStatistics = 15;

            /// <summary>
            /// Property Indexer for Clear Sales Person Statistics 
            /// </summary>
            public const int ClearSalespersonStatistics = 16;

            /// <summary>
            /// Property Indexer for Clear Item Statistics
            /// </summary>
            public const int ClearItemStatistics = 17;

            /// <summary>
            /// Property Indexer for Through Customer Year
            /// </summary>
            public const int ThroughCustomerYear = 18;

            /// <summary>
            /// Property Indexer for Through Customer Period
            /// </summary>
            public const int ThroughCustomerPeriod = 19;

            /// <summary>
            /// Property Indexer for Through National Acct Year
            /// </summary>
            public const int ThroughNationalAccountYear = 20;

            /// <summary>
            /// Property Indexer for Through National Acct Period
            /// </summary>
            public const int ThroughNationalAccountPeriod = 21;

            /// <summary>
            /// Property Indexer for Through Group Year
            /// </summary>
            public const int ThroughGroupYear = 22;

            /// <summary>
            /// Property Indexer for Through Group Period
            /// </summary>
            public const int ThroughGroupPeriod = 23;

            /// <summary>
            /// Property Indexer for Through Sales Person Year
            /// </summary>
            public const int ThroughSalespersonYear = 24;

            /// <summary>
            /// Property Indexer for Through Sales Person Period
            /// </summary>
            public const int ThroughSalespersonPeriod = 25;

            /// <summary>
            /// Property Indexer for Through Item Year
            /// </summary>
            public const int ThroughItemYear = 26;

            /// <summary>
            /// Property Indexer for Through Item Period
            /// </summary>
            public const int ThroughItemPeriod = 27;

        }

        #endregion

    }
}
