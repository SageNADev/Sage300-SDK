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

using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sage300FinderGenerator
{
    /// <summary>
    /// Note: This is used for dev/testing do not use it in runtime
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var dte = CreateDTEInstance();
            dte.Solution.Open(@"C:\Users\AHowu\source\repos\Sage300CMvcApplication1\Sage300CMvcApplication1\Sage300CMvcApplication1.sln");
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new FinderDefinitionForm(dte.Solution as _Solution);
            Application.Run(form);
        }

        private static DTE CreateDTEInstance()
        {
            var vsProgID = "VisualStudio.DTE.16.0"; // VS 2019
            var type = Type.GetTypeFromProgID(vsProgID, true);
            var obj = Activator.CreateInstance(type, true);

            return obj as DTE;
        }
    }

    public class FinderGenerator
    {
        public void Execute(Solution solution)
        {
            using (var form = new FinderDefinitionForm(solution as _Solution))
            {
                form.ShowDialog();
            }
        }
    }
}
