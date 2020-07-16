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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MergeISVProject.CustomAttributes;
using MergeISVProject.Interfaces;
using MergeISVProject.CustomExceptions;
#endregion

namespace MergeISVProject
{
	/// <summary>
	/// This class will handle the loading and parsing of command-line arguments
	/// </summary>
	public class CommandLineOptions : ICommandLineOptions
    {
		#region Constants
		/// <summary>
		/// This is the default prefix character block when specifying
		/// command-line arguments to this program
		/// </summary>
		private const string DEFAULT_PREFIX = "--";

		private const string SINGLE_SPACE = @" ";
		private const char SINGLE_SPACE_CHAR = ' ';
	    private const int EXPECTED_MODULEID_LENGTH = 2;
		#endregion

		#region Private Variables and Properties
		private string divider = new String('-', 105);

		/// <summary>
		/// This will contain the originally passed in argument list
		/// </summary>
        private string[] rawArgList { get; set; }

		/// <summary>
		/// This will contain a cleaned up argument list
		/// </summary>
		private string[] cleanArgList { get; set; }
		#endregion

		#region Public Properties

		/// <summary>
		/// This will contain the cleaned-up argument list
		/// </summary>
		public string[] Arguments => cleanArgList;

		/// <summary>
		/// This will contain the prefix being used when specifying
		/// command-line arguments.
		/// </summary>
		public string OptionPrefix { get; set; }

		/// <summary>
		/// This will contain the name of the application
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		/// This will contain the version of the application
		/// </summary>
		public string ApplicationVersion { get; set; }

        /// <summary>
        /// This will contain the copyright information of the application
        /// </summary>
        public string ApplicationCopyright { get; set; }

        /// <summary>
        /// This will contain the build date of the application
        /// </summary>
        public string ApplicationBuildDate { get; set; }

        /// <summary>
        /// This will contain the build year of the application
        /// Used for the copyright range 
        /// </summary>
        public string ApplicationBuildYear { get; set; }

        /// <summary>
        /// This will contain the list of all errors that
        /// occurred when attempting to load and parse the
        /// command-line options.
        /// </summary>
        public List<string> LoadErrors { get; private set; }

		/// <summary>
		/// This will contain the formatted message 
		/// used when running the application without any parameters.
		/// </summary>
		public string UsageMessage { get; private set; }

		/// <summary>
		/// This will contain the Vendor's unique module id
		/// It is based on the first two characters of the 
		/// MenuFilename specified on the command-line.
		/// </summary>
		public string ModuleId => !string.IsNullOrEmpty(MenuFilename.OptionValue) ? MenuFilename?.OptionValue.Substring(0, EXPECTED_MODULEID_LENGTH) : string.Empty;

	    #endregion

		#region Public Properties that map to valid command-line options

		// Note to developers: 
		//
		// If you wish to add new command-line options,
		// please add them to this section
		// Presently the code can only deal with strings and booleans

		// Required Command-Line Arguments

		/// <summary>
		/// This will contain the fully-qualified path to the
		/// Visual Studio solution.
		/// </summary>
		[RequiredArgument]
        [IsExistingFolder]
		public CommandLineOption<string> SolutionPath { get; set; }

		/// <summary>
		/// This will contain the fully-qualified path to the
		/// Visual Studio solution web project
		/// </summary>
		[RequiredArgument]
        [IsExistingFolder]
        public CommandLineOption<string> WebProjectPath { get; set; }

		/// <summary>
		/// This will contain the name of the menu file
		/// Example: "PMMenuDetails.xml"
		/// </summary>
        [RequiredArgument]
        public CommandLineOption<string> MenuFilename { get; set; }

		/// <summary>
		/// This will contain the name of the Visual Studio solution
		/// build profile.
		/// </summary>
        [RequiredArgument]
        public CommandLineOption<string> BuildProfile { get; set; }

		/// <summary>
		/// This will contain the fully-qualified path to the 
		/// Microsoft .NET Framework
		/// </summary>
        [RequiredArgument]
        [IsExistingFolder]
        public CommandLineOption<string> DotNetFrameworkPath { get; set; }

        /// <summary>
        /// This will contain a comma separated list of language specifiers
        /// that need to be handled in addition to the four default languages
        /// </summary>
        [OptionalArgument]
        public CommandLineOption<string> ExtraResourceLanguages { get; set; }

        /// <summary>
        /// This represents the mode that the application will
        /// be run in.
        /// </summary>
        [RequiredArgument]
	    public CommandLineOption<int> Mode { get; set; }

		// Optional Command-Line Arguments

		/// <summary>
		/// This will determine whether or not the minification process
		/// will run.
		/// </summary>
		[OptionalArgument]
        public CommandLineOption<bool> Minify { get; set; }

		/// <summary>
		/// This will determine whether or not the actual deployment
		/// to the live Sage 300 installation will occur.
		/// </summary>
        [OptionalArgument]
        public CommandLineOption<bool> NoDeploy { get; set; }

		/// <summary>
		/// This will determine whether or not files are actually
		/// copied during the deployment phase. If enabled,
		/// no actual files or folders will be copied. This is
		/// for testing purposes only.
		/// </summary>
        [OptionalArgument]
        public CommandLineOption<bool> TestDeploy { get; set; }

		/// <summary>
		/// This will determine whether or not the application
		/// will log messages and errors to a log file
		/// </summary>
        [OptionalArgument]
        public CommandLineOption<bool> Log { get; set; }

		#endregion

		#region Constructor(s)
		/// <summary>
		/// Empty constructor for unit testing purposes only
		/// </summary>
		public CommandLineOptions()
		{
			// Empty constructor for unit testing purposes
		}

        /// <summary>
        /// The primary constructor
        /// </summary>
        /// <param name="appName">The name of the application</param>
        /// <param name="appVersion">The version number of the application</param>
        /// <param name="appCopyright">The copyright information for the application</param>
		/// <param name="buildDate">The string representation of the build date and time</param>
		/// <param name="buildYear">The string representation of the build year</param>
        /// <param name="args">The argument list passed in via the command-line</param>
        /// <param name="prefix">Optional: The prefix string used when specifying command-line arguments</param>
        public CommandLineOptions(string appName, 
                                  string appVersion, 
                                  string appCopyright, 
                                  string buildDate, 
                                  string buildYear, 
                                  string[] args, 
                                  string prefix=DEFAULT_PREFIX)
        {
            OptionPrefix = prefix;
            ApplicationName = appName;
            ApplicationVersion = appVersion;
            ApplicationCopyright = appCopyright;
            ApplicationBuildDate = buildDate;
            ApplicationBuildYear = buildYear;

			// If the argument array has only a single entry, then the
			// arguments list will likely have /r/n characters in it
			// so we need to replace these with spaces and create a 
			// new arguments array
			rawArgList = args;
			cleanArgList = CleanupArguments(args);

            LoadErrors = new List<string>();

			// Initialize the CommandLineOptions
			#region Create CommandLineOptions

			SolutionPath = new CommandLineOption<string>()
			{
				Name = "solutionpath",
				AliasList = new List<string>() { "s", "sol", "sp", "solution" },
				Description = Messages.Msg_MicrosoftVisualStudioSolutionPath,
				OptionValue = "",
                ExampleValue = @"<path>"
            };
			LoadOption(SolutionPath, cleanArgList);

			WebProjectPath = new CommandLineOption<string>() 
            { 
                Name = "webprojectpath", 
                AliasList = new List<string>() {"p", "wpp", "projectpath"},
				Description = Messages.Msg_MicrosoftVisualStudioSolutionWebProjectPath,
                OptionValue = "",
                ExampleValue = @"<path>"
            };
	        LoadOption(WebProjectPath, cleanArgList);

			MenuFilename = new CommandLineOption<string>() 
            { 
                Name = "menufilename",
                AliasList = new List<string>() { "m", "menu", "menufile" },
                Description = Messages.Msg_Sage300MenuDefinitionFileName, 
                OptionValue = "",
                ExampleValue = @"<name>"
            };
			LoadOption(MenuFilename, cleanArgList);

			BuildProfile = new CommandLineOption<string>() 
            { 
                Name = "buildprofile",
                AliasList = new List<string>() { "b", "bp" },
				Description =  Messages.Msg_VisualStudioProjectBuildConfiguration,
                OptionValue = "Release",
                ExampleValue = @"<name>"
            };
	        LoadOption(BuildProfile, cleanArgList);

			DotNetFrameworkPath = new CommandLineOption<string>() 
            { 
                Name = "dotnetframeworkpath",
                AliasList = new List<string>() { "f", "dotnet", "dotnetframework", "netframework", "framework" },
				Description = Messages.Msg_NetFrameworkPathContainingAspnetCompileDotExe,
                OptionValue = "",
                ExampleValue = @"<path>"
            };
	        LoadOption(DotNetFrameworkPath, cleanArgList);

	        Mode = new CommandLineOption<int>()
	        {
		        Name = "mode",
		        AliasList = new List<string>() { "md" },
		        Description = Messages.Msg_ApplicationModeOption,
		        OptionValue = 0,
		        ExampleValue = @"0"
	        };
	        LoadOption(Mode, cleanArgList);

			Minify = new CommandLineOption<bool>() 
            { 
                Name = "minify",
                AliasList = new List<string>() { "min" },
                Description = Messages.Msg_MinifyJavascriptFiles,
                OptionValue = false,
                ExampleValue = @""
            };
	        LoadOption(Minify, cleanArgList);

			NoDeploy = new CommandLineOption<bool>() 
            { 
                Name = "nodeploy",
                AliasList = new List<string>() { "nd" },
                Description = Messages.Msg_DoNotCopyAssetsToSage300installationDirectory,
                OptionValue = false,
                ExampleValue = @""
            };
	        LoadOption(NoDeploy, cleanArgList);

			TestDeploy = new CommandLineOption<bool>()
            {
                Name = "testdeploy",
                AliasList = new List<string>() { "td" },
                Description = Messages.Msg_SimulateCopyingOfAssetsTo,
                OptionValue = false,
                ExampleValue = @""
            };
	        LoadOption(TestDeploy, cleanArgList);

			Log = new CommandLineOption<bool>()
            {
                Name = "log",
                AliasList = new List<string>() { "logging" },
				Description = Messages.Msg_GenerateALogFileInTheCurrentWorkingFolder,
                OptionValue = false,
                ExampleValue = @""
            };
	        LoadOption(Log, cleanArgList);

            ExtraResourceLanguages = new CommandLineOption<string>()
            {
                Name = "extraresourcelanguages",
                AliasList = new List<string>() { "extralanguages" },
                Description = Messages.Msg_ExtraResourceLanguages,
                OptionValue = "",
                ExampleValue = @"fr-CA"
            };
            LoadOption(ExtraResourceLanguages, cleanArgList);

            #endregion

            this.UsageMessage = BuildUsageMessage();
        }
		#endregion

		#region Private Methods

		/// <summary>
		/// Ensure that the command-line arguments array is well-formed
		/// If the parameters passed into the program contain
		/// NewLine (\n\r) characters, we need to get rid of them
		/// and rebuild the arguments array before we can use it.
		/// </summary>
		/// <param name="argsIn">The original arguments array</param>
		/// <returns>A well-formed arguments array</returns>
		private string[] CleanupArguments(string[] argsIn)
		{
			var tempArgList = new List<string>();
			var argList = new List<string>();

			// First, replace all NewLine characters, if they exist
			// in any of the array entries.
			// The array may be 1 or more in size
			foreach (var s in argsIn)
			{
				var temp = s.Replace(Environment.NewLine, SINGLE_SPACE);
				tempArgList.Add(temp);
			}

			// Next, go through each entry, split up on spaces
			// and add each piece to the final argument list
			foreach (var s in tempArgList)
			{
				if (s.Trim().Length > 0)
				{
					var itemSeparator = new String[] { $" {DEFAULT_PREFIX}" };
					var tempList = s.Split(itemSeparator, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
					if (tempList.Count > 1)
					{
						// Likely the command-line options were separated by new line characters so 
						// they showed up as a single command-line entry.
						foreach (string entry in tempList)
						{
							var trimmed = entry.Trim();
							if (trimmed.Substring(0, 2).ToLowerInvariant() != DEFAULT_PREFIX)
							{
								argList.Add($"{DEFAULT_PREFIX}{trimmed}");
							}
							else
							{
								argList.Add($"{trimmed}");
							}
						}
					}
					else
					{
						// Just add the entry and move on to the next one.
						argList.Add(tempList[0]);
					}
				}
			}

			// Finally, convert to a simple array and return!
			return argList.ToArray();
		}

	    /// <summary>
	    /// Look through all of the command-line arguments to
	    /// determine if there is an entry applicable to
	    /// assignment to the option variable passed into the function.
	    /// This method handles string, integer and boolean based 
	    /// command-line arguments. 
	    /// </summary>
	    /// <param name="option">A dynamic type based on CommandLineOption object</param>
	    /// <param name="args">The cleaned-up command-line options</param>
		private void LoadOption(dynamic option, string[] args)
	    {
		    var theArg = string.Empty;

			try
			{
				if (Array.Exists(args, s =>
				{
					theArg = s;

					// Split the prefix+flag and the actual value
					var optionName = GetArgumentNameOnly(theArg.Replace(Environment.NewLine, String.Empty));

					// Check the regular name
					if (optionName == option.Name)
						return true;

					// Check any Alias'
					if (option.AliasList.Contains(optionName))
						return true;

					return false;
				}))
				{
					// Process based on type
					if (option.GetType() == typeof(CommandLineOption<string>))
					{
						_ProcessString(option, theArg);
					}
					else if (option.GetType() == typeof(CommandLineOption<int>))
					{
						_ProcessNumber(option, theArg);
					}
					else if (option.GetType() == typeof(CommandLineOption<bool>))
					{
						_ProcessBoolean(option, theArg);
					}
				}
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(args.Length.ToString());
				Console.WriteLine($"{option.Name}={option.OptionValue}");
			}
		}

		/// <summary>
		/// This method will process a single string 
		/// command-line argument.
		/// </summary>
		/// <param name="option">The CommandLineOption object (string)</param>
		/// <param name="theArg">The single command-line argument</param>
		private void _ProcessString(CommandLineOption<string> option, string theArg)
		{
			var valueFromArg = theArg.Split('=')[1];

			var isRequired = IsPropertyMarkedAsRequired(option);
			if (isRequired)
			{
				option.LoadError = valueFromArg.Length == 0 ? true : false;
			}
			else
			{
				option.LoadError = false;
			}

			if (option.LoadError)
			{
				var msg = string.Format(Messages.Error_ErrorParsingOptionNoValueWasSet, 
										new String(' ', 10), 
										OptionPrefix + option.Name);
				LoadErrors.Add(msg);
			}


			// Now, if this property is marked with the [IsExistingFolder] attribute,
			// ensure that the value is an actual existing folder.
			if (IsPropertyMarkedAsExistingFolder(option))
			{
				// Only do this if the value of the argument 
				// has a length > 0. 
				// No sense in checking for a directory of zero length.
				// Zero length arguments were handled in the previous
				// code block :)
				if (valueFromArg.Length > 0)
				{
					var folder = valueFromArg.Trim();
					if (!Directory.Exists(folder))
					{
						// Error: folder doesn't exist
						option.LoadError = true;
						var msg = string.Format(Messages.Error_ErrorParsingOptionTheFolderDoesNotExist, 
												new String(' ', 10), 
												OptionPrefix + option.Name, 
												folder);
						LoadErrors.Add(msg);
					}
				}
			}

			option.OptionValue = valueFromArg;
		}

	    /// <summary>
	    /// This method will process a single number (integer) 
	    /// command-line argument.
	    /// </summary>
	    /// <param name="option">The CommandLineOption object (int)</param>
	    /// <param name="theArg">The single command-line argument</param>
	    private void _ProcessNumber(CommandLineOption<int> option, string theArg)
	    {
		    var valueFromArg = theArg.Split('=')[1];

		    var isRequired = IsPropertyMarkedAsRequired(option);
		    if (isRequired)
		    {
			    option.LoadError = valueFromArg.Length == 0 ? true : false;
		    }
		    else
		    {
			    option.LoadError = false;
		    }

		    if (option.LoadError)
		    {
			    var msg = string.Format(Messages.Error_ErrorParsingOptionNoValueWasSet,
				    new String(' ', 10),
				    OptionPrefix + option.Name);
			    LoadErrors.Add(msg);
		    }

		    option.OptionValue = Convert.ToInt32(valueFromArg);
	    }

		/// <summary>
		/// This method will process a single boolean
		/// command-line argument.
		/// </summary>
		/// <param name="option">The CommandLineOption object (boolean)</param>
		/// <param name="theArg">The single command-line argument</param>
		private void _ProcessBoolean(CommandLineOption<bool> option, string theArg)
		{
			// Since this is a boolean flag we only care that it's defined
			// It doesn't need to have any kind of value
			option.OptionValue = true;
			option.LoadError = false;
		}

		/// <summary>
		/// Gets the name of the command-line argument 
		/// without any prefix characters or assigned values
		/// Example: 
		///     Input:   --argumentname=blahblah
		///     Output:  argumentname
		/// </summary>
		/// <param name="arg">The individual argument</param>
		/// <returns>The argument name only</returns>
		private string GetArgumentNameOnly(string arg)
		{
			var temp = arg.Split('=')[0];
			if (OptionPrefix.Length > 0)
			{
				temp = temp.Substring(OptionPrefix.Length, temp.Length - OptionPrefix.Length);
			}
			return temp;
		}

		/// <summary>
		/// Build a text block describing how to run this program!
		/// </summary>
		/// <returns>A string representation of the usage instructions</returns>
		private string BuildUsageMessage()
		{
			var requiredParams = GetRequiredPropertiesAsString();
			var optionalParams = GetOptionalPropertiesAsString();
            var required3rdPartyComponents = "WebGrease.dll, WG.exe, Newtonsoft.Json.dll, Antlr3.Runtime.dll";
			var msg = divider + Environment.NewLine;
			msg += string.Format(Messages.Msg_ProgramUsageMessage, ApplicationName,
																   ApplicationVersion,
                                                                   ApplicationBuildDate,
                                                                   ApplicationCopyright,
																   required3rdPartyComponents,
																   requiredParams,
																   optionalParams,
                                                                   ApplicationBuildYear);
			msg += divider;
			return msg;
		}

		/// <summary>
		/// Interate through this class to find all properties marked with the
		/// RequiredArgument attribute. Once the list of properties is found,
		/// go through each and extract some values to build the line of text
		/// </summary>
		/// <returns>The required arguments as a string</returns>
		private string GetRequiredPropertiesAsString()
		{
			return GetPropertiesByAttributeAsString(new RequiredArgumentAttribute());
		}

		/// <summary>
		/// Interate through this class to find all properties marked with the
		/// OptionalArgument attribute. Once the list of properties is found,
		/// go through each and extract some values to build the line of text
		/// </summary>
		/// <returns>The optional arguments as a string</returns>
		private string GetOptionalPropertiesAsString()
		{
			return GetPropertiesByAttributeAsString(new OptionalArgumentAttribute());
		}

		/// <summary>
		/// Generic method to build a string of properties marked
		/// with a particular attribute.
		/// </summary>
		/// <param name="att">The attribute type to look for</param>
		/// <returns>A formatted string of text</returns>
		private string GetPropertiesByAttributeAsString(Attribute att)
		{
			var spacer5 = new string(' ', 5);
			var spacer2 = new string(' ', 2);

			var sb = new StringBuilder();
			var requiredProperties = this.
									 GetType().
									 GetProperties().
									 Where(x => x.GetCustomAttributes(att.GetType(), true).Any());

			foreach (var propertyInfo in requiredProperties)
			{
				dynamic valueSet = propertyInfo.GetValue(this, null);
				var name = valueSet.Name;
				var exampleValue = valueSet.ExampleValue;
				var description = valueSet.Description;
				List<String> aliasList = valueSet.AliasList;

				// Now build up the lines of text

				// Parameter Name
				sb.AppendLine($"{spacer2}{name}");

				// Blank line
				sb.AppendLine();

				// Alias'
				sb.AppendLine($"{spacer5}Alternate Names: {string.Join(", ", aliasList.ToArray())}");

				// Blank line
				sb.AppendLine();

				// Description - Need to ensure that long lines are broken
				// up into smaller ones.
				FormatDescriptionText(ref sb, description);

				// Blank line
				sb.AppendLine();

				// Example Usage
				if (exampleValue.Length > 0)
				{
					sb.AppendLine($"{spacer5}Example: {OptionPrefix}{name}=\"{exampleValue}\"");
				}
				else
				{
					sb.AppendLine($"{spacer5}Example: {OptionPrefix}{name}");
				}
				// Blank line
				sb.AppendLine();
			}
			return sb.ToString();
		}

		/// <summary>
		/// Process long description strings into smaller chunks on individual lines
		/// </summary>
		/// <param name="sb">The destination StringBuilder object</param>
		/// <param name="description">The description text</param>
		/// <param name="leftPadding">The number of spaces used for left padding on each line. Default value is 5</param>
		private void FormatDescriptionText(ref StringBuilder sb, string description, int leftPadding = 5)
		{
			const int MaxLineWidth = 60;
			var leftSpacer = new string(' ', leftPadding);
			string singleLine = string.Empty;
			var parts = description.Split(' ');
			foreach (var part in parts)
			{
				singleLine += $"{part} ";
				if (singleLine.Length >= MaxLineWidth)
				{
					sb.AppendLine($"{leftSpacer}{singleLine}");
					singleLine = string.Empty;
				}
			}
			sb.AppendLine($"{leftSpacer}{singleLine}");
		}

		/// <summary>
		/// How many required properties are defined in this class?
		/// Use the [RequiredArgument] attribute to determine this.
		/// </summary>
		/// <returns>The count of the required properties</returns>
		private int GetRequiredPropertiesCount()
		{
			Attribute att = new RequiredArgumentAttribute();
			var count = this.
						GetType().
						GetProperties().
						Where(x => x.GetCustomAttributes(att.GetType(), true).Any()).Count();
			return count;
		}

		/// <summary>
		/// Check to see if a property has been tagged with the 
		/// [IsExistingFolder] attribute
		/// </summary>
		/// <param name="option">The CommandLineOption to inspect</param>
		/// <returns>true : Property is marked with attribute, false : not marked with attribute</returns>
		private bool IsPropertyMarkedAsExistingFolder(dynamic option)
		{
			Attribute att = new IsExistingFolderAttribute();
			var propertiesMarkedAsFolders = this.
											GetType().
											GetProperties().
											Where(x => x.GetCustomAttributes(att.GetType(), true).Any());

			foreach (var propertyInfo in propertiesMarkedAsFolders)
			{
				dynamic valueSet = propertyInfo.GetValue(this, null);
				if (valueSet != null)
				{
					if (option.Name == valueSet.Name)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Check to see if a property has been tagged with the 
		/// [RequiredArgument] attribute
		/// </summary>
		/// <param name="option">The CommandLineOption to inspect</param>
		/// <returns>true : Property is marked with attribute, false : not marked with attribute</returns>
		private bool IsPropertyMarkedAsRequired(dynamic option)
		{
			var att = new RequiredArgumentAttribute();
			var propertiesMarkedAsRequired = this.
											 GetType().
											 GetProperties().
											 Where(x => x.GetCustomAttributes(att.GetType(), true).Any());

			foreach (var propertyInfo in propertiesMarkedAsRequired)
			{
				dynamic valueSet = propertyInfo.GetValue(this, null);
				if (valueSet != null)
				{
					if (option.Name == valueSet.Name)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Build a string containing the command-line argument name, an example value and description.
		/// This method is used with displaying the help text block.
		/// </summary>
		/// <param name="prefix">The prefix string used when passing in command-line arguments</param>
		/// <param name="name">The name of the command-line argument</param>
		/// <param name="exampleValue">The example value for this type of command-line argument</param>
		/// <param name="description">The description of the command-line argument</param>
		/// <returns>A formatted string</returns>
		private string MakeFormattedLine(string prefix, string name, string exampleValue, string description)
		{
			const string lineTemplate = "{0}{1,-30}{2}";
			var text = prefix + name;
			text += exampleValue.Length > 0 ? "=" + exampleValue : string.Empty;
			return String.Format(lineTemplate, new String(' ', 5), text, description);
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Were there any errors during startup (Loading of arguments)
		/// </summary>
		/// <returns>true = Errors occurred | False = No errors</returns>
		public bool AnyErrors()
        {
			// How many defined required properties are there?
			var requiredFieldCount = GetRequiredPropertiesCount();

            return cleanArgList.Length < requiredFieldCount ||
                   SolutionPath.LoadError ||
                   WebProjectPath.LoadError ||
                   MenuFilename.LoadError ||
                   BuildProfile.LoadError ||
                   DotNetFrameworkPath.LoadError ||
                   Minify.LoadError ||
                   NoDeploy.LoadError;
        }

        /// <summary>
        /// Build a displayable representation of any and all load error
        /// that occurred.
        /// </summary>
        /// <returns>A string representation of all the errors encountered</returns>
        public string GetLoadErrorsAsText()
        {
            var text = string.Empty;
            foreach (var s in LoadErrors)
            {
                text += s + Environment.NewLine;
            }
            return text;
        }

		#endregion
	}
}
