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

using ACCPAC.Advantage;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

namespace Sage300FinderGenerator
{
    public partial class FinderDefinitionControl : UserControl
    {
        public string ViewID { get; private set; }
        public string ViewDescription { get; private set; }

        public UserCredential CurrentUserCredential { get; private set; }

        private SessionManager sessionManager;
        private FinderDataSet finderDataSet;

        /// <summary>
        /// Map from unique finder name finder detail info
        /// </summary>
        private IDictionary<string, dynamic> _finderLookup;

        public FinderDefinitionControl()
        {
            InitializeComponent();
            CreateFinderDetailDataBinding();
        }

        public string GetFinderDefinitionFilePath() => definitionFileTxt.Text;

        public JObject GetFinderDefinition()
        {
            JObject allFindersDef = null;
            if (_finderLookup != null)
            {
                allFindersDef = new JObject();

                var allKeys = _finderLookup.Keys.OrderBy(x => x);
                var allModules = allKeys.Select(x => x.Split(FinderDataSet.ModuleNameSeparator)[0]).Distinct();

                foreach (var moduleName in allModules)
                {
                    var module = new JObject();
                    allFindersDef[moduleName] = module;
                    var finderNameKeys = allKeys.Where(x => x.Split(FinderDataSet.ModuleNameSeparator)[0] == moduleName);
                    foreach (var finderName in finderNameKeys)
                    {
                        var finderInfo = new JObject();
                        var aFinderDef = _finderLookup[finderName] as IDictionary<string, dynamic>;

                        foreach (var item in aFinderDef)
                        {
                            finderInfo[item.Key] = item.Value is Array ? new JArray(item.Value) : item.Value;
                        }

                        module[finderName.Split(FinderDataSet.ModuleNameSeparator)[1]] = finderInfo;
                    }
                    
                }
            }

            return allFindersDef;
        }

        #region static functions

        public static JObject ExtractFinderPropertyFromFile(string fileName)
        {
            JObject result = default;
            try
            {
                using (var fileStream = File.Open(fileName, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        var fileContent = reader.ReadToEnd();
                        fileContent = $"var jQuery = {{}};{fileContent}; function GetSg(){{return JSON.stringify(sg);}};";
                        var jIntEngine = new Jint.Engine();
                        jIntEngine.Execute(fileContent);
                        result = JsonConvert.DeserializeObject(jIntEngine.Invoke("GetSg").AsString()) as JObject;
                    }
                }
            }
            catch
            {
                //TODO log something here!!
            }
            return result;
        }
        /*
        public static string CombineFinderDefs(string existingFinderFilePath, JObject newFinderDef, string moduleName)
        {
            var existingFinderDef = ExtractFinderPropertyFromFile(existingFinderFilePath);

            string result = CreateFinderDefContent(existingFinderDef, newFinderDef, moduleName);
         
            return result ?? string.Empty;
        }
        */
        public static SortedDictionary<string, dynamic> CreateFinderLookup(JObject finderProps)
        {
            var result = new SortedDictionary<string, dynamic>();

            var allModules = finderProps["viewFinderProperties"].Children();

            foreach (JProperty aModule in allModules)
            {
                var moduleFinders = aModule.Value;

                foreach (var aFinder in moduleFinders)
                {
                    var finderName = ((JProperty)aFinder).Name;
                    dynamic finderDefinition = new ExpandoObject();

                    foreach (JProperty aProperty in aFinder.Values())
                    {
                        if (aProperty.Value is JValue)
                        {
                            ((IDictionary<string, Object>)finderDefinition).Add(aProperty.Name, ConvertJPropertyValue(aProperty.Value));
                        }
                        else if (aProperty.Value is JArray)
                        {
                            ((IDictionary<string, Object>)finderDefinition).Add(aProperty.Name, ((JArray)aProperty.Value).Select(jv => (string)jv).ToArray());
                        }
                    }

                    result.Add($"{aModule.Name}.{finderName}", finderDefinition);
                }

            }

            return result;
        }

        private static object ConvertJPropertyValue(JToken value)
        {
            // for now only assume only 3 types; int, bool, string; everything else will be treated as string
            if (value.Type == JTokenType.Integer)
            {
                return value.ToObject(typeof(int));
            }

            if (value.Type == JTokenType.Boolean)
            {
                return value.ToObject(typeof(bool));
            }

            return value.ToString();
        }
        /*
        private static string CreateFinderDefContent(JObject existingFinderProperties, JObject newFinderDef, string module)
        {
            string result = null;
            string template =
                @"""use strict"";
                (function(n) {{
                    // Note this code will be read by Jint c# interpreter, current does not support ECMA 6
                    var result = {{}};
                    var oldFinders = n.viewFinderProperties[""{0}""];
                    var newFinders = {1};

                    for (var key in oldFinders) {{
                      if(oldFinders.hasOwnProperty(key)){{
                        result[key] = oldFinders[key];
                      }}
                    }}

                    for (var key in newFinders) {{
                      if(newFinders.hasOwnProperty(key)){{
                        result[key] = newFinders[key];
                      }}
                    }}

                    n.viewFinderProperties[""{0}""] = result;
                }})(this.sg = this.sg || {{ }}, this.sg.viewFinderProperties = this.sg.viewFinderProperties || {{ }}, jQuery); ";

            if (newFinderDef != null)
            {
                var existingFinderDef = existingFinderProperties?["viewFinderProperties"]?[module] as JObject ?? new JObject();
                existingFinderDef.Merge(newFinderDef, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });
                result = string.Format(template, module, existingFinderDef.ToString());
            }

            return result;
        }
        */
        #endregion

        /// <summary>
        /// Populate data for combox, return and display fields lists
        /// </summary>
        /// <param name="view"></param>
        private void PopulateControlsWithViewData(ACCPAC.Advantage.View view, IDictionary<string, object> finderInfoLookup)
        {
            if (view != null)
            {
                cboKeys.Items.Clear();
                chklstDisplayFields.Items.Clear();
                chklstReturnFields.Items.Clear();
                
                for (var i = 0; i < view.Keys.Count; i++)
                {
                    var viewKey = view.Keys.IViewKeyByIndex(i);
                    cboKeys.Items.Insert(viewKey.ID, viewKey.Name);
                }

                for (var i = 0; i < view.Fields.Count; i++)
                {
                    var field = view.Fields.IFieldByIndex(i);
                    chklstDisplayFields.Items.Add(field.Name);
                    chklstReturnFields.Items.Add(field.Name);
                }

                if (finderInfoLookup != null)
                {
                    // set check states for box ListBoxes
                    var returnFields = finderInfoLookup[FinderDataSet.ReturnFieldNames_FieldName] as string[];
                    for (int i = 0; i < returnFields.Length; i++)
                    {
                        chklstReturnFields.Items.Remove(returnFields[i]);
                        chklstReturnFields.Items.Insert(i, returnFields[i]);
                        chklstReturnFields.SetItemChecked(i, true);
                    }

                    var displayFields = finderInfoLookup[FinderDataSet.DisplayFieldNames_FieldName] as string[];
                    for (int i = 0; i < displayFields.Length; i++)
                    {
                        chklstDisplayFields.Items.Remove(displayFields[i]);
                        chklstDisplayFields.Items.Insert(i, displayFields[i]);
                        chklstDisplayFields.SetItemChecked(i, true);
                    }
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text) &&
                !string.IsNullOrEmpty(mskPassword.Text) &&
                !string.IsNullOrEmpty(txtCompany.Text))
            {
                sessionManager = new SessionManager();
                if (!sessionManager.InitSessionAndDBLink(txtUserName.Text, mskPassword.Text, txtCompany.Text))
                {
                    MessageBox.Show("Username/password/company combination is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DisableLogin();
                }
            }
            else
            {
                MessageBox.Show("Please enter Username/password/company", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisableLogin()
        {
            btnConfirm.Enabled = false;
            txtUserName.Enabled = false;
            mskPassword.Enabled = false;
            txtCompany.Enabled = false;
        }

        private void fileBtn_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = GetInitPath();
                openFileDialog.Filter = "js files (*.js)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    definitionFileTxt.Text = openFileDialog.FileName;
                    var finderProps = ExtractFinderPropertyFromFile(openFileDialog.FileName);
                    if (finderProps != null)
                    {
                        _finderLookup = CreateFinderLookup(finderProps);
                        PopuplateFinderDropDown(lstFinder, _finderLookup);
                        fileBtn.Enabled = false;
                        btmNew.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// To populate data to list of all current finders
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="dataSource"></param>
        public static void PopuplateFinderDropDown(ListControl lc, IDictionary<string, dynamic> dataSource)
        {
            lc.Enabled = true;
            lc.DataSource = new BindingSource(dataSource, null);
            lc.DisplayMember = "Key";
            lc.ValueMember = "Value";
        }

        private void CreateFinderDetailDataBinding()
        {
            finderDataSet = new FinderDataSet();
            txtViewId.DataBindings.Add("Text", finderDataSet, nameof(finderDataSet.ViewID) , false, DataSourceUpdateMode.OnPropertyChanged);
            cboKeys.DataBindings.Add("SelectedIndex", finderDataSet, nameof(finderDataSet.ViewOrder), false, DataSourceUpdateMode.OnPropertyChanged);
            txtFinderName.DataBindings.Add("Text", finderDataSet, nameof(finderDataSet.FinderName), false, DataSourceUpdateMode.OnPropertyChanged);
            txtFinderModule.DataBindings.Add("Text", finderDataSet, nameof(finderDataSet.FinderModule), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private string GetInitPath()
        {
            var regPath = string.Empty;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\ACCPAC International, Inc.\\ACCPAC\\Configuration"))
            {
                if (key != null)
                {
                    regPath = (string)key.GetValue("Programs");
                }
            }

            return !string.IsNullOrEmpty(regPath) ? Path.Combine(regPath, "Online", "Web", "Areas", "Core", "Scripts") : regPath;
        }

        private IDictionary<string, object> GetCurrentSelectedFinderInfo()
        {
            var finderInfoPair = (KeyValuePair<string, object>)lstFinder.SelectedItem;
            return (IDictionary<string, object>)finderInfoPair.Value;
        }

        private string GetCurrentSelectedFinderName()
        {
            var finderInfoPair = (KeyValuePair<string, object>)lstFinder.SelectedItem;
            return finderInfoPair.Key;
        }

        private void lstFinder_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the selected finder info
            var finderInfoLookup = GetCurrentSelectedFinderInfo();
            var fullFinderName = GetCurrentSelectedFinderName();

            // Set values to all controls
            PopulateControlsWithViewData(sessionManager.TryGetView((string)finderInfoLookup[FinderDataSet.ViewID_FieldName]), finderInfoLookup);

            // Now update the dataset
            finderDataSet.SetDataValue(finderInfoLookup, fullFinderName.Split(FinderDataSet.ModuleNameSeparator)[1], fullFinderName.Split(FinderDataSet.ModuleNameSeparator)[0]);

            // Reset buttons (just in case)
            btmNew.Enabled = true;
            btmInsert.Enabled = false;
        }

        #region drag and drop Handler

        private void listBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listBox_DragDrop(object sender, DragEventArgs e)
        {
            var checkedListBox = sender as CheckedListBox;
            Point point = checkedListBox.PointToClient(new Point(e.X, e.Y));
            int index = checkedListBox.IndexFromPoint(point);
            if (index < 0) index = checkedListBox.Items.Count - 1;
            object data = e.Data.GetData(typeof(string));
            var checkState = checkedListBox.GetItemChecked(checkedListBox.Items.IndexOf(data));
            checkedListBox.Items.Remove(data);
            checkedListBox.Items.Insert(index, data);
            checkedListBox.SetItemChecked(index, checkState);

            UpdateFieldNames(checkedListBox.Name == "chklstDisplayFields"? FinderDataSet.DisplayFieldNames_FieldName : FinderDataSet.ReturnFieldNames_FieldName, checkedListBox.CheckedItems, finderDataSet.FinderDetailLookup);
        }

        private void listBox_MouseDown(object sender, MouseEventArgs e)
        {
            var checkedListBox = sender as CheckedListBox;

            int index = checkedListBox.IndexFromPoint(new Point(e.X, e.Y));

            if (e.Button == MouseButtons.Right && index >= 0)
            {
                var item = checkedListBox.Items[index];
                checkedListBox.DoDragDrop(item, DragDropEffects.Move);
            }
        }
        #endregion

        private void chklstDisplayFields_Leave(object sender, EventArgs e)
        {
            UpdateFieldNames(FinderDataSet.DisplayFieldNames_FieldName, chklstDisplayFields.CheckedItems, finderDataSet.FinderDetailLookup);
        }
        private void chklstReturnFields_Leave(object sender, EventArgs e)
        {
            UpdateFieldNames(FinderDataSet.ReturnFieldNames_FieldName, chklstReturnFields.CheckedItems, finderDataSet.FinderDetailLookup);
        }

        private void UpdateFieldNames(string fieldName, CheckedListBox.CheckedItemCollection checkedItems, IDictionary<string, object> finderInfo)
        {
            var newArray = new string[checkedItems.Count];
            checkedItems.CopyTo(newArray, 0);
            finderInfo[fieldName] = newArray;
        }

        private void btmNew_Click(object sender, EventArgs e)
        {
            // create a *empty* finder lookup detail object
            IDictionary<string, object> newFinderDetail = new Dictionary<string, object>();
            newFinderDetail.Add(FinderDataSet.ViewID_FieldName, string.Empty);
            newFinderDetail.Add(FinderDataSet.ViewOrder_FieldName, -1);
            newFinderDetail.Add(FinderDataSet.ParentValAsInitKey_FieldName, false);
            newFinderDetail.Add(FinderDataSet.ReturnFieldNames_FieldName, new string[0]);
            newFinderDetail.Add(FinderDataSet.DisplayFieldNames_FieldName, new string[0]);

            //cleanup
            cboKeys.Items.Clear();
            chklstDisplayFields.Items.Clear();
            chklstReturnFields.Items.Clear();

            // Now update the dataset
            finderDataSet.SetDataValue(newFinderDetail, string.Empty, string.Empty);

            btmNew.Enabled = false;
            btmInsert.Enabled = true;
        }

        private void txtViewId_Leave(object sender, EventArgs e)
        {
            // check if the view id has been update, if yes, update data for view order dropdown, and return/display list

            if (finderDataSet.IsViewIDUpdate)
            {
                PopulateControlsWithViewData(sessionManager.TryGetView(finderDataSet.ViewID), null);
                finderDataSet.IsViewIDUpdate = false;
            }
        }

        private void btmInsert_Click(object sender, EventArgs e)
        {
            // validation???? 
            string finderKey = $"{finderDataSet.FinderModule}{FinderDataSet.ModuleNameSeparator}{finderDataSet.FinderName}";

            // Check if name already existed
            if (_finderLookup.ContainsKey(finderKey))
            {
                MessageBox.Show($"Name of {finderKey} already existed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _finderLookup.Add(finderKey, finderDataSet.FinderDetailLookup);

            PopuplateFinderDropDown(lstFinder, _finderLookup);
            lstFinder.SetSelected(lstFinder.Items.Count - 1, true);

            btmNew.Enabled = true;
            btmInsert.Enabled = false;
        }
    }

    public class SessionManager : IDisposable
    {
        #region IDisposable
        public void Dispose() => Dispose(true);

        private bool _disposed;

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                ClearData();
            }

            _disposed = true;
        }
        #endregion

        public Session Session { private set; get; }
        public DBLink DBLink { private set; get; }
        
        public bool InitSessionAndDBLink(string userName, string password, string company)
        {
            bool result = true;
            try
            {
                ClearData();
                Session = new Session();
                Session.InitEx2(null, string.Empty, "WX", "WX1000", GetAccapcVersion(), 1);
                Session.Open(userName, password, company, DateTime.UtcNow, 0);
                DBLink = Session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadOnly);
            }
            catch
            {
                // TODO do some loggin here
                result = false;
            }
            
            return result;
        }

        public ACCPAC.Advantage.View TryGetView(string viewId)
        {
            ACCPAC.Advantage.View result = null;
            try
            {
                if (DBLink != null)
                {
                    result = DBLink.OpenView(viewId);
                }
            }
            catch
            {
                // TODO do some loggin here
            }
            return result;
        }

        private string GetAccapcVersion()
        {
            var pattern = new Regex(@"^(?<major>\d)\.(?<minor>\d)");
            var match = pattern.Match(typeof(ACCPAC.Advantage.View).Assembly.GetName().Version.ToString());

            return $"{match.Groups["major"]}{match.Groups["minor"]}A";
        }

        public void ClearData()
        {
            DBLink?.Dispose();
            Session?.Dispose();
        }
    }

    public class UserCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public string Version { get; set; }
        public string ViewId { get; set; }
    }
}
