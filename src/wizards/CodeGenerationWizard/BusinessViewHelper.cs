// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
    #endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary>
    /// Static class to assist with class generation
    /// </summary>
    public static class BusinessViewHelper
    {
        #region Private Enumeration(s)
        private enum MethodSignatureEnum
        {
            RegisterController = 0,
            RegisterFinder = 1,
            RegisterExportImportController = 2
        }
        #endregion

        #region Private Constants
        private struct Constants
        {
            public const string TabTwo = "\t\t";
            public const string TabThree = "\t\t\t";
            public const string TabFour = "\t\t\t\t";
            public const string BootstrapperFilenameBase = "Bootstrapper.cs";
            public const string WebBootstrapperFilenameBase = "WebBootstrapper.cs";
            public const int NotFoundInList = -1;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateFlatBootStrappers(BusinessView view, Settings settings)
        {
            // Get the flags
            var generateClientFiles = view.Options[BusinessView.Constants.GenerateClientFiles];
            var generateFinder = view.Options[BusinessView.Constants.GenerateFinder];

            if (generateClientFiles || generateFinder) { UpdateWebBootStrapperNamespaces(view, settings); }

            if (generateClientFiles)
            {
                if (view.Options[BusinessView.Constants.GenerateGrid])
                {
                    UpdateHeaderDetailBootStrappers(view, settings);
                }
                else
                {
                    UpdateWebBootStrapper(view, settings);
                    UpdateBootStrapper(view, settings);
                }
            }
        }

        public static void UpdateHeaderDetailBootStrappers(BusinessView view, Settings settings)
        {
            UpdateHeaderDetailWebBootStrapper(view, settings);
            UpdateHeaderDetailModuleBootStrapper(view, settings);
        }

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateProcessBootStrappers(BusinessView view, Settings settings)
        {
            if (view.Options[BusinessView.Constants.GenerateClientFiles])
            {
                UpdateProcessWebBootStrapper(view, settings);
            }

            UpdateProcessBootStrapper(view, settings);
        }

        public static void UpdateReportBootStrappers(BusinessView view, Settings settings)
        {
            if (view.Options[BusinessView.Constants.GenerateClientFiles])
            {
                UpdateReportWebBootStrapper(view, settings);
            }

            UpdateReportBootStrapper(view, settings);
        }

        /// <summary>
        /// Update Bundles
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateBundles(BusinessView view, Settings settings)
        {
            // Locals
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = (settings.RepositoryType.Equals(RepositoryType.HeaderDetail)) ? settings.EntitiesContainerName : view.Properties[BusinessView.Constants.EntityName];
            var projectInfoWeb = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId];
            var pathProj = projectInfoWeb.ProjectFolder;
            var bundleFile = Path.Combine(pathProj, "BundleRegistration.cs");
            var scriptBase = settings.CompanyNamespace;
            var entity = string.Format(@"#region {0}", entityName);

            // Tag for insertion point in BundleRegistration.cs
            const string tag = "internal static void RegisterBundles(BundleCollection bundles)";

            var textlineToAdded = string.Format(Constants.TabThree + @"#region {0}" + "\r\n" +
                                                Constants.TabThree + @"bundles.Add(new Bundle(""~/bundles/{2}{1}{0}"").Include(" + "\r\n" +
                                                Constants.TabFour + @"""~/Areas/{1}/Scripts/{0}/{2}.{1}.{0}Behaviour.js""," + "\r\n" +
                                                Constants.TabFour + @"""~/Areas/{1}/Scripts/{0}/{2}.{1}.{0}KoExtn.js""," + "\r\n" +
                                                Constants.TabFour + @"""~/Areas/{1}/Scripts/{0}/{2}.{1}.{0}Repository.js""," + "\r\n" +
                                                Constants.TabFour + @"""~/Areas/{1}/Scripts/FinderDef.js""," + "\r\n" +
                                                Constants.TabFour + @"""~/Areas/Core/Scripts/Process/Sage.CA.SBS.Sage300.Common.Process.js""));" + "\r\n" +
                                                Constants.TabThree + @"#endregion" + "\r\n", entityName, moduleId, scriptBase);

            // Proceed if the file exists
            if (File.Exists(bundleFile))
            {
                var txtLines = File.ReadAllLines(bundleFile).ToList();
                var trimLines = (File.ReadAllLines(bundleFile)).Select(l => l.Trim()).ToList();
                var index = trimLines.IndexOf(tag);


                // Proceed if insertion point exists
                if (index > Constants.NotFoundInList)
                {
                    // Only add if not already added
                    if (trimLines.IndexOf(entity) == Constants.NotFoundInList)
                    {
                        var pos = index + 2;
                        txtLines.Insert(pos, textlineToAdded);
                        File.WriteAllLines(bundleFile, txtLines);
                    }
                }
            }
        }

        /// <summary>
        /// Update Flat Constants
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateConstants(BusinessView view, Settings settings, bool useContainerName = false)
        {
            // Locals
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = useContainerName ? settings.EntitiesContainerName : view.Properties[BusinessView.Constants.EntityName];
            var projectInfoWeb = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId];
            var pathProj = Path.Combine(projectInfoWeb.ProjectFolder, "Areas", moduleId, "Constants");
            var constantFile = Path.Combine(pathProj, "Constant.cs");
            string textToAdd = string.Empty;

            // Tag for insertion point in Constant.cs
            const string tag = "#region Partial Views";

            // Proceed if the file exists
            if (File.Exists(constantFile))
            {
                var txtLines = File.ReadAllLines(constantFile).ToList();
                var trimLines = (File.ReadAllLines(constantFile)).Select(l => l.Trim()).ToList();
                var index = trimLines.IndexOf(tag);

                // Proceed if insertion point exists
                if (index > Constants.NotFoundInList)
                {
                    // Iterate tab pages to create partial views
                    foreach (string tabPage in settings.Widgets["TabPage"])
                    {
                        var text = string.Format(Constants.TabTwo + @"/// <summary>" + "\r\n" +
                                                 Constants.TabTwo + @"/// {0}{2}" + "\r\n" +
                                                 Constants.TabTwo + @"/// </summary>" + "\r\n" +
                                                 Constants.TabTwo + @"public const string {0}{2} = ""~/Areas/{1}/Views/{0}/Partials/_{2}.cshtml"";" + "\r\n",
                                                 entityName, moduleId, tabPage);
                        var entity = string.Format(@"/// {0}{1}", entityName, tabPage);

                        // Only add if not already added
                        if (trimLines.IndexOf(entity) == Constants.NotFoundInList)
                        {
                            textToAdd += text;
                        }
                    }

                    // Insert
                    if (!string.IsNullOrEmpty(textToAdd))
                    {
                        txtLines.Insert(index + 2, textToAdd);
                    }
                }
                // Only need to write if we've added text
                if (!string.IsNullOrEmpty(textToAdd))
                {
                    File.WriteAllLines(constantFile, txtLines);
                }
            }
        }

        /// <summary>
        /// Update View Page URL
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void CreateViewPageUrl(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectFolder;
            var pageUrlFile = Path.Combine(pathProj, "pageUrl.txt");

            // {0} is the session id passed in from Global.asax.cs
            var pageUrl = "OnPremise/{0}/" + moduleId + "/" + (settings.RepositoryType.Equals(RepositoryType.HeaderDetail) ? settings.EntitiesContainerName : entityName);

            if (File.Exists(pageUrlFile))
            {
                File.Delete(pageUrlFile);
            }

            File.AppendAllLines(pageUrlFile, new[] { pageUrl });
        }

        /// <summary>
        /// Concat strings using Path object
        /// </summary>
        /// <param name="values">values to concatenate</param>
        /// <param name="changeToDot">True to replace separator with dot (namespaces)</param>
        /// <returns>Concatenated string</returns>
        public static string ConcatStrings(IEnumerable<string> values, bool changeToDot = false)
        {
            // Locals
            var retVal = values.Aggregate(string.Empty, Path.Combine);

            // Trim any begining slash
            if (retVal.StartsWith(@"\"))
            {
                retVal = retVal.Substring(1);
            }

            // Replace separator with dot?
            if (changeToDot)
            {
                retVal = retVal.Replace(@"\", ".");
            }

            return retVal;
        }

        /// <summary>
        /// Modify module level security const string class to add view security resource ID
        /// Will not insert duplicate entries
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateSecurityClass(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var pathBusinessRepository = settings.Projects[ProcessGeneration.Constants.BusinessRepositoryKey][moduleId].ProjectFolder;
            var filePath = Path.Combine(pathBusinessRepository, @"Security\Security.cs");
            var f = Environment.NewLine + Constants.TabTwo;
            var commentLine = f + "/// <summary>" + f + "/// Security resourceID for " + moduleId + " " + entityName + f + "/// </summary>" + Environment.NewLine;
            var constName = moduleId + entityName;
            var signature = "public const string " + constName + " = \"" + constName.ToUpper() + "\";";
            var constLine = f + signature + Environment.NewLine;

            if (File.Exists(filePath))
            {
                var txtLines = File.ReadAllLines(filePath).ToList();
                var trimLines = (File.ReadAllLines(filePath)).Select(l => l.Trim()).ToList();
                var index = trimLines.IndexOf(signature);
                if (index == Constants.NotFoundInList)
                {
                    // Line not found so let's insert it.

                    // Find the line of the first occurance of '}' character
                    // We want to insert the line just before this line.
                    index = trimLines.IndexOf("}");
                    if (index > Constants.NotFoundInList)
                    {
                        var pos = index - 3;
                        var linesToInsert = commentLine + constLine;

                        txtLines.Insert(pos, linesToInsert);
                        File.WriteAllLines(filePath, txtLines);
                    }
                }
            }
        }

        /// <summary>
        /// Helper method that removes and replaces unwanted characters
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Replaced string</returns>
        public static string Replace(string value)
        {
            if (value == string.Empty)
            {
                return string.Empty;
            }

            // Convert to Pascal Case First, but only if there are spaces in value (else it has already been done)
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var pascalCase = value.Contains(" ") ? textInfo.ToTitleCase(value) : value;

            var newString = pascalCase
                .Replace("Add'l", "Additional")
                .Replace("Addt'l", "Additional")
                .Replace("Ret'd", "Returned")
                .Replace("State/Prov.", "StateProvince")
                .Replace("Company/Org.", "CompanyOrganization")
                .Replace("Distrib.", "Distribution")
                .Replace("Insuff.", "Insufficient")
                .Replace("Prepay.", "Prepayment")
                .Replace("Unreal.", "Unrealized")
                .Replace("Alloc.", "Allocated")
                .Replace("Avail.", "Available")
                .Replace("Jrnls.", "Journals")
                .Replace("Quant.", "Quantity")
                .Replace("Reval.", "Revaluation")
                .Replace("Recon.", "Reconcilation")
                .Replace("Sched.", "Schedule")
                .Replace("Trans.", "Transaction")
                .Replace("Acct.", "Account")
                .Replace("Auth.", "Authority")
                .Replace("Calc.", "Calculation")
                .Replace("Curr.", "Currency")
                .Replace("Cust.", "Customer")
                .Replace("Desc.", "Description")
                .Replace("Dest.", "Destination")
                .Replace("Dist.", "Distribution")
                .Replace("Fisc.", "Fiscal")
                .Replace("Func.", "Functional")
                .Replace("incl.", "Included")
                .Replace("Exch.", "Exchange")
                .Replace("excl.", "Excluded")
                .Replace("Info.", "Information")
                .Replace("Lgst.", "Largest")
                .Replace("Larg.", "Largest")
                .Replace("Misc.", "Miscellaneous")
                .Replace("Orig.", "Original")
                .Replace("Prov.", "Provisional")
                .Replace("Rcpt.", "Receipt")
                .Replace("Rtng.", "Retainage")
                .Replace("Srce.", "Source")
                .Replace("Stats.", "Statistic")
                .Replace("Vend.", "Vendor")
                .Replace("Warr.", "Warranty")
                .Replace("Adj.", "Adjustment")
                .Replace("Amt.", "Amount")
                .Replace("Bal.", "Balance")
                .Replace("Chk.", "Check")
                .Replace("Clr.", "Clearing")
                .Replace("Cur.", "Currency")
                .Replace("Dep.", "Deposit")
                .Replace("Doc.", "Document")
                .Replace("Exp.", "Expense")
                .Replace("Fwd.", "Forward")
                .Replace("diff.", "Difference")
                .Replace("Inc.", "Include")
                .Replace("Inv.", "Invoice")
                .Replace("Int.", "Interest")
                .Replace("Opt.", "Optional")
                .Replace("Ord.", "Ordered")
                .Replace("Per.", "Period")
                .Replace("Qty.", "Quantity")
                .Replace("Rtg.", "Retainage")
                .Replace("Src.", "Source")
                .Replace("Sep.", "Separately")
                .Replace("Seq.", "Sequence")
                .Replace("Sys.", "System")
                .Replace("Cr.", "Credit")
                .Replace("Dr.", "Debit")
                .Replace("Ex.", "Exchange")
                .Replace("No.", "Number")
                .Replace("Pd.", "Paid")
                .Replace("Yr.", "Year")
                .Replace("C.C.", "CreditCard")
                .Replace("w/ ", "With")
                .Replace("w/o ", "Without")
                .Replace("->", "To")
                .Replace(" ", "")
                .Replace("/", "")
                .Replace(@"\", "")
                .Replace("*", "")
                .Replace("#", "")
                .Replace("-", "")
                .Replace(".", "")
                .Replace("'", "")
                .Replace(":", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace(",", "")
                .Replace(@"%", "")
                .Replace("&", "");

            if (newString.Length > 0)
            {
                var num = newString.ToArray()[0];
                if (char.IsNumber(num))
                {
                    newString = "Num" + newString;
                }
            }

            if (string.CompareOrdinal(newString, "OptionalFields") == 0)
            {
                return "NumberOfOptionalFields";
            }

            return newString;
        }

        /// <summary>
        /// Get the pural name for the entered value
        /// </summary>
        /// <param name="name">Name to be made plural</param>
        /// <returns>Plural name</returns>
        public static string PluralName(string name)
        {
            var pluralName = name;
            var lastSecond = name.ElementAt(name.Length - 2);

            // If already plural (best guess), then do nothing
            if (name.EndsWith("s"))
            {
                // Do nothing since it is already plural
            }
            else if (name.EndsWith("x") || name.EndsWith("z")
                || name.EndsWith("ch") || name.EndsWith("sh"))
            {
                pluralName = name + "es";
            }
            else if (name.EndsWith("y") &&
                (lastSecond != 'a' && lastSecond != 'e' && lastSecond != 'i' &&
                lastSecond != 'o' && lastSecond != 'u'))
            {
                pluralName = name.Substring(0, name.Length - 1) + "ies";
            }
            else if (!name.EndsWith("s"))
            {
                pluralName = name + "s";
            }

            return pluralName;
        }

        /// <summary>
        /// Update the Plugin Menu Details
        /// </summary>
        /// <param name="view"></param>
        /// <param name="settings"></param>
        public static void UpdateMenuDetails(BusinessView view, Settings settings)
        {
            // Locals
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectFolder;
            var menuFile = Path.Combine(pathProj, moduleId + "MenuDetails.xml");
            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(File.ReadAllText(menuFile));
                var root = xmlDoc.DocumentElement;
                if (root != null)
                {
                    // Get menu items
                    var itemNodes = root.SelectNodes("item");
                    var screenUrl = moduleId + "/" + entityName;

                    // If there are menu items AND the menu does not already exist
                    if (itemNodes != null && !MenuExists(itemNodes, screenUrl))
                    {
                        var lastItemNode = itemNodes[itemNodes.Count - 1];
                        var itemNode = lastItemNode.Clone();

                        var node = itemNode.SelectSingleNode("MenuID");
                        var name = node.InnerText;
                        var updateName = name.Substring(0, 2) + (int.Parse(name.Substring(2)) + 1);
                        var key = moduleId + entityName;

                        node.InnerText = updateName;
                        node = itemNode.SelectSingleNode("MenuName");
                        node.InnerText = updateName;
                        node = itemNode.SelectSingleNode("ResourceKey");
                        node.InnerText = key;
                        node = itemNode.SelectSingleNode("IsGroupHeader");
                        node.InnerText = "false";
                        node = itemNode.SelectSingleNode("ScreenURL");
                        node.InnerText = screenUrl;
                        node = itemNode.SelectSingleNode("MenuItemOrder");
                        node.InnerText = (int.Parse(node.InnerText) + 1).ToString();
                        node = itemNode.SelectSingleNode("SecurityResourceKey");
                        node.InnerText = key;

                        root.InsertAfter(itemNode, lastItemNode);
                        xmlDoc.Save(menuFile);
                    }
                }
            }
            catch (Exception)
            {
                // do nothing, not throw exception to break application
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Does the menu already exist?
        /// </summary>
        /// <param name="items">List of menu items</param>
        /// <param name="url">menu URL</param>
        /// <returns>True if the menu exists otherwise false</returns>
        private static bool MenuExists(XmlNodeList items, string url)
        {
            // Local
            var exists = false;

            // Iterate items looking for url
            foreach (XmlNode item in items)
            {
                var screenUrl = item.SelectSingleNode("ScreenURL");
                if (screenUrl.InnerText.Equals(url))
                {
                    // Already exists. No need to look further
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        /// <summary>
        /// Register types for controller/ImportExport
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateWebBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectFolder;

            var filename = moduleId + Constants.WebBootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);
            if (File.Exists(filePath))
            {
                var pos = 1;

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                // Insert the Method signatures and bodies
                InsertMethods(entityName, moduleId, modelName, trimLines, ref txtLines, pos);

                // Write out the file changes
                File.WriteAllLines(filePath, txtLines);
            }
        }

        /// <summary>
        /// Update the namespaces
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateWebBootStrapperNamespaces(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var pathProj = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectFolder;
            var webProjNs = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.Constants.ModelsKey][moduleId].ProjectName;

            var filename = moduleId + Constants.WebBootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);
            if (File.Exists(filePath))
            {
                var pos = 1;

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                // Insert the 'using namespace...' lines
                InsertNamespaces(modelProjNs, webProjNs, moduleId, trimLines, ref txtLines, ref pos);

                // Write out the file changes
                File.WriteAllLines(filePath, txtLines);
            }
        }

        /// <summary>
        /// Create the two working List of strings</string>
        /// </summary>
        /// <param name="allLines">The array of strings</param>
        /// <param name="trimLines">The output List of strings (beginning and end whitespace removed from each line)</param>
        /// <param name="txtLines">The output List of strings</param>
        private static void MakeLists(string filepath, out List<string> trimLines, out List<string> txtLines)
        {
            var allLines = File.ReadAllLines(filepath);
            trimLines = allLines.Select(l => l.Trim()).ToList();
            txtLines = allLines.ToList();
        }

        /// <summary>
        /// Insert the namespace lines
        /// </summary>
        /// <param name="modelProjectNS">The model project namespace</param>
        /// <param name="webProjectNS">The web project namespace</param>
        /// <param name="moduleId">The module ID</param>
        /// <param name="trimLines">The List<String> of lines</param>
        /// <param name="txtLines">The working mutating buffer of text lines</param>
        /// <param name="currentPosition">The current index within the working mutating buffer</param>
        private static void InsertNamespaces(string modelProjectNS,
                                             string webProjectNS,
                                             string moduleId,
                                             List<string> trimLines,
                                             ref List<string> txtLines,
                                             ref int currentPosition)
        {
            string[] namespaces =
            {
                    "using " + modelProjectNS + ";" ,
                    "using " + webProjectNS + ".Areas." + moduleId + ".Controllers;",
                };

            for (var i = 0; i < namespaces.Length; i++)
            {
                if (trimLines.IndexOf(namespaces[i]) < 0)
                {
                    // Insert the namespace line only if it doesn't yet exist
                    txtLines.Insert(++currentPosition, namespaces[i]);
                }
            }
        }

        /// <summary>
        /// Insert the necessary methods
        /// </summary>
        /// <param name="entityName">The Entity name as a string</param>
        /// <param name="moduleId">The module ID as a string</param>
        /// <param name="modelName">The model name as a string</param>
        /// <param name="trimLines">The List of strings representing the lines in the file</param>
        /// <param name="txtLines">The mutable list of strings representing the lines in the file (start and end whitespace removed)</param>
        /// <param name="currentPosition">The current position in the list of strings as an integer</param>
        private static void InsertMethods(string entityName,
                                          string moduleId,
                                          string modelName,
                                          List<string> trimLines,
                                          ref List<string> txtLines,
                                          int currentPosition)
        {
            const string register = "\t\t\tUnityUtil.RegisterType";

            string[] methodSignatures =
            {
                @"private void RegisterController(IUnityContainer container)",
                @"private void RegisterExportImportController(IUnityContainer container)"
            };

            string[] linesToAdd =
            {
                string.Format(register + "<IController, {0}Controller<{2}>>(container, \"{1}{0}\");",
                              entityName, moduleId, modelName),
                string.Format(register + "<IExportImportController, {0}ControllerInternal<{3}>>(container, \"{1}{2}\", new InjectionConstructor(typeof(Context)));",
                              entityName, moduleId.ToLower(), entityName.ToLower(), modelName)
            };

            for (var i = 0; i < methodSignatures.Length; i++)
            {
                var lineToAdd = linesToAdd[i];
                var methodSignature = methodSignatures[i];
                var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                var insertionIndex = methodSignatureIndex + 1 + i + currentPosition;

                // Need to remove the tabs '\t' before doing the lookup.
                if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                {
                    // Line was not found so we will now insert it.
                    txtLines.Insert(insertionIndex, linesToAdd[i]);
                }
            }
        }

        /// <summary>
        /// Register types for controller/Finder/ImportExport
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateHeaderDetailWebBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectFolder;

            var webProjNs = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.Constants.ModelsKey][moduleId].ProjectName;

            var filename = moduleId + Constants.WebBootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);
            if (File.Exists(filePath))
            {
                var pos = 1;

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                string[] namespaces =
                {
                    "using " + modelProjNs + ";" ,
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers;",
                    "using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;",
                    "using " + settings.Projects[ProcessGeneration.Constants.InterfacesKey][moduleId].ProjectName + ".BusinessRepository;"
                };

                for (var i = 0; i < namespaces.Length; i++)
                {
                    if (trimLines.IndexOf(namespaces[i]) < 0)
                    {
                        txtLines.Insert(++pos, namespaces[i]);
                    }
                }

                string[] methodSignatures =
                {
                    @"private void RegisterController(IUnityContainer container)",
                    @"private void RegisterExportImportController(IUnityContainer container)"
                };

                const string register = "\t\t\tUnityUtil.RegisterType";

                var containerName = settings.RepositoryType.Equals(RepositoryType.HeaderDetail) ? settings.EntitiesContainerName : entityName;

                string[] linesToAdd =
                {
                    string.Format(register + "<IController, {0}Controller>(container, \"{1}{0}\");", containerName, moduleId),
                    string.Format(register + "<IExportImportController, ImportExportControllerInternal<I{0}Repository>>(container, \"{1}{2}\", new InjectionConstructor(typeof(Context)));", containerName, moduleId.ToLower(), containerName.ToLower())
                };

                for (var i = 0; i < methodSignatures.Length; i++)
                {
                    var lineToAdd = linesToAdd[i];
                    var methodSignature = methodSignatures[i];
                    var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                    var insertionIndex = methodSignatureIndex + i + pos + 1;

                    // Need to remove the tabs '\t' before doing the lookup.
                    if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                    {
                        // Line was not found so we will now insert it.
                        txtLines.Insert(insertionIndex, lineToAdd);
                    }
                }
                File.WriteAllLines(filePath, txtLines);
            }
        }

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateHeaderDetailModuleBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.ServicesKey][moduleId].ProjectFolder;
            var filename = moduleId + Constants.BootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);

            if (File.Exists(filePath))
            {
                var register = "\t\t\tUnityUtil.RegisterType";
                string methodSignature = @"private void RegisterService(IUnityContainer container)";

                var containerName = settings.RepositoryType.Equals(RepositoryType.HeaderDetail) ? settings.EntitiesContainerName : entityName;
                string lineToAdd = string.Format(register + "<I{0}Repository, {0}Repository>(container);", containerName, containerName);

                string[] namespaces =
                {
                    "using " + settings.CompanyNamespace + "." + moduleId + ".BusinessRepository;",
                    "using " +  settings.CompanyNamespace + "." + moduleId + ".Interfaces.BusinessRepository;"
                };

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                for (var i = 0; i < namespaces.Length; i++)
                {
                    if (trimLines.IndexOf(namespaces[i]) < 0)
                    {
                        txtLines.Insert(2, namespaces[i]);
                    }
                }

                var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                var insertionIndex = methodSignatureIndex + 2;

                // Need to remove the tabs '\t' before doing the lookup.
                if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                {
                    // Line was not found so we will now insert it.
                    txtLines.Insert(insertionIndex + 2, lineToAdd);
                }

                File.WriteAllLines(filePath, txtLines);
            }
        }

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.ServicesKey][moduleId].ProjectFolder;
            var filename = moduleId + Constants.BootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);

            //const int RegisterServiceMethodIndex = 0;
            const int RegisterRepositoriesMethodIndex = 1;

            if (File.Exists(filePath))
            {
                string[] methodSignatures =
                {
                    @"private void RegisterService(IUnityContainer container)",
                    @"private void RegisterRepositories(IUnityContainer container)",
                };

                var register = "\t\t\tUnityUtil.RegisterType";
                string[] linesToAdd =
                {
                    string.Format(register + "<Interfaces.Services.I{0}Service<Models.{1}>, {0}EntityService<Models.{1}>>(container);",entityName, modelName),
                    string.Format(register + "<IExportImportRepository, BusinessRepository.{2}Repository<Models.{3}>>(container, \"{1}{0}\", new InjectionConstructor(typeof(Context)));", entityName.ToLower(), moduleId.ToLower(), entityName, modelName),
                    string.Format(register + "(container, typeof(Interfaces.BusinessRepository.I{0}Entity<Models.{1}>), typeof(BusinessRepository.{0}Repository<Models.{1}>), UnityInjectionType.Default, new InjectionConstructor(typeof(Context)));", entityName, modelName),
                    string.Format(register + "(container, typeof(Interfaces.BusinessRepository.I{0}Entity<Models.{1}>), typeof(BusinessRepository.{0}Repository<Models.{1}>), UnityInjectionType.Session, new InjectionConstructor(typeof(Context), typeof(IBusinessEntitySession)));", entityName, modelName)
                };

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                for (var i = 0; i < methodSignatures.Length; i++)
                {
                    var methodSignature = methodSignatures[i];
                    var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                    var insertionIndex = methodSignatureIndex + 2 + i;
                    var lineToAdd = linesToAdd[i];

                    // Need to remove the tabs '\t' before doing the lookup.
                    if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                    {
                        // Line was not found so we will now insert it.
                        txtLines.Insert(insertionIndex, lineToAdd);
                    }

                    if (i == RegisterRepositoriesMethodIndex)
                    {
                        lineToAdd = linesToAdd[2];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(++insertionIndex, lineToAdd);
                        }

                        lineToAdd = linesToAdd[3];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(++insertionIndex, lineToAdd);
                        }
                    }
                }
                File.WriteAllLines(filePath, txtLines);
            }
        }

        /// <summary>
        /// Register types for controller
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateProcessWebBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectFolder;

            var webProjNs = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.Constants.ModelsKey][moduleId].ProjectName + ".Process";

            var filename = moduleId + Constants.WebBootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);
            if (File.Exists(filePath))
            {
                var pos = 1;

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                string[] namespaces =
                {
                    "using " + modelProjNs + ";" ,
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers.Process;",
                };

                for (var i = 0; i <= 1; i++)
                {
                    if (trimLines.IndexOf(namespaces[i]) < 0)
                    {
                        txtLines.Insert(++pos, namespaces[i]);
                    }
                }

                string[] methodSignatures =
                {
                    @"private void RegisterController(IUnityContainer container)",
                };

                const string register = "\t\t\tUnityUtil.RegisterType";
                string[] linesToAdd =
                {
                    string.Format(register + "<IController, {0}Controller<{2}>>(container, \"{1}{0}\");", entityName, moduleId, modelName)
                };

                for (var i = 0; i <= 0; i++)
                {
                    var lineToAdd = linesToAdd[i];
                    var methodSignature = methodSignatures[i];
                    var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                    var insertionIndex = methodSignatureIndex + 1 + i + pos;

                    // Need to remove the tabs '\t' before doing the lookup.
                    if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                    {
                        // Line was not found so we will now insert it.
                        txtLines.Insert(insertionIndex, lineToAdd);
                    }
                }

                File.WriteAllLines(filePath, txtLines);
            }
        }

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateProcessBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.ServicesKey][moduleId].ProjectFolder;

            var businessProjNs = settings.Projects[ProcessGeneration.Constants.BusinessRepositoryKey][moduleId].ProjectName + ".Process";
            var interfacesProjNs = settings.Projects[ProcessGeneration.Constants.InterfacesKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.Constants.ModelsKey][moduleId].ProjectName + ".Process";
            var servicesProjNs = settings.Projects[ProcessGeneration.Constants.ServicesKey][moduleId].ProjectName + ".Process";

            var filename = moduleId + Constants.BootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);

            const int RegisterServiceMethodIndex = 0;
            const int RegisterRepositoriesMethodIndex = 1;

            if (File.Exists(filePath))
            {
                var pos = 1;

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                string[] namespaces =
                {
                    "using " + businessProjNs + ";",
                    "using " + interfacesProjNs + ".BusinessRepository.Process;",
                    "using " + interfacesProjNs + ".Services.Process;",
                    "using " + modelProjNs + ";",
                    "using " + servicesProjNs + ";"
                };

                for (var i = 0; i < namespaces.Length; i++)
                {
                    if (trimLines.IndexOf(namespaces[i]) < 0)
                    {
                        txtLines.Insert(++pos, namespaces[i]);
                    }
                }

                string[] methodSignatures =
                {
                    @"private void RegisterService(IUnityContainer container)",
                    @"private void RegisterRepositories(IUnityContainer container)"
                };

                var register = "\t\t\tUnityUtil.RegisterType";
                string[] linesToAdd =
                {
                    string.Format(register + "<I{0}Service<{1}>, {0}Service<{1}>>(container);", entityName, modelName),
                    string.Format(register + "<I{0}Entity<{1}>, {0}Repository<{1}>>(container, UnityInjectionType.Default, new InjectionConstructor(typeof(Context)));", entityName, modelName),
                    string.Format(register + "<I{0}Entity<{1}>, {0}Repository<{1}>>(container, UnityInjectionType.Session, new InjectionConstructor(typeof(Context), typeof(IBusinessEntitySession)));", entityName, modelName)
                };

                for (var i = 0; i < methodSignatures.Length; i++)
                {
                    var methodSignature = methodSignatures[i];
                    var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                    var insertionIndex = methodSignatureIndex + pos + i;

                    if (i == RegisterServiceMethodIndex)
                    {
                        insertionIndex++;
                        var lineToAdd = linesToAdd[i];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(insertionIndex, lineToAdd);
                        }
                    }
                    else if (i == RegisterRepositoriesMethodIndex)
                    {
                        var lineToAdd = linesToAdd[1];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(++insertionIndex, lineToAdd);
                        }

                        lineToAdd = linesToAdd[2];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(++insertionIndex, lineToAdd);
                        }
                    }
                }
                File.WriteAllLines(filePath, txtLines);
            }
        }

        private static void UpdateReportWebBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectFolder;

            var webProjNs = settings.Projects[ProcessGeneration.Constants.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.Constants.ModelsKey][moduleId].ProjectName + ".Reports";

            var filename = moduleId + Constants.WebBootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);
            if (File.Exists(filePath))
            {
                var pos = 1;
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                string[] namespaces =
                {
                    "using " + modelProjNs + ";" ,
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers.Reports;",
                };

                for (var i = 0; i <= 1; i++)
                {
                    if (trimLines.IndexOf(namespaces[i]) < 0)
                    {
                        txtLines.Insert(++pos, namespaces[i]);
                    }
                }

                string[] methodSignatures =
                {
                    @"private void RegisterController(IUnityContainer container)",
                };

                const string register = "\t\t\tUnityUtil.RegisterType";
                string[] linesToAdd =
                {
                    string.Format(register + "<IController, {0}Controller<{2}>>(container, \"{1}{0}\");", entityName, moduleId, modelName)
                };

                for (var i = 0; i <= 0; i++)
                {
                    var lineToAdd = linesToAdd[i];
                    var methodSignature = methodSignatures[i];
                    var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                    var insertionIndex = methodSignatureIndex + 1 + i + pos;

                    // Need to remove the tabs '\t' before doing the lookup.
                    if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                    {
                        // Line was not found so we will now insert it.
                        txtLines.Insert(insertionIndex, lineToAdd);
                    }
                }

                File.WriteAllLines(filePath, txtLines);
            }
        }

        private static void UpdateReportBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.Constants.ModuleId];
            var entityName = view.Properties[BusinessView.Constants.EntityName];
            var modelName = view.Properties[BusinessView.Constants.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.Constants.ServicesKey][moduleId].ProjectFolder;

            var businessProjNs = settings.Projects[ProcessGeneration.Constants.BusinessRepositoryKey][moduleId].ProjectName + ".Reports";
            var interfacesProjNs = settings.Projects[ProcessGeneration.Constants.InterfacesKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.Constants.ModelsKey][moduleId].ProjectName + ".Reports";
            var servicesProjNs = settings.Projects[ProcessGeneration.Constants.ServicesKey][moduleId].ProjectName + ".Reports";

            var filename = moduleId + Constants.BootstrapperFilenameBase;
            var filePath = Path.Combine(pathProj, filename);

            const int RegisterServiceMethodIndex = 0;
            const int RegisterRepositoriesMethodIndex = 1;

            if (File.Exists(filePath))
            {
                var pos = 1;

                // Load the file and make the working lists
                MakeLists(filePath,
                          out List<string> trimLines,
                          out List<string> txtLines);

                string[] namespaces =
                {
                    "using " + businessProjNs + ";",
                    "using " + interfacesProjNs + ".BusinessRepository.Reports;",
                    "using " + interfacesProjNs + ".Services.Reports;",
                    "using " + modelProjNs + ";",
                    "using " + servicesProjNs + ";"
                };

                for (var i = 0; i < namespaces.Length; i++)
                {
                    if (trimLines.IndexOf(namespaces[i]) < 0)
                    {
                        txtLines.Insert(++pos, namespaces[i]);
                    }
                }

                string[] methodSignatures =
                {
                    @"private void RegisterService(IUnityContainer container)",
                    @"private void RegisterRepositories(IUnityContainer container)"
                };

                var register = "\t\t\tUnityUtil.RegisterType";
                string[] linesToAdd =
                {
                    string.Format(register + "<I{0}Service<{1}>, {0}Service<{1}>>(container);", entityName, modelName),
                    string.Format(register + "<I{0}Entity<{1}>, {0}Repository<{1}>>(container, UnityInjectionType.Default, new InjectionConstructor(typeof(Context)));", entityName, modelName),
                    string.Format(register + "<I{0}Entity<{1}>, {0}Repository<{1}>>(container, UnityInjectionType.Session, new InjectionConstructor(typeof(Context), typeof(IBusinessEntitySession)));", entityName, modelName)
                };

                for (var i = 0; i < methodSignatures.Length; i++)
                {
                    var methodSignature = methodSignatures[i];
                    var methodSignatureIndex = trimLines.IndexOf(methodSignature);
                    var insertionIndex = methodSignatureIndex + pos + i;

                    if (i == RegisterServiceMethodIndex)
                    {
                        insertionIndex++;
                        var lineToAdd = linesToAdd[i];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(insertionIndex, lineToAdd);
                        }
                    }
                    else if (i == RegisterRepositoriesMethodIndex)
                    {
                        var lineToAdd = linesToAdd[1];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(++insertionIndex, lineToAdd);
                        }

                        lineToAdd = linesToAdd[2];

                        // Need to remove the tabs '\t' before doing the lookup.
                        if (trimLines.IndexOf(lineToAdd.Trim()) == Constants.NotFoundInList)
                        {
                            // Line was not found so we will now insert it.
                            txtLines.Insert(++insertionIndex, lineToAdd);
                        }
                    }
                }
                File.WriteAllLines(filePath, txtLines);
            }
        }

        #endregion
    }
}
