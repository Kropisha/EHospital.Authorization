using System;

namespace EHospital.Authorization.Data
{
    /// <summary>
    /// Set current logger for application
    /// </summary>
    public class CurrentLogger : ILogging
    {
        /// <summary>
        /// type of logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager
                                                         .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Information log
        /// </summary>
        /// <param name="message">some text for log file</param>
        public void LogInfo(string message)
        {
            Log.Info(message);
        }

        /// <summary>
        /// Warning log
        /// </summary>
        /// <param name="message">some text for log file</param>
        public void LogWarn(string message)
        {
            Log.Warn(message);
        }

        /// <summary>
        /// Warning log
        /// </summary>
        /// <param name="message">some text for log file</param>
        /// <param name="ex">type of exception</param>
        public void LogWarn(string message, Exception ex)
        {
            Log.Warn(message, ex);
        }

        /// <summary>
        /// Error log
        /// </summary>
        /// <param name="message">some text for log file</param>
        public void LogError(string message)
        {
            Log.Error(message);
        }

        /// <summary>
        /// Error log
        /// </summary>
        /// <param name="message">some text for log file</param>
        /// <param name="ex">type of exception</param>
        public void LogError(string message, Exception ex)
        {
            Log.Error(message, ex);
        }
    }
}
