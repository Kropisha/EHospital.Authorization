namespace EHospital.Authorization.Data
{
    using System;

    public interface ILogging
    {

        void LogError(string message);

        void LogError(string message, Exception ex);

        void LogInfo(string message);

        void LogWarn(string message);

        void LogWarn(string message, Exception ex);
    }
}