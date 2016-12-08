using System;

namespace ContosoUniversity.Infrastructure.Logging
{
    public interface ILogger
    {
        void Information(string message);
        void Information(string format, params object[] variables);
        void Information(Exception exception, string format, params object[] variables);
        void Warning(string message);
        void Warning(string format, params object[] variables);
        void Warning(Exception exception, string format, params object[] variables);
        void Error(string message);
        void Error(string format, params object[] variables);
        void Error(Exception exception, string format, params object[] variables);
        void Trace(string componentName, string method, TimeSpan timespan);
        void Trace(string componentName, string method, TimeSpan timespan, string properties);
        void Trace(string componentName, string method, TimeSpan timespan, string format, params object[] variables);
    }
}