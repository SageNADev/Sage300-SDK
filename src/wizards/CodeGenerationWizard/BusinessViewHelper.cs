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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary>
    /// Static class to assist with class generation
    /// </summary>
    public static class BusinessViewHelper
    {
        #region Private Constants

        private const string TabTwo = "\t\t";
        private const string TabThree = "\t\t\t";
        private const string TabFour = "\t\t\t\t";

        #endregion

        #region Public Methods

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateFlatBootStrappers(BusinessView view, Settings settings)
        {
            if (view.Options[BusinessView.GenerateClientFiles])
            {
                UpdateWebBootStrapper(view, settings);
            }
            if (view.Options[BusinessView.GenerateFinder])
            {
                UpdateWebBootStrapperForFinder(view, settings);
            }

            UpdateBootStrapper(view, settings);
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
            if (view.Options[BusinessView.GenerateClientFiles])
            {
                UpdateProcessWebBootStrapper(view, settings);
            }

            UpdateProcessBootStrapper(view, settings);
        }

        /// <summary>
        /// Update Bundles
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateBundles(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = (settings.RepositoryType.Equals(RepositoryType.HeaderDetail))? settings.EntitiesContainerName : view.Properties[BusinessView.EntityName];
            var projectInfoWeb = settings.Projects[ProcessGeneration.WebKey][moduleId];
            var pathProj = projectInfoWeb.ProjectFolder;
            var bundleFile = Path.Combine(pathProj, "BundleRegistration.cs");
            var scriptBase = settings.CompanyNamespace;

            const string tag = "internal static void RegisterBundles(BundleCollection bundles)";
            var textlineToAdded = string.Format(TabThree + @"#region {0}" + "\r\n" +
                                                TabThree + @"bundles.Add(new Bundle(""~/bundles/{2}{1}{0}"").Include(" + "\r\n" +
                                                TabFour + @"""~/Areas/{1}/Scripts/{0}/{2}.{1}.{0}Behaviour.js""," + "\r\n" +
                                                TabFour + @"""~/Areas/{1}/Scripts/{0}/{2}.{1}.{0}KoExtn.js""," + "\r\n" +
                                                TabFour + @"""~/Areas/{1}/Scripts/{0}/{2}.{1}.{0}Repository.js""," + "\r\n" +
                                                TabFour + @"""~/Areas/Core/Scripts/Process/Sage.CA.SBS.Sage300.Common.Process.js""));" + "\r\n" +
                                                TabThree + @"#endregion" + "\r\n", entityName, moduleId, scriptBase);

            if (File.Exists(bundleFile))
            {
                var txtLines = File.ReadAllLines(bundleFile).ToList();
                var trimLines = (File.ReadAllLines(bundleFile)).Select(l => l.Trim()).ToList();
                var index = trimLines.IndexOf(tag);
                if (index > -1)
                {
                    var pos = index + 2;
                    txtLines.Insert(pos, textlineToAdded);
                }
                File.WriteAllLines(bundleFile, txtLines);
            }
        }

        /// <summary>
        /// Update View Page URL
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void CreateViewPageUrl(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var pathProj = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectFolder;
            var pageUrlFile = Path.Combine(pathProj, "pageUrl.txt");
            var pageUrl = "/OnPremise/" + moduleId + "/" + (settings.RepositoryType.Equals(RepositoryType.HeaderDetail)?settings.EntitiesContainerName:entityName);

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
        /// <param name="changeToDot">True to replace separaor with dot (namespaces)</param>
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

            // Replace seperator with dot?
            if (changeToDot)
            {
                retVal = retVal.Replace(@"\", ".");
            }

            return retVal;
        }

        /// <summary>
        /// Modify module level security const string class to add view security resource ID
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        public static void UpdateSecurityClass(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var pathBusinessRepository = settings.Projects[ProcessGeneration.BusinessRepositoryKey][moduleId].ProjectFolder;
            var filePath = Path.Combine(pathBusinessRepository, @"Security\Security.cs");
            var f = Environment.NewLine + TabTwo;
            var commentLine = f + "/// <summary>" + f + "/// Security resourceID for " + moduleId + " " + entityName + f + "/// </summary>" + Environment.NewLine;
            var constName = moduleId + entityName;
            var constLine = f + "public const string " + constName + " = \"" + constName.ToUpper() + "\";" + Environment.NewLine;

            if (File.Exists(filePath))
            {
                var text = File.ReadAllText(filePath);
                var pos = text.IndexOf('}');
                if (pos > -1)
                {
                    var updateText = text.Substring(0, pos) + commentLine + constLine + text.Substring(pos - 2);
                    File.WriteAllText(filePath, updateText);
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
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var pathProj = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectFolder;
            var menuFile = Path.Combine(pathProj, moduleId + "MenuDetails.xml");
            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(File.ReadAllText(menuFile));
                var root = xmlDoc.DocumentElement;
                if (root != null)
                {
                    var itemNodes = root.SelectNodes("item");
                    if (itemNodes != null)
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
                        node.InnerText = moduleId + "/" + entityName;
                        node = itemNode.SelectSingleNode("MenuItemOrder");
                        node.InnerText = (int.Parse(node.InnerText) + 1).ToString();
                        node = itemNode.SelectSingleNode("SecurityResourceKey");
                        node.InnerText = key;

                        root.InsertAfter(itemNode, lastItemNode);
                    }
                }
                xmlDoc.Save(menuFile);
            }
            catch (Exception)
            {
                // do nothing, not throw exception to break application
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Register types for controller/Finder/ImportExport
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateWebBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var modelName = view.Properties[BusinessView.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectFolder;

            var webProjNs = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.ModelsKey][moduleId].ProjectName;

            var bsName = moduleId + "WebBootstrapper.cs";
            var bsFile = Path.Combine(pathProj, bsName);
            if (File.Exists(bsFile))
            {
                const string register = "\t\t\tUnityUtil.RegisterType";
                var trimLines = (File.ReadAllLines(bsFile)).Select(l => l.Trim()).ToList();
                var txtLines = File.ReadAllLines(bsFile).ToList();
                var pos = 1;

                string[] nameSpace =
                {
                    "using " + modelProjNs + ";" ,
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers;",
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers.Finder;"
                };

                for (var i = 0; i <= 2; i++)
                {
                    if (trimLines.IndexOf(nameSpace[i]) < 0)
                    {
                        txtLines.Insert(++pos, nameSpace[i]);
                    }
                }

                string[] tags =
                {
                    @"private void RegisterController(IUnityContainer container)",
                    @"private void RegisterFinder(IUnityContainer container)",
                    @"private void RegisterExportImportController(IUnityContainer container)"
                };
                string[] linesToAdded =
                {
                    string.Format(register + "<IController, {0}Controller<{2}>>(container, \"{1}{0}\");", entityName, moduleId, modelName),
                    string.Format(register + "<IFinder, Find{0}ControllerInternal<{3}>>(container, \"{1}{2}\", new InjectionConstructor(typeof(Context)));", entityName, moduleId.ToLower(), entityName.ToLower(), modelName),
                    string.Format(register + "<IExportImportController, {0}ControllerInternal<{3}>>(container, \"{1}{2}\", new InjectionConstructor(typeof(Context)));",entityName, moduleId.ToLower(), entityName.ToLower(), modelName)
                };

                for (var i = 0; i <= 2; i++)
                {
                    if (!view.Options[BusinessView.GenerateFinder] && i == 1)
                    {
                        pos--;
                        continue;
                    }
                    var index = trimLines.IndexOf(tags[i]) + 1 + i + pos;
                    txtLines.Insert(index, linesToAdded[i]);
                }
                File.WriteAllLines(bsFile, txtLines);
            }
        }
        /// <summary>
        /// Register types for Finder
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateWebBootStrapperForFinder(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var modelName = view.Properties[BusinessView.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectFolder;

            var webProjNs = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.ModelsKey][moduleId].ProjectName;

            var bsName = moduleId + "WebBootstrapper.cs";
            var bsFile = Path.Combine(pathProj, bsName);
            if (File.Exists(bsFile))
            {
                const string register = "\t\t\tUnityUtil.RegisterType";
                var trimLines = (File.ReadAllLines(bsFile)).Select(l => l.Trim()).ToList();
                var txtLines = File.ReadAllLines(bsFile).ToList();
                var pos = 1;

                string[] nameSpace =
                {
                    "using " + modelProjNs + ";" ,
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers;",
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers.Finder;"
                };

                for (var i = 0; i <= 2; i++)
                {
                    if (trimLines.IndexOf(nameSpace[i]) < 0)
                    {
                        txtLines.Insert(++pos, nameSpace[i]);
                    }
                }

                string tags = @"private void RegisterFinder(IUnityContainer container)";
                
                var linesToAdded = string.Format(register + "<IFinder, Find{0}ControllerInternal<{3}>>(container, \"{1}{2}\", new InjectionConstructor(typeof(Context)));", entityName, moduleId.ToLower(), entityName.ToLower(), modelName);

                var index = trimLines.IndexOf(tags) + 1 + pos;
                txtLines.Insert(index, linesToAdded);

                File.WriteAllLines(bsFile, txtLines);
            }
        }

        /// <summary>
        /// Register types for controller/Finder/ImportExport
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateHeaderDetailWebBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var modelName = view.Properties[BusinessView.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectFolder;

            var webProjNs = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.ModelsKey][moduleId].ProjectName;

            var bsName = moduleId + "WebBootstrapper.cs";
            var bsFile = Path.Combine(pathProj, bsName);
            if (File.Exists(bsFile))
            {
                const string register = "\t\t\tUnityUtil.RegisterType";
                var trimLines = (File.ReadAllLines(bsFile)).Select(l => l.Trim()).ToList();
                var txtLines = File.ReadAllLines(bsFile).ToList();
                var pos = 1;

                string[] nameSpace =
                {
                    "using " + modelProjNs + ";" ,
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers;",
                    "using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;",
                    "using " + settings.Projects[ProcessGeneration.InterfacesKey][moduleId].ProjectName + ".BusinessRepository;"
                };

                for (var i = 0; i < nameSpace.Count(); i++)
                {
                    if (trimLines.IndexOf(nameSpace[i]) < 0)
                    {
                        txtLines.Insert(++pos, nameSpace[i]);
                    }
                }

                string[] tags =
                {
                    @"private void RegisterController(IUnityContainer container)",
                    @"private void RegisterExportImportController(IUnityContainer container)"
                };
                string[] linesToAdded =
                {
                    string.Format(register + "<IController, {0}Controller>(container, \"{1}{0}\");", settings.EntitiesContainerName, moduleId),
                    string.Format(register + "<IExportImportController, ImportExportControllerInternal<I{0}Repository>>(container, \"{1}{2}\", new InjectionConstructor(typeof(Context)));", settings.EntitiesContainerName, moduleId.ToLower(), settings.EntitiesContainerName.ToLower())
                };

                for (var i = 0; i < tags.Count(); i++)
                {
                    var index = trimLines.IndexOf(tags[i]) +  i + pos + 1;
                    txtLines.Insert(index, linesToAdded[i]);
                }
                File.WriteAllLines(bsFile, txtLines);
            }
        }
        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateHeaderDetailModuleBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var modelName = view.Properties[BusinessView.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.ServicesKey][moduleId].ProjectFolder;
            var bsName = moduleId + "Bootstrapper.cs";
            var bsFile = Path.Combine(pathProj, bsName);

            if (File.Exists(bsFile))
            {
                var register = "\t\t\tUnityUtil.RegisterType";
                string tags = @"private void RegisterService(IUnityContainer container)";
                string linesToAdded = string.Format(register + "<I{0}Repository, {0}Repository>(container);", settings.EntitiesContainerName, settings.EntitiesContainerName);

                string[] nameSpace =
                {
                    "using " + settings.CompanyNamespace + "." + moduleId + ".BusinessRepository;",
                    "using " +  settings.CompanyNamespace + "." + moduleId + ".Interfaces.BusinessRepository;"
                };

                var txtLines = File.ReadAllLines(bsFile).ToList();
                var trimLines = (File.ReadAllLines(bsFile)).Select(l => l.Trim()).ToList();

                for (var i = 0; i < nameSpace.Count(); i++)
                {
                    if (trimLines.IndexOf(nameSpace[i]) < 0)
                    {
                        txtLines.Insert(2, nameSpace[i]);
                    }
                }


                var index = trimLines.IndexOf(tags) + 2;
                txtLines.Insert(index + 2, linesToAdded);

                File.WriteAllLines(bsFile, txtLines);
            }
        }

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var modelName = view.Properties[BusinessView.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.ServicesKey][moduleId].ProjectFolder;
            var bsName = moduleId + "Bootstrapper.cs";
            var bsFile = Path.Combine(pathProj, bsName);

            if (File.Exists(bsFile))
            {
                var register = "\t\t\tUnityUtil.RegisterType";
                string[] tags =
                {
                    @"private void RegisterService(IUnityContainer container)",
                    @"private void RegisterRepositories(IUnityContainer container)",
                };

                string[] linesToAdded =
                {
                    string.Format(register + "<Interfaces.Services.I{0}Service<Models.{1}>, {0}EntityService<Models.{1}>>(container);",entityName, modelName),
                    string.Format(register + "<IExportImportRepository, BusinessRepository.{2}Repository<Models.{3}>>(container, \"{1}{0}\", new InjectionConstructor(typeof(Context)));", entityName.ToLower(), moduleId.ToLower(), entityName, modelName),
                    string.Format(register + "(container, typeof(Interfaces.BusinessRepository.I{0}Entity<Models.{1}>), typeof(BusinessRepository.{0}Repository<Models.{1}>), UnityInjectionType.Default, new InjectionConstructor(typeof(Context)));", entityName, modelName),
                    string.Format(register + "(container, typeof(Interfaces.BusinessRepository.I{0}Entity<Models.{1}>), typeof(BusinessRepository.{0}Repository<Models.{1}>), UnityInjectionType.Session, new InjectionConstructor(typeof(Context), typeof(IBusinessEntitySession)));", entityName, modelName)
                };

                var txtLines = File.ReadAllLines(bsFile).ToList();
                var trimLines = (File.ReadAllLines(bsFile)).Select(l => l.Trim()).ToList();
                for (var i = 0; i < 2; i++)
                {
                    var index = trimLines.IndexOf(tags[i]) + 2 + i;
                    txtLines.Insert(index, linesToAdded[i]);
                    if (i == 1)
                    {
                        txtLines.Insert(++index, linesToAdded[2]);
                        txtLines.Insert(++index, linesToAdded[3]);
                    }
                }
                File.WriteAllLines(bsFile, txtLines);
            }
        }

        /// <summary>
        /// Register types for controller
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateProcessWebBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var modelName = view.Properties[BusinessView.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectFolder;

            var webProjNs = settings.Projects[ProcessGeneration.WebKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.ModelsKey][moduleId].ProjectName + ".Process";

            var bsName = moduleId + "WebBootstrapper.cs";
            var bsFile = Path.Combine(pathProj, bsName);
            if (File.Exists(bsFile))
            {
                const string register = "\t\t\tUnityUtil.RegisterType";
                var trimLines = (File.ReadAllLines(bsFile)).Select(l => l.Trim()).ToList();
                var txtLines = File.ReadAllLines(bsFile).ToList();
                var pos = 1;

                string[] nameSpace =
                {
                    "using " + modelProjNs + ";" ,
                    "using " + webProjNs + ".Areas." + moduleId + ".Controllers.Process;",
                };

                for (var i = 0; i <= 1; i++)
                {
                    if (trimLines.IndexOf(nameSpace[i]) < 0)
                    {
                        txtLines.Insert(++pos, nameSpace[i]);
                    }
                }

                string[] tags =
                {
                    @"private void RegisterController(IUnityContainer container)",
                };
                string[] linesToAdded =
                {
                    string.Format(register + "<IController, {0}Controller<{2}>>(container, \"{1}{0}\");", entityName, moduleId, modelName)
                };

                for (var i = 0; i <= 0; i++)
                {
                    var index = trimLines.IndexOf(tags[i]) + 1 + i + pos;
                    txtLines.Insert(index, linesToAdded[i]);
                }
                File.WriteAllLines(bsFile, txtLines);
            }
        }

        /// <summary>
        /// Register types for service/repository
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">Settings</param>
        private static void UpdateProcessBootStrapper(BusinessView view, Settings settings)
        {
            var moduleId = view.Properties[BusinessView.ModuleId];
            var entityName = view.Properties[BusinessView.EntityName];
            var modelName = view.Properties[BusinessView.ModelName];
            var pathProj = settings.Projects[ProcessGeneration.ServicesKey][moduleId].ProjectFolder;

            var businessProjNs = settings.Projects[ProcessGeneration.BusinessRepositoryKey][moduleId].ProjectName + ".Process";
            var interfacesProjNs = settings.Projects[ProcessGeneration.InterfacesKey][moduleId].ProjectName;
            var modelProjNs = settings.Projects[ProcessGeneration.ModelsKey][moduleId].ProjectName + ".Process";
            var servicesProjNs = settings.Projects[ProcessGeneration.ServicesKey][moduleId].ProjectName + ".Process";

            var bsName = moduleId + "Bootstrapper.cs";
            var bsFile = Path.Combine(pathProj, bsName);

            if (File.Exists(bsFile))
            {
                var register = "\t\t\tUnityUtil.RegisterType";
                var txtLines = File.ReadAllLines(bsFile).ToList();
                var trimLines = (File.ReadAllLines(bsFile)).Select(l => l.Trim()).ToList();
                var pos = 1;

                string[] nameSpace =
                {
                    "using " + businessProjNs + ";",
                    "using " + interfacesProjNs + ".BusinessRepository.Process;",
                    "using " + interfacesProjNs + ".Services.Process;",
                    "using " + modelProjNs + ";",
                    "using " + servicesProjNs + ";"
                };

                for (var i = 0; i <= 4; i++)
                {
                    if (trimLines.IndexOf(nameSpace[i]) < 0)
                    {
                        txtLines.Insert(++pos, nameSpace[i]);
                    }
                }

                string[] tags =
                {
                    @"private void RegisterService(IUnityContainer container)",
                    @"private void RegisterRepositories(IUnityContainer container)"
                };

                string[] linesToAdded =
                {
                    string.Format(register + "<I{0}Service<{1}>, {0}Service<{1}>>(container);", entityName, modelName),
                    string.Format(register + "<I{0}Entity<{1}>, {0}Repository<{1}>>(container, UnityInjectionType.Default, new InjectionConstructor(typeof(Context)));", entityName, modelName),
                    string.Format(register + "<I{0}Entity<{1}>, {0}Repository<{1}>>(container, UnityInjectionType.Session, new InjectionConstructor(typeof(Context), typeof(IBusinessEntitySession)));", entityName, modelName)
                };



                for (var i = 0; i < 2; i++)
                {
                    var index = trimLines.IndexOf(tags[i]) + pos + i;
                    if (i == 0)
                    {
                        index = index + 2;
                        txtLines.Insert(index, linesToAdded[i]);
                    }
                    else
                    {
                        txtLines.Insert(++index, linesToAdded[1]);
                        txtLines.Insert(++index, linesToAdded[2]);
                    }
                }
                File.WriteAllLines(bsFile, txtLines);
            }
        }

        #endregion
    }
}
