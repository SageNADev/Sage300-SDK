using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    public static class Delegates
    {
        /// <summary> Delegate to update UI with name of the step being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void ProcessingEventHandler(string text);

        /// <summary> Delegate to update log with status of the step being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void LogEventHandler(string text);
    }
}
