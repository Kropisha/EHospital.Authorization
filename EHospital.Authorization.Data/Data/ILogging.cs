namespace EHospital.Authorization.Data
{
    using System;

    /// <summary>
    /// An interface for logging
    /// </summary>
    public interface ILogging
    {
        /// <summary>
        /// Error log
        /// </summary>
        /// <param name="message">some text for log file</param>
        void LogError(string message);

        /// <summary>
        /// Error log
        /// </summary>
        /// <param name="message">some text for log file</param>
        /// <param name="ex">type of exception</param>
        void LogError(string message, Exception ex);

        /// <summary>
        /// Information log
        /// </summary>
        /// <param name="message">some text for log file</param>
        void LogInfo(string message);

        /// <summary>
        /// Warning log
        /// </summary>
        /// <param name="message">some text for log file</param>
        void LogWarn(string message);

        /// <summary>
        /// Warning log
        /// </summary>
        /// <param name="message">some text for log file</param>
        /// <param name="ex">type of exception</param>
        void LogWarn(string message, Exception ex);
    }
}