// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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

#region Imports
using System;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Utilities;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
#endregion

namespace Sage300UIWizardUnitTests
{
    [TestClass]
    public class CodeGenerationWizardTests
    {
        /// <summary>
        /// Build two enumeration objects to be used for testing the EnumExists method
        /// </summary>
        /// <param name="enum1">This object will be the one to search in and will represent a list of one or more enumerations</param>
        /// <param name="enum2">This object will be the one to search for and will represent a single enumeration and it's or or more values</param>
        private void BuildTestEnumerationObjects(out Dictionary<string, Dictionary<string, object>> enum1, // Search In
                                                 out Dictionary<string, Dictionary<string, object>> enum2, // Search For
                                                 bool enumExists, 
                                                 bool valuesMatch)

        {
            enum1 = new Dictionary<string, Dictionary<string, object>>();
            enum2 = new Dictionary<string, Dictionary<string, object>>();

            // Build the 'SearchIn' list (Two enumerations)
            var enumValues1 = new Dictionary<string, object>
            {
                { "Inactive", "0" },
                { "Active", "1" }
            };
            enum1.Add("Status", enumValues1);
            var enumValues2 = new Dictionary<string, object>
            {
                { "Cash", "0" },
                { "Credit", "1" },
                { "Cheque", "2" }
            };
            enum1.Add("PaymentType", enumValues2);

            if (enumExists && valuesMatch)
            {
                // Build the 'SearchFor' list (One enumeration)
                var enumValues3 = new Dictionary<string, object>
                {
                    { "Inactive", "0" },
                    { "Active", "1" }
                };
                enum2.Add("Status", enumValues3);
            }
            else if (enumExists == false)
            {
                // Build the 'SearchFor' list (One enumeration)
                var enumValues3 = new Dictionary<string, object>
                {
                    { "Value1", "0" },
                    { "Value2", "1" }
                };
                enum2.Add("Test", enumValues3);
            }
            else if (enumExists == true && valuesMatch == false)
            {
                // Build the 'SearchFor' list (One enumeration)
                var enumValues3 = new Dictionary<string, object>
                {
                    { "Good", "1" },
                    { "Bad", "0" }
                };
                enum2.Add("Status", enumValues3);
            }
            // enumExists == false && valuesMatch == false 
            // is redundant and does not need testing
        }

        [TestMethod]
        public void Test_RemoveLast_WithStringEndingWithAComma_ExpectSuccess()
        {
            // Arrange
            var str = "1,";
            var expectedOutput = "1";

            // Act
            var output = str.RemoveLast(",");

            // Assert
            Assert.IsTrue(expectedOutput == output);
        }

        [TestMethod]
        public void Test_RemoveLast_WithStringEndingWithoutAComma_ExpectSuccess()
        {
            // Arrange
            var str = "1";
            var expectedOutput = "1";

            // Act
            var output = str.RemoveLast(",");

            // Assert
            Assert.IsTrue(expectedOutput == output);
        }

        [TestMethod]
        public void Test_RemoveLast_WithEmptyString_ExpectSuccess()
        {
            // Arrange
            var str = "";
            var expectedOutput = "";

            // Act
            var output = str.RemoveLast(",");

            // Assert
            Assert.IsTrue(expectedOutput == output);
        }

        [TestMethod]
        public void Test_EnumExists_EnumerationExists_ValuesMatch_ExpectSuccess()
        {
            bool output = true;
            bool expectedOutput = true;

            var searchIn = new Dictionary<string, Dictionary<string, object>>();
            var searchFor = new Dictionary<string, Dictionary<string, object>>();
            BuildTestEnumerationObjects(out searchIn, out searchFor, enumExists: true, valuesMatch: true);

            // Act
            output = Utilities.EnumExists(searchIn, searchFor);


            // Assert
            Assert.IsTrue(expectedOutput == output);
        }

        [TestMethod]
        public void Test_EnumExists_EnumerationDoesNotExist_ExpectSuccess()
        {
            bool output = true;
            bool expectedOutput = false;

            var searchIn = new Dictionary<string, Dictionary<string, object>>();
            var searchFor = new Dictionary<string, Dictionary<string, object>>();
            BuildTestEnumerationObjects(out searchIn, out searchFor, enumExists: false, valuesMatch: false);

            // Act
            output = Utilities.EnumExists(searchIn, searchFor);

            // Assert
            Assert.IsTrue(expectedOutput == output);
        }

        [TestMethod]
        public void Test_EnumExists_EnumerationExists_ValuesDoNotMatch_ExpectSuccess()
        {
            bool output = true;
            bool expectedOutput = false;

            var searchIn = new Dictionary<string, Dictionary<string, object>>();
            var searchFor = new Dictionary<string, Dictionary<string, object>>();
            BuildTestEnumerationObjects(out searchIn, out searchFor, enumExists: true, valuesMatch: false);

            // Act
            output = Utilities.EnumExists(searchIn, searchFor);

            // Assert
            Assert.IsTrue(expectedOutput == output);
        }
    }
}
