// Copyright (c) 2019 Sage Software, Inc.  All rights reserved.

using System;
using System.IO;

namespace $companynamespace$.$applicationid$.Web
{
    /// <summary>
    /// Class to monitor folder for evict user request
    /// </summary>
    public class EvictUserWatcher
    {
        #region private members

        private EvictUserWatcher() { }

        private static readonly Lazy<EvictUserWatcher> _lazyInstance = new Lazy<EvictUserWatcher>(() => new EvictUserWatcher());


        static internal readonly Func<string, bool> DefaultEvictAction =
            (userName) =>
            {
                return true;
            };

        
        #endregion

        static internal Func<string, bool> EvictAction = DefaultEvictAction;

        #region public members
        /// <summary>
        /// Return singleton instnce of the class
        /// </summary>
        public static EvictUserWatcher Instance
        {
            get { return _lazyInstance.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>UserId with unique value</returns>
        public static void AddUserIdToPauseEviction(string userId)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public static void RemoveUserIdFromPauseEviction(string userId)
        {
        }

        /// <summary>
        /// Start the watcher
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// Stop the watcher
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Handler when a new file is created in the folder to be monitor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void WatcherCreatedHandler(object sender, FileSystemEventArgs e)
        {
        }

        /// <summary>
        /// Open file from full path and extra datetime out of it
        /// </summary>
        /// <param name="fullPath">string of file path</param>
        /// <param name="evictDateTime">Out parameter for the extracted datetime</param>
        /// <returns>True if file is valid, false otherwise</returns>
        internal static bool ReadAndDecryptFile(string fullPath, out DateTime evictDateTime)
        {
            evictDateTime = DateTime.MinValue;

            return true;
        }

        /// <summary>
        /// To decrypt file user name
        /// </summary>
        /// <param name="userName">string of user name</param>
        /// <returns>String of decrypted user name</returns>
        internal static string DecryptUserName(string userName)
        {
            return string.Empty;
        }

        /// <summary>
        /// The full folder path to be monitored
        /// </summary>
        /// <param name="sharedDataFolder">String of shared data folder</param>
        /// <returns>string of folder to be monitored</returns>
        internal string GetMonitorFolder(string sharedDataFolder)
        {
            return string.Empty;
        }
        #endregion

    }
}
