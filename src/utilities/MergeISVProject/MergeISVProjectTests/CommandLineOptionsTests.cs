using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MergeISVProject; 

namespace MergeISVProjectTests
{
    [TestClass]
    public class CommandLineOptionsTests
    {
		private const string APPNAME = "MergeISVProject.exe";
		private const string APPVERSION = "V1.0.0.0";
        private const string APPCOPYRIGHT = "Copyright 2019";

		/// <summary>
		/// Test passing zero command-line arguments
		/// </summary>
		[TestMethod]
        public void CommandLineOptions_Test_NoParameters()
        {
			// Arrange
			string[] args = { };

			// Act
			var options = new CommandLineOptions("AppName", "V1.0", "Copyright 2019", args);
			bool anyErrors = options.AnyErrors();

			// Assert
			Assert.IsTrue(anyErrors);
		}

		/// <summary>
		/// Test to ensure all valid arguments are provided
		/// </summary>
		[TestMethod]
		public void CommandLineOptions_Test_AllValidArguments()
		{
			// Arrange
			string[] args = BuildArgumentArray();

			// Act
			var options = new CommandLineOptions(APPNAME, APPVERSION, APPCOPYRIGHT, args);
			var anyErrors = options.AnyErrors();

			// Assert
			Assert.IsFalse(anyErrors);
		}

		/// <summary>
		/// Test method CleanupArguments 
		/// </summary>
		[TestMethod]
		public void CommandLineOptions_Test_CleanupArguments()
		{
			// Arrange
			var methodToTest = "CleanupArguments";
			var badArgumentArray = MakeArgumentArraySingleEntry();

			// Act
			var options = new CommandLineOptions();
			var obj = new PrivateObject(options);

			object[] args = new object[] { badArgumentArray };
			var retVal = (string[])obj.Invoke(methodToTest, args);
			int argCountExpected = 8;
			int argCountActual = retVal.Length;

			// Assert
			Assert.AreEqual(argCountExpected, argCountActual);
		}

		/// <summary>
		/// Test method GetArgumentNameOnly
		/// </summary>
		[TestMethod]
		public void CommandLineOptions_Test_GetArgumentNameOnly()
		{
			// Arrange
			var methodToTest = "GetArgumentNameOnly";
			var optionPrefix = "--";
			var inputArgument = "--argumentname=blahblah";
			var expectedResult = "argumentname";

			// Act
			var options = new CommandLineOptions();
			options.OptionPrefix = optionPrefix;
			var obj = new PrivateObject(options);
			object[] args = new object[] { inputArgument };
			var retVal = (string)obj.Invoke(methodToTest, args);

			// Assert
			Assert.AreEqual(expectedResult, retVal);
		}

		/// <summary>
		/// Generate an example command-line string array
		/// </summary>
		/// <returns>The Array</returns>
		private string[] BuildArgumentArray()
		{
			string[] args =
			{
				"--mode=0",
				"--solutionpath=\"E:\\Sage300CMvcApplication1\\Sage300CMvcApplication1\"",
				"--webprojectpath=\"E:\\Sage300CMvcApplication1\\Sage300CMvcApplication1\\TrustedVendor.PM.Web\"",
				"--menufilename=\"PMMenuDetails.xml\"",
				"--buildprofile=\"release\"",
				"--dotnetframeworkpath=\"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\"",
				"--minify",
				"--nodeploy",
				"--log"
			};
			return args;
		}

		/// <summary>
		/// Generate a fake single-entry string array
		/// to mimic passing in command-line parameters on 
		/// their own individual lines.
		/// Instead of spaces separating each argument,
		/// we're going to use NewLine characters instead.
		/// </summary>
		/// <returns>The single-entry array containing all command-line arguments</returns>
		private string[] MakeArgumentArraySingleEntry()
		{
			var temp = string.Empty;
			string[] output = new string[] { String.Empty };
			var args = BuildArgumentArray();
			foreach (var s in args)
			{
				temp += s + Environment.NewLine;
			}
			output[0] = temp;
			return output;
		}
	}
}
