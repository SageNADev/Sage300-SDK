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
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary>
    /// Snippet Helper to provide code snippets
    /// </summary>
    public static class SnippetHelper
    {
        /// <summary> div constant </summary>
        private const string DIV = "div";
        /// <summary> h3 constant </summary>
        private const string H3 = "h3";
        /// <summary> ul constant </summary>
        private const string UL = "ul";
        /// <summary> Number Of OptionalFields  </summary>
        private const string NUMBEROFOPTIONALFIELDS = "NumberOfOptionalFields";
        /// <summary> Widget types for special iterations  </summary>
        private static readonly string[] WIDGET_TYPES = { "DateTime", "Time", "Checkbox", "Numeric", "Textbox", "Finder", "Dropdown" };

        /// <summary>
        /// Generate Widgets
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="element">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="settings">Settings object</param>
        /// <param name="view">Business View</param>
        public static void GenerateWidgets(int depth, XElement element, StringBuilder snippet, 
            Settings settings, BusinessView view)
        {
            // Local references
            bool rowStarted = false;
            bool isHeaderDetails = settings.RepositoryType.Equals(RepositoryType.HeaderDetail);
            depth++;

            // Iterate elements
            foreach (var controlElement in element.Elements())
            {
                // grid column control no element in razor view
                if (controlElement.Attribute("type").Value.Equals("gridColumn"))
                {
                    continue;
                }

                // If row is found
                if (controlElement.Attribute("newRow").Value.Equals("true"))
                {
                    StartRowRazorView(depth, controlElement, snippet);
                    rowStarted = true;
                }
                else if (controlElement.Attribute("widget").Value.Equals("Finder"))
                {
                    FinderRazorView(depth, controlElement, snippet, view);
                }
                else if (controlElement.Attribute("widget").Value.Equals("Textbox") && controlElement.Attribute("type").Value.Equals(DIV))
                {
                    TextboxRazorView(depth, controlElement, snippet, view);
                }
                else if (controlElement.Attribute("widget").Value.Equals("Dropdown"))
                {
                    DropdownRazorView(depth, controlElement, snippet, view);
                }
                else if (controlElement.Attribute("widget").Value.Equals("DateTime") &&
                    !Convert.ToBoolean(controlElement.Attribute("timeOnly").Value))
                {
                    DatetimeRazorView(depth, controlElement, snippet, view);
                }
                else if (controlElement.Attribute("widget").Value.Equals("Checkbox"))
                {
                    CheckboxRazorView(depth, controlElement, snippet, view);
                }
                else if (controlElement.Attribute("widget").Value.Equals("RadioButtons"))
                {
                    RadioButtonsRazorView(depth, controlElement, snippet, settings, view);
                }
                else if (controlElement.Attribute("widget").Value.Equals("Grid"))
                {
                    GridRazorView(depth, controlElement, snippet);
                }
                else if (controlElement.Attribute("widget").Value.Equals("Button"))
                {
                    ButtonRazorView(depth, controlElement, snippet);
                }
                else if (controlElement.Attribute("widget").Value.Equals("Tab"))
                {
                    var entityName = isHeaderDetails? settings.EntitiesContainerName : view.Properties[BusinessView.Constants.EntityName];
                    TabRazorView(depth, controlElement, snippet, view, entityName);
                }
                else if (controlElement.Attribute("widget").Value.Equals("Numeric"))
                {
                    var property = controlElement.Attribute("property").Value;
                    if (property== "NumberOfOptionalFields") 
                    {
                        CheckboxHamburgerRazorView(depth, controlElement, snippet, view, "HasOptionalFields");
                    } 
                    else 
                    {
                        NumericRazorView(depth, controlElement, snippet, view);
                    }
                }
                else if (controlElement.Attribute("widget").Value.Equals("Time") ||
                        ((controlElement.Attribute("widget").Value.Equals("DateTime") &&
                        Convert.ToBoolean(controlElement.Attribute("timeOnly").Value))))
                {
                    TimeRazorView(depth, controlElement, snippet, 
                        view, Convert.ToBoolean(controlElement.Attribute("timeOnly").Value));
                }

                // children?
                if (controlElement.HasElements && !controlElement.Attribute("widget").Value.Equals("Tab"))
                {
                    GenerateWidgets(depth, controlElement.Descendants().First(), snippet, settings, view);
                }

                // End element
                if (rowStarted)
                {
                    EndRowRazorView(depth, snippet);
                }
            }
        }

        /// <summary>
        /// Finder Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        private static void FinderRazorView(int depth, XElement controlElement, StringBuilder snippet, BusinessView view)
        {
            var property = controlElement.Attribute("property").Value;
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "search-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + SgFinderFor(property, businessField, depth + 2));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Textbox Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        private static void TextboxRazorView(int depth, XElement controlElement, StringBuilder snippet, BusinessView view)
        {
            var property = controlElement.Attribute("property").Value;
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "input-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + SgTextFor(property, businessField, depth + 2));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Dropdown Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        private static void DropdownRazorView(int depth, XElement controlElement, StringBuilder snippet, BusinessView view)
        {
            var property = controlElement.Attribute("property").Value;
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "dropdown-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + SgDropdownFor(property, businessField, depth + 2));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Datetime Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="isTimeOnly">True to treat DateTime as Time otherwise false </param>
        /// <param name="view">Business View</param>
        private static void DatetimeRazorView(int depth, XElement controlElement, StringBuilder snippet, BusinessView view)
        {
            var property = controlElement.Attribute("property").Value;
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "datepicker-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + SgDatepickerFor(property, businessField, depth + 2));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Checkbox Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        private static void CheckboxRazorView(int depth, XElement controlElement, StringBuilder snippet, BusinessView view)
        {
            var property = controlElement.Attribute("property").Value;
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "ctrl-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + StartingTag(DIV, "child"));
            snippet.AppendLine(new string(' ', (depth + 2) * 4) + SgCheckboxFor(property, businessField, depth + 3));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + EndingTag(DIV));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// CheckboxHamburger Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        private static void CheckboxHamburgerRazorView(int depth, XElement controlElement, StringBuilder snippet, BusinessView view, string property)
        {
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            //@Html.SageHamburger("#", null, null, new { @id = "lnkOptionalField" })
            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "ctrl-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + StartingTag(DIV, "child"));
            snippet.AppendLine(new string(' ', (depth + 2) * 4) + SgCheckboxFor(property, businessField, depth + 3));
            snippet.AppendLine(new string(' ', (depth + 2) * 4) + SageHamburger(property));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + EndingTag(DIV));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// RadioButtons Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="settings">Settings object</param>
        /// <param name="view">Business View</param>
        private static void RadioButtonsRazorView(int depth, XElement controlElement, StringBuilder snippet,
            Settings settings, BusinessView view)
        {
            var property = controlElement.Attribute("property").Value;
            var enumHelper = view.Enums[property];
            var enumName = enumHelper.Name;
            var resxName = view.Properties[BusinessView.Constants.ResxName];

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "sub-section"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + StartingTag(DIV, "section-heading"));
            snippet.AppendLine(new string(' ', (depth + 2) * 4) + StartingTag(H3) + SageLabelFor(property, view) + EndingTag(H3));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + EndingTag(DIV));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + StartingTag(DIV, "section-body"));
            snippet.AppendLine(new string(' ', (depth + 2) * 4) + StartingTag(DIV, "ctrl-group ctrl-group-inline ctrl-group-inline-lg"));

            foreach (var value in enumHelper.Values)
            {
                // Locals - Used to split out prefix and replace invalid characters
                var tmp = value.Key.Split(':');
                var valueName = tmp[1];
                if (!settings.ResourceKeys.Contains(valueName))
                {
                    valueName = settings.ResourceKeys.FirstOrDefault(e => e.Equals(valueName, StringComparison.CurrentCultureIgnoreCase));
                }

                snippet.AppendLine(new string(' ', (depth + 3) * 4) + StartingTag(DIV, "child"));
                snippet.AppendLine(new string(' ', (depth + 4) * 4) + KoSageRadioButtonFor(property, enumName, valueName, 
                    settings.CompanyNamespace, settings.ModuleId));
                snippet.AppendLine(new string(' ', (depth + 4) * 4) + SageLabel(property, resxName, valueName));
                snippet.AppendLine(new string(' ', (depth + 3) * 4) + EndingTag(DIV));
            }

            snippet.AppendLine(new string(' ', (depth + 2) * 4) + EndingTag(DIV));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + EndingTag(DIV));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Grid Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        private static void GridRazorView(int depth, XElement controlElement, StringBuilder snippet)
        {
            var entityName = controlElement.Attribute("text").Value;
            var gridName = entityName.Substring(0, 1).ToLower() + entityName.Substring(1);
            snippet.AppendLine(new string(' ', depth * 4) + string.Format("@Html.SageGrid(\"{0}Grid\", (Sage.CA.SBS.ERP.Sage300.Common.Models.GridDefinition)@ViewBag.{1}Grid)", gridName, entityName));
        }

        /// <summary>
        /// Tab Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        private static void TabRazorView(int depth, XElement controlElement, StringBuilder snippet,
            BusinessView view, string entityName = "")
        {
            var id = controlElement.Attribute("id").Value;
            entityName = string.IsNullOrEmpty(entityName) ? view.Properties[BusinessView.Constants.EntityName] : entityName;
            var resxName = view.Properties[BusinessView.Constants.ResxName];

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "xsmall tab-group", id));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + StartingTag(UL));

            // Iterate tab pages
            var tabCount = 0;
            foreach (var tabPageElement in controlElement.Descendants().First().Elements())
            {
                // Tab Page Snippet
                var pageId = tabPageElement.Attribute("id").Value;
                var elementType = tabPageElement.Attribute("type").Value;
                var activePage = string.Empty;
                tabCount++;

                // Determine class
                if (tabCount == 1)
                {
                    activePage = " class=\"k-state-active\"";
                }

                snippet.AppendLine(new string(' ', (depth + 2) * 4) + "<" + elementType + activePage + " id=\"tab" + pageId + "\">@" + resxName + "." + pageId + "</" + elementType + ">");
            }

            snippet.AppendLine(new string(' ', (depth + 1) * 4) + EndingTag(UL));

            // Need to specify partial views
            foreach (var tabPageElement in controlElement.Descendants().First().Elements())
            {
                // Tab Page Snippet
                var pageId = tabPageElement.Attribute("id").Value;
                
                snippet.AppendLine(new string(' ', (depth + 1) * 4) + "@Html.Partial(Constants." + entityName + pageId + ")");
            }

            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Numeric Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        private static void NumericRazorView(int depth, XElement controlElement, StringBuilder snippet, BusinessView view)
        {
            var property = controlElement.Attribute("property").Value;
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "numeric-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + SgNumericFor(property, businessField, depth + 2));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Button Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        private static void ButtonRazorView(int depth, XElement controlElement, StringBuilder snippet)
        {
            var id = controlElement.Attribute("id").Value;
            var text = controlElement.Attribute("text").Value;

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "button-group no-label"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + KoSageButton(id, text));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Time Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        /// <param name="isTimeOnly">True if time only otherwise false</param>
        private static void TimeRazorView(int depth, XElement controlElement, StringBuilder snippet, 
            BusinessView view, bool isTimeOnly)
        {
            var property = controlElement.Attribute("property").Value;
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, "timepicker-group"));
            snippet.AppendLine(new string(' ', (depth + 1) * 4) + SgTimepickerFor(property, businessField, isTimeOnly, depth + 2));
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// StartRow Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        private static void StartRowRazorView(int depth, XElement controlElement, StringBuilder snippet)
        {
            // If new row is for a tab, then remove the class from the div
            var className = "form-group";
            if (controlElement.HasElements)
            {
                var tabElement = controlElement.Descendants().First();
                if (tabElement != null && tabElement.HasElements)
                {
                    if (tabElement.Elements().First().Attribute("widget").Value.Equals("Tab"))
                    {
                        className = "";
                    }
                }
            }

            snippet.AppendLine(new string(' ', depth * 4) + StartingTag(DIV, className));
        }

        /// <summary>
        /// EndRow Snippet Razor View
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="controlElement">XML element</param>
        /// <param name="snippet">Snippet being constructed</param>
        private static void EndRowRazorView(int depth, StringBuilder snippet)
        {
            snippet.AppendLine(new string(' ', depth * 4) + EndingTag(DIV));
        }

        /// <summary>
        /// Ignored Properties Snippet JavaScript
        /// </summary>
        /// <param name="settings">Settings object</param>
        /// <returns>Disabled properties or empty if none</returns>
        public static string IgnoredPropertiesJavaScript(Settings settings)
        {
            var retVal = string.Empty;

            foreach (var key in WIDGET_TYPES)
            {
                // Iterate widgets, if any
                if (settings.Widgets.ContainsKey(key))
                {
                    foreach (var widget in settings.Widgets[key])
                    {
                        retVal += ", \"Is" + widget + "Disabled\"";
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Tab Strip Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="localEntityName">Local Entity Name</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <returns>First tab page or empty if none</returns>
        public static string TabStripJavaScript(int depth, Settings settings, string localEntityName, StringBuilder snippet)
        {
            var retVal = string.Empty;

            // Iterate tab page widgets, if any
            if (settings.Widgets.ContainsKey("TabPage"))
            {
                // tabStrip
                snippet.AppendLine("var " + localEntityName + "TabStrip = " + localEntityName + "TabStrip || {};");
                snippet.AppendLine(localEntityName + "TabStrip.ValueType = {");
                var count = 0;
                foreach (var widget in settings.Widgets["TabPage"])
                {
                    count++;
                    var addComma = settings.Widgets["TabPage"].Count != count ? "," : string.Empty;
                    snippet.AppendLine(new string(' ', depth) + widget + ": \"" + widget + "\"" + addComma);
                    if (count == 1)
                    {
                        retVal = localEntityName + "TabStrip.ValueType." + widget;
                    }
                }
                snippet.AppendLine("};");
            }

            return retVal;
        }

        /// <summary>
        /// Init Dropdown List Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <returns>First tab page or empty if none</returns>
        public static void InitDropDownListJavaScript(int depth, Settings settings, StringBuilder snippet)
        {
            // Iterate drop down widgets, if any
            if (settings.Widgets.ContainsKey("Dropdown"))
            {
                foreach (var widget in settings.Widgets["Dropdown"])
                {
                    snippet.AppendLine(new string(' ', depth) + "sg.utls.kndoUI.dropDownList(\"ddl" + widget + "\");");
                }
            }
        }

        /// <summary>
        /// Init Buttons Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <returns>Code snippet or empty if none</returns>
        public static void InitButtonsJavaScript(int depth, Settings settings, StringBuilder snippet)
        {
            // Iterate button widgets, if any
            if (settings.Widgets.ContainsKey("Button"))
            {
                foreach (var widget in settings.Widgets["Button"])
                {
                    snippet.AppendLine(new string(' ', depth));
                    snippet.AppendLine(new string(' ', depth) + "$(\"#" + widget + "\").bind('click', function (e) {");
                    snippet.AppendLine(new string(' ', depth) + "});");
                }
            }
        }

        /// <summary>
        /// Init Tabs Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="localEntityName">Local Entity Name</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="firstTabPage">First tab page</param>
        public static void InitTabsJavaScript(int depth, Settings settings, string localEntityName, 
            StringBuilder snippet, string firstTabPage)
        {
            // Iterate tab, if any
            if (settings.Widgets.ContainsKey("Tab"))
            {
                foreach (var widget in settings.Widgets["Tab"])
                {
                    snippet.AppendLine(new string(' ', depth) + "sg.utls.kndoUI.initTab(\"" + widget + "\");");
                    snippet.AppendLine(new string(' ', depth) + "sg.utls.kndoUI.selectTab(\"" + widget + "\", " + firstTabPage + ");");
                    snippet.AppendLine("");
                    snippet.AppendLine(new string(' ', depth) + "var tabStrip = $(\"#" + widget + "\").data(\"kendoTabStrip\");");
                    snippet.AppendLine(new string(' ', depth) + "if (tabStrip) {");
                    snippet.AppendLine(new string(' ', depth + 4) + "tabStrip.bind(\"select\", " + localEntityName + "UI.onTabSelect);");
                    snippet.AppendLine(new string(' ', depth) + "}");
                }
            }
        }

        /// <summary>
        /// Init Numeric Textboxes Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        public static void InitNumericTextboxesJavaScript(int depth, Settings settings, StringBuilder snippet, BusinessView view)
        {
            // Iterate numeric widgets, if any
            if (settings.Widgets.ContainsKey("Numeric"))
            {
                foreach (var widget in settings.Widgets["Numeric"])
                {
                    if (widget == NUMBEROFOPTIONALFIELDS) continue;
                    // Get Business View
                    var businessField = view.Fields.Where(x => x.Name == widget).FirstOrDefault();
                    var maxDigits = (businessField.Size - 1) * 2;

                    snippet.AppendLine(new string(' ', depth) + "sg.utls.initNumericTextBox(\"nbr" + widget + "\", " +
                    businessField.Precision.ToString() + ", false, 0, " + maxDigits + ", " +
                    businessField.MinValue.ToString() + ", " +
                    businessField.MaxValue.ToString() + ");");
                }
            }
        }

        /// <summary>
        /// Init Textboxes Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="containerName">Container Name</param>
        public static void InitTextboxesJavaScript(int depth, Settings settings, StringBuilder snippet, string containerName)
        {
            // Iterate date widgets, if any
            if (settings.Widgets.ContainsKey("DateTime"))
            {
                foreach (var widget in settings.Widgets["DateTime"])
                {
                    snippet.AppendLine(new string(' ', depth) + "$(\"#txt" + widget + "\").on('change', function(e) {");
                    snippet.AppendLine(new string(' ', depth + 4) + "if (sg.utls.kndoUI.checkForValidDate($(\"#txt" + widget + "\").val())) {");
                    snippet.AppendLine(new string(' ', depth + 8) + "sg.utls.clearValidations(\"frm" + containerName + "\");");
                    snippet.AppendLine(new string(' ', depth + 4) + "} else {");
                    snippet.AppendLine(new string(' ', depth + 8) + "sg.controls.Focus($(\"#txt" + widget + "\"));");
                    snippet.AppendLine(new string(' ', depth + 4) + "}");
                    snippet.AppendLine(new string(' ', depth) + "});");
                    snippet.AppendLine("");
                }
            }
        }

        /// <summary>
        /// Init TimePickers Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="localEntityName">Local Entity Name</param>
        /// <param name="view">Business View</param>
        public static void InitTimePickersJavaScript(int depth, Settings settings, StringBuilder snippet, 
            string localEntityName, BusinessView view)
        {
            // Iterate time widgets, if any
            if (settings.Widgets.ContainsKey("Time"))
            {
                foreach (var widget in settings.Widgets["Time"])
                {
                    TimePickersJavaScript(depth, snippet, localEntityName, widget);
                }
            }

            // Iterate date time widgets, if any that are time only
            if (settings.Widgets.ContainsKey("DateTime"))
            {
                foreach (var widget in settings.Widgets["DateTime"])
                {
                    // Get Business View
                    var businessField = view.Fields.Where(x => x.Name == widget).FirstOrDefault();

                    // Only needed if IsTimeOnly is true
                    if (businessField.IsTimeOnly)
                    {
                        TimePickersJavaScript(depth, snippet, localEntityName, widget);
                    }
                }
            }
        }

        /// <summary>
        /// Init TimePickers Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="localEntityName">Local Entity Name</param>
        /// <param name="widget">Widget</param>
        private static void TimePickersJavaScript(int depth, StringBuilder snippet, 
            string localEntityName, string widget)
        {
            snippet.AppendLine(new string(' ', depth) + "if (" + localEntityName + "UI." + localEntityName + "Model.Data !== undefined) {");
            snippet.AppendLine(new string(' ', depth + 4) + "sg.utls.kndoUI.timePicker.init('#txt" + widget + "', " +
                localEntityName + "UI." + localEntityName + "Model.Data." + widget +
                ", " + localEntityName + "UI." + localEntityName + "Model.isModelDirty, null);");
            snippet.AppendLine(new string(' ', depth) + "}");
        }

        /// <summary>
        /// Init Numeric Textboxes Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="localEntityName">Local Entity Name</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        public static void SetTimeOnlyValuesJavaScript(int depth, Settings settings, string localEntityName, 
            StringBuilder snippet, BusinessView view)
        {
            // Iterate DateTime widgets, if any
            if (settings.Widgets.ContainsKey("DateTime"))
            {
                foreach (var widget in settings.Widgets["DateTime"])
                {
                    // Get Business View
                    var businessField = view.Fields.Where(x => x.Name == widget).FirstOrDefault();

                    // Only needed if IsTimeOnly is true
                    if (businessField.IsTimeOnly)
                    {
                        snippet.AppendLine(new string(' ', depth) + "sg.utls.kndoUI.timePicker.setModelTime(" +
                            localEntityName + "UI." + localEntityName + "Model.Data." + widget + ",");
                        snippet.AppendLine(new string(' ', depth + 39) + 
                            localEntityName + "UI." + localEntityName + "Model.Data." + widget + "Ext());");
                    }
                }
            }
        }

        /// <summary>
        /// Create Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="localEntityName">Local Entity Name</param>
        /// <param name="snippet">Snippet being constructed</param>
        public static void CreateJavaScript(int depth, Settings settings, string localEntityName,
            StringBuilder snippet)
        {
            // Iterate tab, if any
            if (settings.Widgets.ContainsKey("TabPage"))
            {
                snippet.AppendLine(new string(' ', depth) + "");
                snippet.AppendLine(new string(' ', depth) + @"/**");
                snippet.AppendLine(new string(' ', depth) + @"* @function");
                snippet.AppendLine(new string(' ', depth) + @"* @name onTabSelect");
                snippet.AppendLine(new string(' ', depth) + @"* @description Tab page selection ");
                snippet.AppendLine(new string(' ', depth) + @"* @namespace " + localEntityName + "UI");
                snippet.AppendLine(new string(' ', depth) + @"* @public");
                snippet.AppendLine(new string(' ', depth) + @"*/");

                snippet.AppendLine(new string(' ', depth) + "onTabSelect: function (e) {");
                snippet.AppendLine(new string(' ', depth + 4) + "var selectedTab = e.item.id;");
                snippet.AppendLine(new string(' ', depth + 4) + "switch (selectedTab) {");

                foreach (var widget in settings.Widgets["TabPage"])
                {
                    snippet.AppendLine(new string(' ', depth + 8) + "case " + localEntityName + "TabStrip.ValueType." + widget + ":");
                    snippet.AppendLine(new string(' ', depth + 12) + "// Place logic here for selected tab page");
                    snippet.AppendLine(new string(' ', depth + 12) + "");
                    snippet.AppendLine(new string(' ', depth + 12) + "break;");
                }
                snippet.AppendLine(new string(' ', depth + 4) + "}");
                snippet.AppendLine(new string(' ', depth) + "},");
            }
        }

        /// <summary>
        /// Select Tab Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="firstTabPage">First tab page</param>
        public static void SelectTabJavaScript(int depth, Settings settings, StringBuilder snippet, string firstTabPage)
        {
            // Iterate tab, if any
            if (settings.Widgets.ContainsKey("Tab"))
            {
                foreach (var widget in settings.Widgets["Tab"])
                {
                    snippet.AppendLine(new string(' ', depth) + "sg.utls.kndoUI.selectTab(\"" + widget + "\", " + firstTabPage + ");");
                }
                snippet.AppendLine("");
            }
        }

        /// <summary>
        /// Set Numeric Textboxes Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        public static void SetNumericTextboxesJavaScript(int depth, Settings settings, StringBuilder snippet)
        {
            // Iterate numeric widgets, if any
            if (settings.Widgets.ContainsKey("Numeric"))
            {
                snippet.AppendLine(new string(' ', depth) + "// Set and enable/disable numerics");
                foreach (var widget in settings.Widgets["Numeric"])
                {
                    snippet.AppendLine(new string(' ', depth) + "sg.utls.setNumericTextBox(\"nbr" + widget + "\");");
                    snippet.AppendLine(new string(' ', depth) + "// sg.utls.enableNumericTextbox(\"nbr" + widget + "\", true);");
                }
            }
        }

        /// <summary>
        /// Refresh Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="localEntityName">Local Entity Name</param>
        /// <param name="snippet">Snippet being constructed</param>
        public static void RefreshJavaScript(int depth, Settings settings, string localEntityName,
            StringBuilder snippet)
        {
            // Iterate tab page widgets, if any
            if (settings.Widgets.ContainsKey("TabPage"))
            {
                snippet.AppendLine("");
                snippet.AppendLine(new string(' ', depth) + "// Enable/Disable tabs");
                foreach (var widget in settings.Widgets["TabPage"])
                {
                    snippet.AppendLine(new string(' ', depth) + localEntityName + "Utility.tabStripEnable(true, " + localEntityName + "TabStrip.ValueType." + widget + ");");
                }
            }

            // For grid refresh
            if (settings.Entities.FirstOrDefault(e => e.ForGrid) != null)
            {
                snippet.AppendLine(new string(' ', depth) + "// Grid load data");
            }

            foreach (var entity in settings.Entities)
            {
                if (entity.ForGrid)
                {
                    var gridId = char.ToLowerInvariant(entity.Text[0]) + entity.Text.Substring(1);
                    var output = string.Format("$('#{0}Grid').data('kendoGrid').dataSource.read();", gridId);
                    snippet.AppendLine(new string(' ', depth) + output);
                }
            }
        }

        ///// <summary>
        ///// Set Dropdown Snippet JavaScript
        ///// </summary>
        ///// <param name="depth">Indentation for generation</param>
        ///// <param name="settings">Settings object</param>
        ///// <param name="snippet">Snippet being constructed</param>
        ///// <param name="useResultData">True for using result data otherwise false</param>
        //public static void SetDropdownJavaScript(int depth, Settings settings, StringBuilder snippet, bool useResultData)
        //{
        //    // Iterate drop down widgets, if any
        //    if (settings.Widgets.ContainsKey("Dropdown"))
        //    {
        //        foreach (var widget in settings.Widgets["Dropdown"])
        //        {
        //            var resultData = useResultData ? "jsonResult.Data." + widget : string.Empty;
        //            snippet.Append(new string(' ', depth) + "$(\"#ddl" + widget + "\").data(\"kendoDropDownList\").value(modelData." + widget + "(" + resultData + "));");
        //        }
        //    }
        //}

        /// <summary>
        /// Show Tab Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        public static void ShowTabJavaScript(int depth, Settings settings, StringBuilder snippet)
        {
            // Iterate tab, if any
            if (settings.Widgets.ContainsKey("Tab"))
            {
                foreach (var widget in settings.Widgets["Tab"])
                {
                    snippet.AppendLine(new string(' ', depth) + "$(\"#" + widget + "\").show();");
                }
                snippet.AppendLine("");
            }
        }

        /// <summary>
        /// Utility Tab Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        public static void UtilityTabJavaScript(int depth, Settings settings, StringBuilder snippet)
        {
            // Iterate tab, if any
            if (settings.Widgets.ContainsKey("Tab"))
            {
                snippet.AppendLine(new string(' ', depth) + "tabStripEnable: function (value, id) {");
                foreach (var widget in settings.Widgets["Tab"])
                {
                    snippet.AppendLine(new string(' ', depth + 4) + "var tabStrip = $(\"#" + widget + "\").data(\"kendoTabStrip\");");
                    snippet.AppendLine(new string(' ', depth + 4) + "var items = tabStrip.items();");
                    snippet.AppendLine(new string(' ', depth + 4) + "if (value) {");
                    snippet.AppendLine(new string(' ', depth + 8) + "// Enable");
                    snippet.AppendLine(new string(' ', depth + 8) + "tabStrip.enable(items[id]);");
                    snippet.AppendLine(new string(' ', depth + 4) + "}");
                    snippet.AppendLine(new string(' ', depth + 4) + "else {");
                    snippet.AppendLine(new string(' ', depth + 8) + "// Disable");
                    snippet.AppendLine(new string(' ', depth + 8) + "tabStrip.disable(items[id]);");
                    snippet.AppendLine(new string(' ', depth + 4) + "}");
                    snippet.AppendLine(new string(' ', depth) + "},");
                }
            }
        }

        /// <summary>
        /// Finder Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        public static void FinderJavaScript(int depth, Settings settings, StringBuilder snippet)
        {
            if (settings.XmlLayout != null)
            {
                var finderControls = settings.XmlLayout.Root.XPathSelectElements("//Control[@widget='Finder']"); // get all finder widgets
                foreach (var finderControl in finderControls)
                {
                    // Need to read view finder file (NON MINIFIED)
                    // to get object as may not be sg.viewFinderProperties
                    var objectName = GetObjectName(finderControl.Attribute("finderFileName")?.Value);
                    var finderName = finderControl.Attribute("property")?.Value;
                    var finderProperty = finderControl.Attribute("finderProperty")?.Value != null ? $", {objectName}.{finderControl.Attribute("finderProperty").Value}" : string.Empty;
                    snippet.AppendLine(new string(' ', depth) + $@"sg.viewFinderHelper.setViewFinder(""btnFinder{finderName}"", ""txt{finderName}"" {finderProperty});");
                }
            }
        }

        /// <summary>
        /// DisabledProperties Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        /// <param name="localEntityName">LocalEntityName</param>
        public static void DisabledPropertiesJavaScript(int depth, Settings settings, StringBuilder snippet, 
            BusinessView view, string localEntityName)
        {
            foreach (var key in WIDGET_TYPES)
            {
                DisabledPropertiesWorkerJavaScript(4, settings, snippet, view, key, localEntityName);
            }
        }

        /// <summary>
        /// DisabledProperties Snippet JavaScript
        /// </summary>
        /// <param name="depth">Indentation for generation</param>
        /// <param name="settings">Settings object</param>
        /// <param name="snippet">Snippet being constructed</param>
        /// <param name="view">Business View</param>
        /// <param name="key">Key to dictionary</param>
        /// <param name="localEntityName">Local entity name</param>
        private static void DisabledPropertiesWorkerJavaScript(int depth, Settings settings, StringBuilder snippet, 
            BusinessView view, string key, string localEntityName)
        {
            // Iterate widgets, if any
            if (settings.Widgets.ContainsKey(key))
            {
                foreach (var widget in settings.Widgets[key])
                {
                    // Get Business View
                    var businessField = view.Fields.Where(x => x.Name == widget).FirstOrDefault();

                    snippet.AppendLine(new string(' ', depth) + "model.Is" + widget + "Disabled = ko.computed(function () {");
                    snippet.AppendLine(new string(' ', depth + 4) + "// Default to IsReadOnly property in business view");
                    snippet.AppendLine(new string(' ', depth + 4) + "var isReadOnly = " + businessField.IsReadOnly.ToString().ToLower() + ";");

                    // Enable/disable the dropdown portion of the timepicker control
                    if ((key == "DateTime" && businessField.IsTimeOnly) || key == "Time")
                    {
                        snippet.AppendLine(new string(' ', depth + 4) + "// Get widget");
                        snippet.AppendLine(new string(' ', depth + 4) + "let widget = \"#txt" + widget + "\";");
                        snippet.AppendLine(new string(' ', depth + 4) + "// Enable/disable the dropdown portion of the timepicker control");
                        snippet.AppendLine(new string(' ', depth + 4) + "var timepicker = sg.utls.kndoUI.timePicker.getControlById(widget);");
                        snippet.AppendLine(new string(' ', depth + 4) + "// Init if needed");
                        snippet.AppendLine(new string(' ', depth + 4) + "if (timepicker === undefined) {");
                        snippet.AppendLine(new string(' ', depth + 8) + localEntityName + "UI.initTimePickers();");
                        snippet.AppendLine(new string(' ', depth + 8) + "timepicker = sg.utls.kndoUI.timePicker.getControlById(widget);");
                        snippet.AppendLine(new string(' ', depth + 4) + "}");
                        snippet.AppendLine(new string(' ', depth + 4) + "// Enable/disable");
                        snippet.AppendLine(new string(' ', depth + 4) + "timepicker.enable(!isReadOnly);");
                    }
                    snippet.AppendLine(new string(' ', depth + 4) + "");
                    snippet.AppendLine(new string(' ', depth + 4) + "return isReadOnly;");
                    snippet.AppendLine(new string(' ', depth) + "});");
                    snippet.AppendLine("");
                }
            }
        }

        /// <summary>
        /// Starting Tag snippet
        /// </summary>
        /// <param name="tag">Tag Name</param>
        /// <param name="name">Class Name</param>
        /// <param name="id">ID</param>
        private static string StartingTag(string tag, string name = "", string id = "")
        {
            var idValue = string.IsNullOrEmpty(id) ? string.Empty : "id=\"" + id + "\" ";
            var classValue = string.IsNullOrEmpty(name) ? string.Empty : "class=\"" + name + "\" ";

            return "<" + tag + " " + idValue + classValue + ">";
        }

        /// <summary>
        /// Ending Tag snippet
        /// </summary>
        /// <param name="tag">Tag Name</param>
        private static string EndingTag(string tag)
        {
            return "</" + tag + ">";
        }

        /// <summary>
        /// Sage Label For snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="view">Business View</param>
        /// <param name="name">Class Name</param>
        private static string SageLabelFor(string property, BusinessView view, string name = "")
        {
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();
            var classNames = (GetRequiredClassName(property, view) + " " + name).Replace("  ", " ").Trim();

            return "@Html.SageLabelFor(model => model.Data." + property + 
                ", new { @id = \"lbl" + property + "\", @class = \"" + classNames +  "\" })";
        }

        /// <summary>
        /// Sage Label snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="resxName">Resx Name</param>
        /// <param name="resxKey">Resx Key</param>
        /// <param name="name">Class Name</param>
        private static string SageLabel(string property, string resxName, string resxKey, string name = "")
        {
            return "@Html.SageLabel(\"lbl" + property + resxKey + "\", " + 
                resxName + "." + resxKey + ", new { @class = \"" + name + 
                "\", @for = \"chk" + property + resxKey + "\" })";
        }

        /// <summary>
        /// Ko Sage Button snippet
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="text">Button text</param>
        private static string KoSageButton(string id, string text)
        {
            return "@Html.KoSageButton(\"" + id + "\", null, new { @id = \"" + id + "\", " + 
                "@class = \"btn btn-primary\", @value = \"" + text + "\" })";
        }

        /// <summary>
        /// Ko Sage Radio Button For snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="enumName">Enum Name</param>
        /// <param name="valueName">Enum Value</param>
        /// <param name="companyNamespace">Namespace</param>
        /// <param name="moduleId">Module Id</param>
        private static string KoSageRadioButtonFor(string property, string enumName, string valueName,
            string companyNamespace, string moduleId)
        {
            return "@Html.KoSageRadioButtonFor(model => model.Data." + property + 
                ", (int)" + companyNamespace + "." + moduleId + ".Models.Enums." + enumName + "." + valueName + ", new { @sagechecked = \"Data." + enumName + 
                "\", @sagedisable = \"Data.HasModifyAccess\" }, new { @id = \"chk" + property + valueName + "\" })";
        }

        /// <summary>
        /// sgFinderFor with Label Snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="field">Business Field</param>
        /// <param name="depth">Indentation for generation</param>
        private static string SgFinderFor(string property, BusinessField field, int depth)
        {
            var isNumeric = field != null && field.IsNumeric;

            var methodName = "@Html.SgFinderFor";
            var modelProperty   = $"model => model.Data.{property}";
            var dataAttrs = "new { @sagevalue = " + $"\"Data.{property}\"" + ", @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";
            var htmlAttrs = "new { @id = " + $"\"{GetTextboxId(property, isNumeric)}\"" + 
                ", @class = " + $"\"{GetUpperAttr(field)}\"" + (field.IsAlphaNumeric ? ", @formatTextbox = \"alphaNumeric\"" : "") + " }";
            var size = GetSizeName(field);
            var labelHtmlAttrs = GetRequiredAttr(field);
            var buttonDataAttrs = ", buttonDataAttrs: new { @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";

            var output = $"{methodName}({modelProperty}, " + NewLine(depth) +
                         $"{dataAttrs}, " + NewLine(depth) +
                         $"{htmlAttrs}, size: \"{size}\", " + NewLine(depth) +
                         $"isNumeric: {isNumeric.ToString().ToLower()}{labelHtmlAttrs}{ buttonDataAttrs})";

            return output;
        }

        /// <summary>
        /// sgNumericFor with Label Snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="field">Business Field</param>
        /// <param name="depth">Indentation for generation</param>
        private static string SgNumericFor(string property, BusinessField field, int depth)
        {
            var methodName = "@Html.SgNumericFor";
            var modelProperty = $"model => model.Data.{property}";
            var dataAttrs = "new { @sagevalue = " + $"\"Data.{property}\"" + ", @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";
            var htmlAttrs = "new { @id = " + $"\"{GetTextboxId(property, true)}\"" + "}";
            var size = GetSizeName(field);
            var labelHtmlAttrs = GetRequiredAttr(field);

            var output = $"{methodName}({modelProperty}, " + NewLine(depth) +
                         $"{dataAttrs}, " + NewLine(depth) +
                         $"{htmlAttrs},  size: \"{size}\"{labelHtmlAttrs})";

            return output;
        }

        /// <summary>
        /// sgTextFor with Label Snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="field">Business Field</param>
        /// <param name="depth">Indentation for generation</param>
        private static string SgTextFor(string property, BusinessField field, int depth)
        {
            var methodName = "@Html.SgTextFor";
            var modelProperty = $"model => model.Data.{property}";
            var dataAttrs = "new { @sagevalue = " + $"\"Data.{property}\"" + ", @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";
            var htmlAttrs = "new { @id = " + $"\"{GetTextboxId(property, false)}\"" +
                ", @class = " + $"\"{GetUpperAttr(field)}\"" + (field.IsAlphaNumeric ? ", @formatTextbox = \"alphaNumeric\"" : "") + " }";
            var size = GetSizeName(field);
            var labelHtmlAttrs = GetRequiredAttr(field);

            var output = $"{methodName}({modelProperty}, " + NewLine(depth) +
                         $"{dataAttrs}, " + NewLine(depth) +
                         $"{htmlAttrs},  size: \"{size}\"{labelHtmlAttrs})";

            return output;
        }

        /// <summary>
        /// sgDatepickerFor with Label Snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="field">Business Field</param>
        /// <param name="depth">Indentation for generation</param>
        private static string SgDatepickerFor(string property, BusinessField field, int depth)
        {
            var methodName = "@Html.SgDatepickerFor";
            var modelProperty = $"model => model.Data.{property}";
            var dataAttrs = "new { @sageDatePicker = " + $"\"Data.{property}\"" + ", @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";
            var htmlAttrs = "new { @id = " + $"\"txt{property}\"" + " }";
            var labelHtmlAttrs = GetRequiredAttr(field);

            var output = $"{methodName}({modelProperty}, " + NewLine(depth) +
                         $"{dataAttrs}, " + NewLine(depth) +
                         $"{htmlAttrs}{labelHtmlAttrs})";

            return output;
        }

        /// <summary>
        /// SgTimepickerFor with Label Snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="field">Business Field</param>
        /// <param name="isTimeOnly">True if time only otherwise false</param>
        /// <param name="depth">Indentation for generation</param>
        private static string SgTimepickerFor(string property, BusinessField field, bool isTimeOnly, int depth)
        {
            var methodName = "@Html.SgTimepickerFor";
            var modelProperty = $"model => model.Data.{property}";
            var dataAttrs = "new { @sagevalue = " + $"\"Data.{property}" + (isTimeOnly ? "Ext" : "") + "\", @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";
            var htmlAttrs = "new { @id = " + $"\"txt{property}\"" + " }";
            var labelHtmlAttrs = GetRequiredAttr(field);

            var output = $"{methodName}({modelProperty}, " + NewLine(depth) +
                         $"{dataAttrs}, " + NewLine(depth) +
                         $"{htmlAttrs}{labelHtmlAttrs})";

            return output;
        }

        /// <summary>
        /// sgDropdownFor with Label Snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="field">Business Field</param>
        /// <param name="depth">Indentation for generation</param>
        private static string SgDropdownFor(string property, BusinessField field, int depth)
        {
            var methodName = "@Html.SgDropdownFor";
            var modelProperty = $"model => model.Data.{property}";
            var dataAttrs = "new { @value = " + $"\"Data.{property}\"" + ", @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";
            var htmlAttrs = "new { @id = " + $"\"ddl{property}\", @class = \"single-select\"" + " }";
            var selectList = $"selectList: Model.Get{property}";
            var size = "small";
            var labelHtmlAttrs = GetRequiredAttr(field);

            var output = $"{methodName}({modelProperty}," + NewLine(depth) +
                         $"{dataAttrs}, {htmlAttrs}," + NewLine(depth) +
                         $"{selectList}, size: \"{size}\"{labelHtmlAttrs})";

            return output;
        }

        /// <summary>
        /// Ko Sage Checkbox with Label Snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="field">Business Field</param>
        /// <param name="depth">Indentation for generation</param> 
        private static string SgCheckboxFor(string property, BusinessField field, int depth)
        {
            var methodName = "@Html.SgCheckboxFor";
            var modelProperty = $"model => model.Data.{property}";
            var dataAttrs = "new { @sagechecked = " + $"\"Data.{property}\"" + ", @sagedisable = " + $"\"Data.Is{property}Disabled\"" + " }";
            var htmlAttrs = "new { @id = " + $"\"chk{property}\"" + " }";
            var labelHtmlAttrs = GetRequiredAttr(field);

            var output = $"{methodName}({modelProperty}, " + NewLine(depth) +
                         $"{dataAttrs}, " + NewLine(depth) +
                         $"{htmlAttrs}{labelHtmlAttrs})";

            return output;
        }

        /// <summary>
        /// Sage Hamburger For snippet
        /// </summary>
        /// <param name="property">Property Name</param>
        private static string SageHamburger(string property)
        {
            return "@Html.SageHamburger(\"#\", null, null, new { @id = \"lnk" + property + "\" })";
        }

        /// <summary>
        /// Get the size name
        /// </summary>
        /// <param name="businessField">Business Field</param>
        /// <returns>Size name to be used for size. Default to 'default'</returns>
        private static string GetSizeName(BusinessField businessField)
        {
            // Default return 
            var retVal = "default";

            // Get the size from the business field and set a default
            var size = businessField == null ? 20 : businessField.Size;

            // Evaluate based upon UX Guidelines
            if (size >= 0 && size <= 4)
            {
                retVal = "xsmall";
            }
            else if (size >= 5 && size <= 15)
            {
                retVal = "smaller";
            }
            else if (size >= 16 && size <= 19)
            {
                retVal = "small";
            }
            else if (size >= 20 && size <= 31)
            {
                retVal = "default";
            }
            else if (size >= 32 && size <= 36)
            {
                retVal = "medium";
            }
            else if (size >= 37 && size <= 41)
            {
                retVal = "mediumlarge";
            }
            else if (size >= 42 && size <= 69)
            {
                retVal = "large";
            }
            else if (size >= 70 && size <= 84)
            {
                retVal = "larger";
            }
            else if (size >= 85)
            {
                retVal = "xlarge";
            }

            return retVal;
        }

        /// <summary>
        /// Get the required class name
        /// </summary>
        /// <param name="property">Property Name</param>
        /// <param name="view">Business View</param>
        /// <returns>Class name to be used for required. Default to string.Empty</returns>
        private static string GetRequiredClassName(string property, BusinessView view)
        {
            // Get the IsRequired from the business field
            var businessField = view.Fields.Where(x => x.Name == property).FirstOrDefault();
            var isRequired = businessField != null && businessField.IsRequired;

            return isRequired ? "required" : string.Empty;
        }

        /// <summary>
        /// Get the required attribute
        /// </summary>
        /// <param name="businessField">Business Field</param>
        /// <returns>Class name to be used for required. Default to string.Empty</returns>
        private static string GetRequiredAttr(BusinessField businessField)
        {
            // Get the IsRequired from the business field
            var isRequired = businessField != null && businessField.IsRequired;

            return isRequired ? ", labelHtmlAttrs: new { @class = \"required\" }" : "";
        }

        /// <summary>
        /// Get the upper attribute
        /// </summary>
        /// <param name="field">Business Field</param>
        /// <returns>Upper switch</returns>
        private static string GetUpperAttr(BusinessField field)
        {
            // Get the IsUpperCase from the business field
            return (field != null && (field.IsUpperCase || field.IsAlphaNumeric)) ? "txt-upper" : "";
        }

        /// <summary>
        /// Get the textbox id
        /// </summary>
        /// <param name="property">Property</param>
        /// <param name="isNumeric">True if numeric otherwise false</param>
        /// <returns>Textbox id</returns>
        private static string GetTextboxId(string property, bool isNumeric)
        {
            return (isNumeric ? "nbr" : "txt") + property;
        }

        /// <summary>
        /// Add a new line
        /// </summary>
        /// <returns>New line</returns>
        /// <param name="depth">Indentation for generation</param>
        private static string NewLine(int depth)
        {
            return Environment.NewLine + new string(' ', (depth) * 4);
        }

        /// <summary> Object and method name in JavaScript file may NOT be named "sg.viewFinderProperties" </summary>
        /// <param name="fileName">JavaScript filename</param>
        /// <returns>Object and method name found in file</returns>
        /// <remarks>There are two formats that must be scanned to retrieve the object and method name
        /// 1. ...@namespace ObjectName.MethodName (note: method name must be properly cased)
        /// 2. ObjectName.MethodName = ...
        /// </remarks>
        private static string GetObjectName(string fileName)
        {
            // Default
            string result = "sg.viewFinderProperties";

            try
            {
                // Read view finder file (NON-MINIFIED)
                var lines = File.ReadLines(fileName);

                // Tokens
                string[] token = { "namespace ", "=" };

                // Iterate file
                foreach (var line in lines)
                {
                    // Investigate tokens
                    if (line.Contains(token[0]) || line.Contains(token[1]))
                    {
                        var split = line.Split(token, StringSplitOptions.RemoveEmptyEntries);
                        result = split[line.Contains(token[0]) ? 1 : 0].Trim();
                        break;
                    }
                }
            }
            catch
            {
            }
            return result;
        }
    }
}
