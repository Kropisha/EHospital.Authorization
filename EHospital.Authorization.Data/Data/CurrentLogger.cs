namespace EHospital.Authorization.Data
{
    using System;

    /// <summary>
    /// Set current logger for application
    /// </summary>
    public class CurrentLogger : ILogging
    {
        /// <summary>
        /// type of logger
        /// </summary>
        public static readonly log4net.ILog log = log4net.LogManager
                                                         .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Information log
        /// </summary>
        /// <param name="message">some text for log file</param>
        public void LogInfo(string message)
        {
            log.Info(message);
        }

        /// <summary>
        /// Warning log
        /// </summary>
        /// <param name="message">some text for log file</param>
        public void LogWarn(string message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// Warning log
        /// </summary>
        /// <param name="message">some text for log file</param>
        /// <param name="ex">type of exception</param>
        public void LogWarn(string message, Exception ex)
        {
            log.Warn(message, ex);
        }

        /// <summary>
        /// Error log
        /// </summary>
        /// <param name="message">some text for log file</param>
        public void LogError(string message)
        {
            log.Error(message);
        }

        /// <summary>
        /// Error log
        /// </summary>
        /// <param name="message">some text for log file</param>
        /// <param name="ex">type of exception</param>
        public void LogError(string message, Exception ex)
        {
            log.Error(message, ex);
        }
    }
}
