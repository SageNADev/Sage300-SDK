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
using MergeISVProject.Constants;
using MergeISVProject.CustomExceptions;
using MergeISVProject.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;
#endregion

namespace MergeISVProject
{
	/// <summary>
	/// A class to manage the various folders
	/// (Deployment/Working area and Sage 300 Live Installation)
	/// </summary>
	public class FolderManager
	{
		#region Private Constants
		private const string LogOutputTemplate = "{0:15}: {1}";
		#endregion

		#region Classes meant to be used internally 
		/// <summary>
		/// Class to encapsulate all folders related to the 
		/// Live Sage 300 installation
		/// </summary>
		public class LiveOnlineFolderBlock
		{
			/// <summary>
			/// Represents the root Sage300 installation folder
			/// </summary>
			public string Root { get; set; }
			/// <summary>
			/// Represents the Sage300 Online Web folder
			/// </summary>
			public string Web { get; set; }
			/// <summary>
			/// Represents the Sage300 Online Worker folder
			/// </summary>
			public string Worker { get; set; }

			/// <summary>
			/// Generate a formatted string of the contents
			/// of this object.
			/// </summary>
			public IEnumerable<string> GenerateLogOutput()
			{
				var lines = new List<string>();
				lines.Add(typeof(LiveOnlineFolderBlock).ToString());
				lines.Add(string.Format(LogOutputTemplate, "Root", Root));
				lines.Add(string.Format(LogOutputTemplate, "Web", Web));
				lines.Add(string.Format(LogOutputTemplate, "Worker", Worker));
				return lines;
			}
		}

		/// <summary>
		/// Class to encapsulate all folders related to
		/// the Staging/Compiled/Final process
		/// </summary>
		public class StagingFolderBlock
		{
			public string Root { get; set; }
			public string Bin { get; set; }
			public string Areas { get; set; }
			public string AreasViews { get; set; }
			public string AreasScripts { get; set; }

			/// <summary>
			/// Create the Root folder only
			/// </summary>
			public void CreateRootOnly()
			{
				Directory.CreateDirectory(Root);
			}

			/// <summary>
			/// Create the necessary folders
			/// </summary>
			public void CreateFolders()
			{
				Directory.CreateDirectory(Bin);
				Directory.CreateDirectory(Areas);
				Directory.CreateDirectory(AreasViews);
				Directory.CreateDirectory(AreasScripts);
			}

			/// <summary>
			/// Generate a formatted string of the contents
			/// of this object.
			/// </summary>
			public IEnumerable<string> GenerateLogOutput()
			{
				var lines = new List<string>();
				lines.Add(typeof(StagingFolderBlock).ToString());
				lines.Add(string.Format(LogOutputTemplate, "Root", Root));
				lines.Add(string.Format(LogOutputTemplate, "Bin", Bin));
				lines.Add(string.Format(LogOutputTemplate, "Areas", Areas));
				lines.Add(string.Format(LogOutputTemplate, "AreasViews", AreasViews));
				lines.Add(string.Format(LogOutputTemplate, "AreasScripts", AreasScripts));
				return lines;
			}

		}

		#endregion

		#region Private Properties
		private ILogger _Logger { get; set; }
		#endregion

		#region Public Properties
		/// <summary>
		/// This represents the original location of the
		/// source code
		/// </summary>
		public string RootSource { get; set; }
		/// <summary>
		/// This represents the Deployment folder
		/// </summary>
		public string Deploy { get; set; }
		/// <summary>
		/// Represents the Original file locations
		/// </summary>
		public StagingFolderBlock Originals { get; set; }
		/// <summary>
		/// Represents the Staging folder file locations
		/// </summary>
		public StagingFolderBlock Staging { get; set; }
		/// <summary>
		/// Represents the Compiled assets folder file locations
		/// </summary>
		public StagingFolderBlock Compiled { get; set; }
		/// <summary>
		/// Represents the Final folder file locations
		/// </summary>
		public StagingFolderBlock Final { get; set; }
		/// <summary>
		/// Represents the Sage 300 installation folder file locations
		/// </summary>
		public LiveOnlineFolderBlock Live { get; set; }
		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logger">The Logger object</param>
		/// <param name="rootPathIn">the root path</param>
		/// <param name="sage300Installation">The Sage 300 installation folder</param>
		/// <param name="moduleId">The Module ID</param>
		public FolderManager(ILogger logger, string rootPathIn, string sage300Installation, string moduleId)
		{
			_Logger = logger;

			// Original Source Folders
			RootSource = rootPathIn;

			// Target __Deploy based folders
			// /Deploy/						--> Base deployment folder where all intermediate ging and final deployment files live
			// /Deploy/Staging/				--> Working folder for doing any compilation prior to actual moving to Final
			// /Deploy/__READYTODEPLOY__/   --> Final destination of all staging files (after any compilation)
			Deploy = Path.Combine(rootPathIn, FolderNameConstants.DEPLOY);
			RecreateDeploymentPath();

			// This object represents the original source folders
			Originals = new StagingFolderBlock();
			Originals.Root = rootPathIn;
			Originals.Bin = Path.Combine(rootPathIn, FolderNameConstants.BIN);
			Originals.Areas = Path.Combine(rootPathIn, FolderNameConstants.AREAS);
			Originals.AreasViews = Path.Combine(rootPathIn, FolderNameConstants.AREAS, moduleId, FolderNameConstants.VIEWS);
			Originals.AreasScripts = Path.Combine(rootPathIn, FolderNameConstants.AREAS, moduleId, FolderNameConstants.SCRIPTS);

			// This is where compiled objects will be put (aspnet_compiler)
			//Compiled = Path.Combine(Deploy, FolderNameConstants.COMPILED);
			// Deploy/Compiled
			Compiled = new StagingFolderBlock();
			Compiled.Root = Path.Combine(Deploy, FolderNameConstants.COMPILED);
			Compiled.Bin = Path.Combine(Compiled.Root, FolderNameConstants.BIN);
			Compiled.Areas = Path.Combine(Compiled.Root, FolderNameConstants.AREAS);
			Compiled.AreasViews = Path.Combine(Compiled.Areas, moduleId, FolderNameConstants.VIEWS);
			Compiled.AreasScripts = Path.Combine(Compiled.Areas, moduleId, FolderNameConstants.SCRIPTS);

			// Deploy/Staging
			Staging = new StagingFolderBlock();
			Staging.Root = Path.Combine(Deploy, FolderNameConstants.STAGING);
			Staging.Bin = Path.Combine(Staging.Root, FolderNameConstants.BIN);
			Staging.Areas = Path.Combine(Staging.Root, FolderNameConstants.AREAS);
			Staging.AreasViews = Path.Combine(Staging.Areas, moduleId, FolderNameConstants.VIEWS);
			Staging.AreasScripts = Path.Combine(Staging.Areas, moduleId, FolderNameConstants.SCRIPTS);

			// Deploy/Final
			Final = new StagingFolderBlock();
			Final.Root = Path.Combine(Deploy, FolderNameConstants.FINAL);
			Final.Bin = Path.Combine(Final.Root, FolderNameConstants.BIN);
			Final.Areas = Path.Combine(Final.Root, FolderNameConstants.AREAS);
			Final.AreasViews = Path.Combine(Final.Areas, moduleId, FolderNameConstants.VIEWS);
			Final.AreasScripts = Path.Combine(Final.Areas, moduleId, FolderNameConstants.SCRIPTS);

			// Live
			Live = new LiveOnlineFolderBlock();
			Live.Root = Path.Combine(sage300Installation, FolderNameConstants.ONLINE);
			Live.Web = Path.Combine(Live.Root, FolderNameConstants.WEB);
			Live.Worker = Path.Combine(Live.Root, FolderNameConstants.WORKER);

			// Create the folders
			Create();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Generate a formatted string of the contents
		/// of this object.
		/// </summary>
		public IEnumerable<string> GenerateLogOutput()
		{
			var lines = new List<string>();
			lines.Add(typeof(FolderManager).ToString());
			lines.Add(string.Format(LogOutputTemplate, "RootSource", RootSource));
			lines.Add(string.Format(LogOutputTemplate, "Deploy", Deploy));
			lines.Add("Originals");
			lines.AddRange(Originals.GenerateLogOutput());
			lines.Add("Compiled");
			lines.AddRange(Compiled.GenerateLogOutput());
			lines.Add("Staging");
			lines.AddRange(Staging.GenerateLogOutput());
			lines.Add("Final");
			lines.AddRange(Final.GenerateLogOutput());
			lines.Add("Live");
			lines.AddRange(Live.GenerateLogOutput());
			return lines;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Create the necessary output folders
		/// </summary>
		private void Create()
		{
			_Logger.Log(string.Format(Messages.Msg_PreparingDeployFoldersAndFilesForStaging, FolderNameConstants.DEPLOY));

			// __DEPLOY__ should already exist by this point
			Compiled.CreateRootOnly();
			Staging.CreateFolders();
			Final.CreateFolders();
		}

		/// <summary>
		/// If the Deploy path already exists, delete it and then recreate it
		/// If the Deploy path doesn't exist, create it.
		/// </summary>
		private void RecreateDeploymentPath()
		{
			var path = Deploy;
			if (Directory.Exists(Deploy))
			{
				_Logger.Log(string.Format(Messages.Msg_PathExists, path));
				try
				{
					Directory.Delete(path, true);
					_Logger.Log(string.Format(Messages.Msg_PathDeleted, path));
				}
				catch (IOException)
				{
					throw new MergeISVProjectException(_Logger, Messages.Error_DeploymentFolderLockedOrInUse);
				}
			}
			Directory.CreateDirectory(path);
			_Logger.Log(string.Format(Messages.Msg_PathCreated, path));
		}

		#endregion
	}
}
