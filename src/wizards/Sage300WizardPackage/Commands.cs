// The MIT License (MIT) 
// Copyright (c) 1994-2024 The Sage Group plc or its licensors.  All rights reserved.
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
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.Composition;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard;
using Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard;
using Sage.CA.SBS.ERP.Sage300.SyncAssembliesWizard;

namespace Sage.CA.SBS.ERP.Sage300.WizardPackage
{
    /// <summary> Sage 300 Wizard Package Commands </summary>
    internal sealed class Commands
    {
        /// <summary> Code Generation Wizard ID </summary>
        public const int CodeGenerationId = 0x0100;
        /// <summary> Finder Generator ID </summary>
        public const int FinderGenerationId = 0x0200;
        /// <summary>  Upgrade Wizard ID </summary>
        public const int UpgradeWIzardId = 0x0300;
        /// <summary>  Language Wizard ID </summary>
        public const int LanguageWIzardId = 0x0400;
        /// <summary>  Sync Assemblies Wizard ID </summary>
        public const int SyncAssembliesWIzardId = 0x0500;

        /// <summary> Command menu group (command set GUID) </summary>
        public static readonly Guid CommandSet = new Guid("d350dd2f-d779-4823-8043-ba699c554bd5");

        /// <summary> VS Package that provides this command, not null </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="Commands"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private Commands(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            // Menu Command for Code Generation Wizard
            var menuCommandID = new CommandID(CommandSet, CodeGenerationId);
            var menuItem = new MenuCommand(MenuItemCallbackForCodeGeneration, menuCommandID);
            commandService.AddCommand(menuItem);

            // Menu Command for Finder Generator
            menuCommandID = new CommandID(CommandSet, FinderGenerationId);
            menuItem = new MenuCommand(MenuItemCallbackForFinderGenerator, menuCommandID);
            commandService.AddCommand(menuItem);

            // Menu Command for Upgrade Wizard
            menuCommandID = new CommandID(CommandSet, UpgradeWIzardId);
            menuItem = new MenuCommand(MenuItemCallbackForUpgrade, menuCommandID);
            commandService.AddCommand(menuItem);

            // Menu Command for Language Wizard
            menuCommandID = new CommandID(CommandSet, LanguageWIzardId);
            menuItem = new MenuCommand(MenuItemCallbackForLanguage, menuCommandID);
            commandService.AddCommand(menuItem);

            // Menu Command for Sync Assemblies Wizard
            menuCommandID = new CommandID(CommandSet, SyncAssembliesWIzardId);
            menuItem = new MenuCommand(MenuItemCallbackForSyncAssemblies, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary> Gets the instance of the command </summary>
        public static Commands Instance { get; private set; }

        /// <summary> Gets the service provider from the owner package </summary>
        private IAsyncServiceProvider ServiceProvider
        {
            get { return this.package; }
        }

        /// <summary> Initializes the singleton instance of the command </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand requires the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new Commands(package, commandService);
        }

        /// <summary> Call back to launch the Code Generation Wizard </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MenuItemCallbackForCodeGeneration(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (Package.GetGlobalService(typeof(DTE)) is DTE dte)
            {
                // Invoke Code Generation Wizard with solution
                new CodeGenerationWizard.CodeGenerationWizard().Execute(dte.Solution);
            }
        }

        /// <summary> Call back to launch the Finder Generator </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MenuItemCallbackForFinderGenerator(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (Package.GetGlobalService(typeof(DTE)) is DTE dte)
            {
                // Invoke Finder Generator with solution
                new FinderGenerator.FinderGenerator().Execute(dte.Solution);
            }
        }

        /// <summary> Call back to launch the Upgrade Wizard </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MenuItemCallbackForUpgrade(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (Package.GetGlobalService(typeof(DTE)) is DTE dte)
            {
                // Invoke Upgrade Wizard with solution
                new Sage300Upgrade().Execute(dte.Solution);
            }
        }

        /// <summary> Call back to launch the Language Wizard </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MenuItemCallbackForLanguage(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (Package.GetGlobalService(typeof(DTE)) is DTE dte)
            {
                // Invoke Language Wizard with solution
                new LanguageResource().Execute(dte.Solution);
            }
        }

        /// <summary> Call back to launch the Sync Assemblies Wizard </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MenuItemCallbackForSyncAssemblies(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (Package.GetGlobalService(typeof(DTE)) is DTE dte)
            {
                // Invoke Sync Assemblies Wizard with solution
                new SyncAssemblies().Execute(dte.Solution);
            }
        }
    }
}
