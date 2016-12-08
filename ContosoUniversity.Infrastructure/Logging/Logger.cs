using System;
using System.Text;

namespace ContosoUniversity.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        public void Information(string message)
        {
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public void Information(string format, params object[] variables)
        {
            System.Diagnostics.Trace.TraceInformation(format, variables);
        }

        public void Information(Exception exception, string format, params object[] variables)
        {
            System.Diagnostics.Trace.TraceInformation(FormatExceptionMessage(exception, format, variables));
        }

        public void Warning(string message)
        {
            System.Diagnostics.Trace.TraceWarning(message);
        }

        public void Warning(string format, params object[] variables)
        {
            System.Diagnostics.Trace.TraceWarning(format, variables);
        }

        public void Warning(Exception exception, string format, params object[] variables)
        {
            System.Diagnostics.Trace.TraceWarning(FormatExceptionMessage(exception, format, variables));
        }

        public void Error(string message)
        {
            System.Diagnostics.Trace.TraceError(message);
        }

        public void Error(string format, params object[] variables)
        {
            System.Diagnostics.Trace.TraceError(format, variables);
        }

        public void Error(Exception exception, string format, params object[] variables)
        {
            System.Diagnostics.Trace.TraceError(FormatExceptionMessage(exception, format, variables));
        }

        public void Trace(string componentName, string method, TimeSpan timespan)
        {
            Trace(componentName, method, timespan, "");
        }

        public void Trace(string componentName, string method, TimeSpan timespan, string format, params object[] variables)
        {
            Trace(componentName, method, timespan, string.Format(format, variables));
        }
        public void Trace(string componentName, string method, TimeSpan timespan, string properties)
        {
            string message = String.Concat("Component:", componentName, ";Method:", method, ";Timespan:", timespan.ToString(), ";Properties:", properties);
            System.Diagnostics.Trace.TraceInformation(message);
        }

        private static string FormatExceptionMessage(Exception exception, string fmt, object[] vars)
        {
            // Simple exception formatting: for a more comprehensive version see 
            // http://code.msdn.microsoft.com/windowsazure/Fix-It-app-for-Building-cdd80df4
            var sb = new StringBuilder();
            sb.Append(string.Format(fmt, vars));
            sb.Append(" Exception: ");
            sb.Append(exception);
            return sb.ToString();
        }
    }
}