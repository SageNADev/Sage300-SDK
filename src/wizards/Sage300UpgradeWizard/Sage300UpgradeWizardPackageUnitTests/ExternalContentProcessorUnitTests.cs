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

#region Imports
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.PerRelease;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Interfaces;
using EnvDTE;
using EnvDTE80;
#endregion

namespace Sage300UpgradeWizardPackageUnitTests
{
    [TestClass]
    public class ExternalContentProcessorUnitTests
    {
        private ISettings _mockSettings;

        private void CreateMockSettings()
        {
            _mockSettings = new Settings
            {
                DestinationSolutionFolder = @"C:\Users\GrGagnaux\source\repos\Test102",
                DestinationWebFolder = @"C:\Users\GrGagnaux\source\repos\Test102\SuperConsulting.SC.Web",
                SourceFolder = string.Empty,
                WizardSteps = null,
                Solution = null
            };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ExternalContentProcessorUnitTests()
        {
            CreateMockSettings();
        }

        [TestMethod]
        public void Test_ExtractModuleIdFromPath_ValidPath_ExpectSuccess()
        {
            // Arrange
            var processor = new ExternalContentProcessor(_mockSettings);
            var expectedModuleId = "SC";
            var actualModuleId = string.Empty;

            // Act
            actualModuleId = processor.ExtractModuleIdFromPath(_mockSettings.DestinationWebFolder);

            // Assert
            Assert.IsTrue(expectedModuleId.Equals(actualModuleId, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
