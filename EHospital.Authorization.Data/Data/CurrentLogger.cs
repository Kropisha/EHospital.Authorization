namespace EHospital.Authorization.Data
{
    using System;

    public class CurrentLogger : ILogging
    {
        public static readonly log4net.ILog log = log4net.LogManager
                                                         .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void LogInfo(string message)
        {
            log.Info(message);
        }

        public void LogWarn(string message)
        {
            log.Warn(message);
        }

        public void LogWarn(string message, Exception ex)
        {
            log.Warn(message, ex);
        }

        public void LogError(string message)
        {
            log.Error(message);
        }

        public void LogError(string message, Exception ex)
        {
            log.Error(message, ex);
        }
    }
}
