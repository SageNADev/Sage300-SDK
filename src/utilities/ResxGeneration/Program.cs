// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Windows.Forms;

namespace Sage.CA.SBS.ERP.Sage300.ResxGeneration
{
    static class Program
    {
        /// <summary> The main entry point for the application </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run headless or via generation form?
            if (args.Length == 0)
            {
                Application.Run(new Generation());
            }
            else
            {
                var generation = new ProcessGeneration();

                // Get rid of quotes
                for (var arg = 0; arg < args.Length; arg++)
                {
                    args[arg] = args[arg].Replace("\"", "");
                }

                // Get resource info and validate
                var resourceInfo = generation.GetResourceInfo(@args[0]);
                if (!generation.ValidResourceInfo(resourceInfo))
                {
                    throw new Exception(Properties.Resources.ErrorResourceFile);
                }

                // Get settings and validate
                var settings = generation.BuildSettings(generation.GetSettings(@args[1]));
                if (!generation.ValidSettings(settings))
                {
                    throw new Exception(Properties.Resources.ErrorSettingsFile);
                }

                // Add resource info and settings to a dictionary to be passed to processing class
                var dictionary = new Dictionary<string, object>
                    {
                        {ProcessGeneration.ResourceInfoKey, resourceInfo},
                        {ProcessGeneration.SettingsKey, settings}
                    };

                // Start process
                generation.Process(dictionary);
                               
            }
        }
    }
}
