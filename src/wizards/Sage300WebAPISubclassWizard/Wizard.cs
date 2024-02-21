using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Application = System.Windows.Forms.Application;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using System.IO.Compression;
using WebAPISubclassWizard.Properties;
using ICSharpCode.Decompiler.CSharp.Syntax.PatternMatching;
using System.Text.RegularExpressions;

namespace WebAPISubclassWizard
{
    public partial class Wizard : Form
    {
        private bool[] pageVisited = new bool[]{false, false, false, false};

        private string baseModule = string.Empty;
        private string selectedController = null;
        private string sage300webapibinfolder = string.Empty;
        private List<ControllerProperties> basemoduleControllers = null;
        private static string GetWebAPIBinFolderPath()
        {
            using (var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\ACCPAC International, Inc.\\ACCPAC\\Configuration"))
            {
                var installFolder = key?.GetValue("Programs") as string;
                return Path.Combine(installFolder, @"Online\WebApi\bin");
            }
        }

       
        public Wizard()
        {
            InitializeComponent();
            sage300webapibinfolder = GetWebAPIBinFolderPath();
            InitializePage1();
        }

        private string CleanName(string text)
        {
            int pos = text.IndexOf('`');
            if (pos > 0)
                text = text.Substring(0, pos);
            text = text.Trim();


            return text;
        }
        private List<ControllerProperties> GetAllControllers(string assemblyName)
        {
            var decompiler = new CSharpDecompiler(assemblyName, new DecompilerSettings());

            var controllerList = new List<ControllerProperties>();
            
            foreach (var type in decompiler.TypeSystem.MainModule.TypeDefinitions)
            {

                if (type.Accessibility != Accessibility.Public)
                    continue;

                var controllersuffix = "Controller";
                if (!type.FullName.EndsWith(controllersuffix))
                    continue;

                var typeasString = decompiler.DecompileTypeAsString(type.FullTypeName);

                // get view resource 

                var controller = new ControllerProperties();
                controller.ControllerName = type.FullName.Substring(0, type.FullName.Length - controllersuffix.Length);
                controller.RegisterODataEntityBaseTemplateParameters = new List<string>();
                controller.GetViewEntityHierarchyBaseTemplateParameters = new List<string>();

                System.Text.RegularExpressions.Match match = Regex.Match(typeasString, @"\[RestrictedViewResourceController.*\]");

                if (match.Success)
                {
                    controller.RestrictedViewResource = match.Value;
                }
                else
                {
                    controller.RestrictedViewResource = string.Empty;
                }

                foreach (var m in type.Members)
                {
                    if (m.FullName.EndsWith("RegisterODataEntityBase"))
                    {
                        IMethod method = (IMethod)m;
                        foreach (var p in method.TypeParameters)
                        {
                            controller.RegisterODataEntityBaseTemplateParameters.Add(p.Name.Substring(1));
                        }
                    }
                    else if (m.FullName.EndsWith("GetViewEntityHierarchyBase"))
                    {
                        IMethod method = (IMethod)m;
                        foreach (var p in method.TypeParameters)
                        {
                            controller.GetViewEntityHierarchyBaseTemplateParameters.Add(p.Name.Substring(1));
                        }
                    }

                }
                controllerList.Add(controller);

            }

            return controllerList;
        }
    

        private void WizardTab_Selected(object sender, TabControlEventArgs e)
        {
            switch (WizardTab.SelectedIndex)
            {
                case 0:
                    InitializePage1();
                    break;

                case 1:
                    InitializePage2();
                    break;
                
                case 2:
                    InitializePage3();
                    break;

                case 3:
                    InitializePage4();
                    break;
            }
        }

        private List<string> GetAllModules(string path)
        {
            List<string> modules = new List<string>();

            // find all the modules in the path.
            // the filename should be in the format of Sage.CA.SBS.ERP.Sage300.??.WebApi.BaseModels.dll
            // where ?? is the module number

            string[] files = Directory.GetFiles(path, "Sage.CA.SBS.ERP.Sage300.??.WebApi.dll");
            foreach (string file in files)
            {
                modules.Add(Path.GetFileName(file).Substring(24, 2));
            }

            return modules;
        }

        private void InitializePage1()
        {
            if (pageVisited[0])                 
                return;

            // populate the dropdownlistModules
            dropdownlistModules.DataSource = GetAllModules(@sage300webapibinfolder);

            pageVisited[0] = true;
        }

        private void InitializePage2()
        {
            if (pageVisited[1])
                return;

            // get the module name
            var module = dropdownlistModules.SelectedItem;

            basemoduleControllers = GetAllControllers(sage300webapibinfolder + "\\" + "Sage.CA.SBS.ERP.Sage300." + module + ".WebApi.dll");

            // populate the list 
            listBoxClasses.DataSource = basemoduleControllers.Select(c => c.ControllerName).ToArray();

            pageVisited[1] = true;
        }

        private void InitializePage3()
        {
            if (pageVisited[2])
                return;

            pageVisited[2] = true;
        }

        private void InitializePage4()
        {
            if (pageVisited[3])
                return;

            pageVisited[3] = true;
        }

        private void btnPage1Next_Click(object sender, EventArgs e)
        {
            if (dropdownlistModules.SelectedIndex < 0)
                return;

            // reset the second page if the module selection has changed
            if (baseModule != dropdownlistModules.SelectedItem.ToString())
                pageVisited[1] = false;

            baseModule = dropdownlistModules.SelectedItem.ToString();

            WizardTab.SelectTab(1);
        }

        private void btnPage2Back_Click(object sender, EventArgs e)
        {
            WizardTab.SelectTab(0);
        }

        private void btnPage2Next_Click(object sender, EventArgs e)
        {
            if (listBoxClasses.SelectedIndex == -1)
                return;

            if (selectedController != listBoxClasses.SelectedItem.ToString())
            {
                pageVisited[2] = false;
                selectedController = listBoxClasses.SelectedItem.ToString();
            }

            WizardTab.SelectTab(2);
        }

        private void btnProjectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                textBoxProjectFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        
        private void UnzipFiles(string destFolder, Dictionary<string, string> macros)
        {
            var fileName = Path.GetTempFileName();
            File.WriteAllBytes(fileName, Properties.Resources.projecttemplate);
            ZipFile.ExtractToDirectory(fileName, destFolder);

            // replace filenames 
            var files = Directory.GetFiles(destFolder, "*.*", SearchOption.AllDirectories);
            
            foreach (var file in files)
            {
                if (file.Contains(@"\."))
                    continue;

                var newFile = file;

                foreach (var m in macros.Keys)
                {
                    newFile = newFile.Replace(m, macros[m]);
                }

                File.Move(file, newFile);

                // Read the contents of the file
                string fileContents = File.ReadAllText(newFile);

                // Replace the desired content
                string newContents = fileContents;

                foreach (var m in macros.Keys)
                {
                    newContents = newContents.Replace(m, macros[m]);
                }

                // Write the updated contents back to the file
                File.WriteAllText(newFile, newContents);
            }
        }
        private void CreateProject(string projectPath, string module, string company, string controller)
        {
            // verify projectPath is a folder
            if (!Directory.Exists(projectPath))
            {
                MessageBox.Show($"Project folder {projectPath} does not exist!");
                return;
            }

            if (Directory.GetFiles(projectPath).Length > 0)
            {
                MessageBox.Show($"Project folder {projectPath} is not empty!");
                return;
            }

            try
            {
                var macros = new Dictionary<string, string>();

                var baseController = basemoduleControllers.Where(c => c.ControllerName == selectedController).First();
                macros["%BASEMODULE%"] = baseModule;
                macros["%BASECONTROLLER%"] = baseController.ControllerName;
                macros["%CONTROLLER%"] = module + controller;
                macros["%BASEMODEL%"] = baseController.GetViewEntityHierarchyBaseTemplateParameters[0];
                macros["%COMPANY%"] = company;
                macros["%MODULE%"] = module;

                List<string> parameters = new List<string>(baseController.GetViewEntityHierarchyBaseTemplateParameters);
                parameters[0] += "Ext";
                macros["%GET_VIEW_ENTITY_HIERARCHY_PARAMETERS%"] = string.Join(",", parameters);

                parameters = new List<string>(baseController.RegisterODataEntityBaseTemplateParameters);
                parameters[0] += "Ext";
                macros["%REGISTER_ODATA_ENTITY_PARAMETERS%"] = string.Join(",", parameters);

                macros["%PROJECTGUID%"] = Guid.NewGuid().ToString();
                macros["%SAGE300WEBAPIBINFOLDER%"] = sage300webapibinfolder;
                macros["%RESTRICTEDVIEWRESOURCECONTROLLER%"] = baseController.RestrictedViewResource;

                UnzipFiles(projectPath, macros);

                MessageBox.Show("Project has been created successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnPage3Back_Click(object sender, EventArgs e)
        {
            WizardTab.SelectTab(1);
        }

        private void btnPage4Back_Click(object sender, EventArgs e)
        {
            WizardTab.SelectTab(2);
        }

        private bool ConfirmExit()
        {
            if (MessageBox.Show("Are you sure to exit?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;
            return false;
        }
       
        private void Wizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmExit())
            {
                e.Cancel = true;
            }
        }
        
        private void btnCreateProject_Click(object sender, EventArgs e)
        {
            if (textBoxModule.Text.Trim() == string.Empty ||
                textBoxProjectFolder.Text.Trim() == string.Empty ||
                textBoxCompany.Text.Trim() == string.Empty ||
                textBoxController.Text.Trim() == string.Empty)
                return;

            CreateProject(textBoxProjectFolder.Text.Trim(), textBoxModule.Text.Trim(), textBoxCompany.Text.Trim(), textBoxController.Text.Trim());
        }

        private void btnPage3Next_Click_1(object sender, EventArgs e)
        {       
            Application.Exit();
        }
    }

    class ControllerProperties
    {
        public string ControllerName { get; set; }
        public List<string> RegisterODataEntityBaseTemplateParameters { get; set; }
        public List<string> GetViewEntityHierarchyBaseTemplateParameters { get; set; }

        public string RestrictedViewResource { get; set; }
    }
}
