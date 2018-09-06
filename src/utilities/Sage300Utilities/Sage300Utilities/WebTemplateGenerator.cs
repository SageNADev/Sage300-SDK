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
using Sage300Utilities.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
#endregion

namespace Sage300Utilities
{
	/// <summary>
	/// This class is used to generate a visual studio vstemplate file
	/// based on the contents of a directory
	/// </summary>
	public class WebTemplateGenerator
	{
		#region Private Constants
		private static class Constants
		{
			public const string ApplicationIdPlaceholder = "$applicationid$";
            public const string LowercaseApplicationIdPlaceholderLowerCase = "$lowercaseapplicationid$";
			public const string NamespaceString = @"http://schemas.microsoft.com/developer/vstemplate/2005";
            public const string ExternalContentFolderName = "ExternalContent";
            public const string ModuleID = "TU";
		}
		#endregion

		#region Private Variables
		private ILogger _Logger;
		private XNamespace DocumentNamespace = Constants.NamespaceString;
		#endregion

		#region Public Properties
		public string WebTemplateFileName { get; set; }
		public string RootFolder { get; set; }
		public string TargetFolder { get; set; }
		#endregion

		#region Private Properties
		private bool ProcessingAreasVendorFolder { get; set; }
		private string FullPathToAreasVendorFolder { get; set; }
		#endregion

		#region Constructor(s)
		/// <summary>
		/// The primary constructor
		/// </summary>
		/// <param name="logger">An instance of the logger object</param>
		public WebTemplateGenerator(ILogger logger)
		{
			_Logger = logger;

			// This property will be set to true when we are processing 
			// the Areas\ModuleID\ folder.
			// For example: Areas\TU\
			// Some special processing is necessary when processing this folder
			ProcessingAreasVendorFolder = false;
		}
		#endregion

		#region Public Methods
		public string GenerateXML()
		{
			var doc = GenerateDocument();
			return doc.ToString();
		}

		/// <summary>
		/// Generate the In-memory representation of the XML document
		/// </summary>
		/// <returns></returns>
		public XDocument GenerateDocument()
		{
			DirectoryInfo dir = new DirectoryInfo(RootFolder);
			var rootXml = CreateRootXml();
			var templateContent = CreateTemplateContent();
			var projectXml = CreateProjectXml(dir);
			templateContent.Add(projectXml);
			rootXml.Add(templateContent);
			return new XDocument(rootXml);
		}

		/// <summary>
		/// Save the XML document to a file
		/// </summary>
		public void SaveXml()
		{
			var doc = GenerateDocument();
			var fullPath = Path.Combine(TargetFolder, WebTemplateFileName);
			doc.Save(fullPath);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Create the root XML element
		/// </summary>
		/// <returns>The XElement object</returns>
		private XElement CreateRootXml() 
		{
			// Create the root element
			var root = new XElement(this.DocumentNamespace + "VSTemplate", new XAttribute("Version", "3.0.0"),
															  new XAttribute("Type", "Project"));

			// Create and add the TemplateData element
			var xmlTemplateData = new XElement(this.DocumentNamespace + "TemplateData");
			AddTemplateDataNodes(xmlTemplateData);
			root.Add(xmlTemplateData);

			return root;
		}

		/// <summary>
		/// Create the TemplateContent element
		/// </summary>
		/// <returns>The XElement object</returns>
		private XElement CreateTemplateContent()
		{
			return new XElement(this.DocumentNamespace + "TemplateContent");
		}

		/// <summary>
		/// Add the nodes to the TemplateData element
		/// </summary>
		/// <param name="templateNode">The element to add the items to</param>
		private void AddTemplateDataNodes(XElement templateNode)
		{
			templateNode.Add(new XElement(this.DocumentNamespace + "Name",						"Web"));
			templateNode.Add(new XElement(this.DocumentNamespace + "Description",				"Web Project Template"));
			templateNode.Add(new XElement(this.DocumentNamespace + "ProjectType",				"CSharp"));
			templateNode.Add(new XElement(this.DocumentNamespace + "Hidden",					"true"));
			templateNode.Add(new XElement(this.DocumentNamespace + "ProjectSubType",			string.Empty));
			templateNode.Add(new XElement(this.DocumentNamespace + "SortOrder",					"1000"));
			templateNode.Add(new XElement(this.DocumentNamespace + "CreateNewFolder",			"true"));
			templateNode.Add(new XElement(this.DocumentNamespace + "DefaultName",				"Web"));
			templateNode.Add(new XElement(this.DocumentNamespace + "ProvideDefaultName",		"true"));
			templateNode.Add(new XElement(this.DocumentNamespace + "LocationField",				"Enabled"));
			templateNode.Add(new XElement(this.DocumentNamespace + "EnableLocationBrowseButton","true"));
			templateNode.Add(new XElement(this.DocumentNamespace + "Icon",						"__TemplateIcon.ico"));
		}
		
		/// <summary>
		/// Create the Project element and fill it based on the directory being passed in
		/// </summary>
		/// <param name="dir">The directory to enumerate looking for project folders and files</param>
		/// <returns>The fully populated Project element</returns>
		private XElement CreateProjectXml(DirectoryInfo dir)
		{
			var xmlInfo = new XElement(this.DocumentNamespace + "Project", 
									   new XAttribute("TargetFileName",		"TPA.Web.csproj"),
									   new XAttribute("File",				"TPA.Web.csproj"),
									   new XAttribute("ReplaceParameters",	"true"));

			// Get all the files in the root of the directory first
			foreach (var file in dir.GetFiles())
			{
				AddProjectItem(xmlInfo, dir.Name, file.Name);
			}

			IEnumerable<DirectoryInfo> subdirectories = from eachDir in dir.GetDirectories()
														orderby eachDir.FullName descending
														select eachDir;
			foreach (var subDir in subdirectories)
			{
				xmlInfo.Add(CreateSubdirectoryXML(subDir));
			}

			return xmlInfo;
		}

		/// <summary>
		/// Process subdirectories and create 'Folder' elements
		/// This method is called recursively to process further subdirectories
		/// </summary>
		/// <param name="dir">The current directory</param>
		/// <returns>The XElement 'Folder' element</returns>
		private XElement CreateSubdirectoryXML(DirectoryInfo dir)
		{
			var name = dir.Name;

			var nameUpper = name.ToUpperInvariant();
			var fullName = dir.FullName;
			var targetFolderName = name;

			if (nameUpper == Constants.ModuleID)
			{
				// Special processing section
				this.FullPathToAreasVendorFolder = fullName;
				targetFolderName = WebTemplateGenerator.Constants.ApplicationIdPlaceholder;
			}

			// Get directories
			var xmlInfo = new XElement(this.DocumentNamespace + "Folder", 
									   new XAttribute("Name", name),
									   new XAttribute("TargetFolderName", targetFolderName));

			// Process the files in this directory first
			foreach (var file in dir.GetFiles())
			{
				AddProjectItem(xmlInfo, dir.Name, file.Name);
			}

			// Now process the subdirectories in this directory
			var subdirectories = dir.GetDirectories().ToList();//.OrderBy(d => d.Name);
			if (nameUpper == Constants.ModuleID)
			{
				// Special folder processing

				// Check for the existence of these three folders.
				// Add them if they don't exist

				// Create the '\Areas\TU\Controllers\' folder (only if it doesn't yet exist)
				var newFolderPath = Path.Combine(this.FullPathToAreasVendorFolder, "Controllers");
				DirectoryInfo di = null;
				di = new DirectoryInfo(newFolderPath);
				if (!di.Exists) { di.Create(); }

				// Create the '\Areas\TU\Models\' folder (only if it doesn't yet exist)
				newFolderPath = Path.Combine(this.FullPathToAreasVendorFolder, "Models");
				di = new DirectoryInfo(newFolderPath);
				if (!di.Exists) { di.Create(); }

				// Create the '\Areas\TU\Scripts\' folder (only if it doesn't yet exist)
				newFolderPath = Path.Combine(this.FullPathToAreasVendorFolder, "Scripts");
				di = new DirectoryInfo(newFolderPath);
				if (!di.Exists) { di.Create(); }

				// Now, rebuild the list so it's consistent
				subdirectories = dir.GetDirectories().ToList();
			}
			else if (nameUpper == "CONTROLLERS")
			{
				// Create the '\Areas\TU\Controllers\Finder\' folder
				var newFolderPath = Path.Combine(this.FullPathToAreasVendorFolder, @"Controllers");
				newFolderPath = Path.Combine(newFolderPath, "Finder");
				DirectoryInfo di = null;
				di = new DirectoryInfo(newFolderPath);
				if (!di.Exists) { di.Create(); }

				// Now, rebuild the list so it's consistent
				subdirectories = dir.GetDirectories().ToList();
			}

			foreach (var subDir in subdirectories)
			{
				xmlInfo.Add(CreateSubdirectoryXML(subDir));
			}

			return xmlInfo;
		}

        /// <summary>
        /// Add a ProjectItem element with special substitution for certain files
        /// Note: There are lists of filenames in the code below,
        ///       that will have placeholder text inserted into.
        /// </summary>
        /// <param name="element">The element to add the new ProjectItem to.</param>
        /// <param name="directory">The directory were currently processing</param>
        /// <param name="currentFilename">The filename to use</param>
        private void AddProjectItem(XElement element, string directory, string currentFilename)
		{
            var currentDirectory = directory;
			var targetFileName = currentFilename;

			// List of files that can be ignored. This (or these)
			// are not part of the C# project. 
			// They are used when building the UI Wizard.
			// Since it's just a single file, we don't need
			// to write a loop. If we add more, we'll need a loop.
			// If the current file is in this list, just return.
			var filesToIgnore = new[] {
				"__TemplateIcon.ico"
			};
			if (currentFilename.ToUpperInvariant().Equals(filesToIgnore[0].ToUpperInvariant())) return;


			// Look for the following special files:
			// XXBootstrapper.xml
			// XXMenuDetails.xml
			// XXAreaRegistration.cs
			// XXWebBootstrapper.cs
			var filters = new[] {
					@"Bootstrapper.xml",
					@"MenuDetails.xml",
					@"AreaRegistration.cs",
					@"WebBootstrapper.cs",
			};

			// Check for any substitutions
			foreach (var f in filters)
			{
				if (f.ToUpperInvariant() == currentFilename.ToUpperInvariant().Substring(2, currentFilename.Length-2))
				{
					// Match found
					targetFileName = $"{WebTemplateGenerator.Constants.ApplicationIdPlaceholder}{f}";

					_Logger.LogInfo($"Substitution Match Found : Target filename will be renamed from '{currentFilename}' to '{targetFileName}'.");

					break;
				}
			}

            // 
            // Another couple of potential string replacements to deal with.
            // Note: This is applicable to the ExternalContent folder only.
            // If these files are elsewhere, their filename will NOT be altered.
            // Notes: Examples
            //        <ProjectItem ReplaceParameters="true" TargetFileName="bg_menu_$lowercaseapplicationid$.jpg">menuBackGroundImage.jpg</ProjectItem>
            //        <ProjectItem ReplaceParameters="true" TargetFileName="icon_$lowercaseapplicationid$.png">menuIcon.png</ProjectItem>
            var ph = WebTemplateGenerator.Constants.LowercaseApplicationIdPlaceholderLowerCase;
            var filters2 = new[] {
                new[] { @"menuBackGroundImage.jpg", @"bg_menu_" + ph + ".jpg" },
                new[] { @"menuIcon.png",            @"icon_" + ph + ".png" },
            };

            var replaceParametersFlagOverride = false;
            for (int i = 0; i < filters2.Length; i++)
            {
                var file = filters2[i][0];

                var fileMatch = currentFilename.Equals(file, System.StringComparison.InvariantCultureIgnoreCase);
                var directoryMatch = currentDirectory.Equals(Constants.ExternalContentFolderName, System.StringComparison.InvariantCultureIgnoreCase);
                var match = fileMatch && directoryMatch;
                if (match)
                {
                    // Found a match. Assign the replacement filename
                    targetFileName = filters2[i][1];
                    _Logger.LogInfo($"Substitution Match Found : Current Directory = '{currentDirectory}', Target filename will be renamed from '{currentFilename}' to '{targetFileName}'.");
                    replaceParametersFlagOverride = true;
                    break;
                }
            }

            // Set the ReplaceParameters attribute based on file extension
            // Files of these types will be set to ReplaceParameters=false
            // otherwise they'll be set to ReplaceParameters=true

            var extensionList = new[] {
					// Executable Formats
					"EXE", "DLL",

					// Font Types
					"EOT", "TTF", "WOFF", "WOFF2", "OTF", 

					// Graphics Formats
					"SVG", "GIF", "PNG", "JPG", "ICO",

					// CSS related
					"LESS",  

					// Microsoft Excel format
					"XLSX"
			};

			var extension = new FileInfo(currentFilename).Extension.RemoveFirstCharacter().ToUpperInvariant();
			bool replaceParameters = extensionList.Contains(extension) ? false : true;

            // We need to override the replaceParameters flag when dealing with the two special files in 'ExternalContent' above
            // menuBackgroundImage.jpg and menuIcon.png
            if (replaceParametersFlagOverride == true)
            {
                replaceParameters = true;
            }

			var replaceParametersString = replaceParameters.ToString().ToLower();
            element.Add(new XElement(this.DocumentNamespace + "ProjectItem", 
									 new XAttribute("ReplaceParameters", replaceParametersString),
									 new XAttribute("TargetFileName", targetFileName),
									 currentFilename));
		}
#endregion
	}
}

// EOF
