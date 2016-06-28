using System;
using System.Diagnostics;
using System.IO;
using NLog;

namespace Cortex.Util
{
    public class Explorer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Attempts to open an explorer window at the given path
        /// Exceptions are caught and written to Error log
        /// </summary>
        /// <param name="path">Path to open Explorer window at</param>
        /// <returns>Wether or not the call was successful. Returns false if exception is thrown or path does not exist.</returns>
        public static bool TryOpenExplorer(string path)
        {
            var success = false;
            try
            {
                if (path != null && Directory.Exists(path))
                {
                    Process.Start(path);
                    success = true;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
            return success;
        }
    }
}
