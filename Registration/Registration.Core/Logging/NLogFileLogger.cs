using Newtonsoft.Json;
using Registration.Core.Abstraction;
using System;
using System.Text;
using System.Threading;

namespace Registration.Core.Logging
{
    public class NLogFileLogger : ILogger
    {
        private const string _delimiter = "  ";
        private readonly NLog.ILogger _logger;

        public NLogFileLogger() : this(NLog.LogManager.GetCurrentClassLogger())
        {
        }

        public NLogFileLogger(NLog.ILogger logger)
        {
            _logger = logger;
        }

        public void Information(string message, params object[] parameters)
        {
            _logger.Info(CreateLogMessage(message, parameters));
        }

        public void Warning(string message, params object[] parameters)
        {
            _logger.Warn(CreateLogMessage(message, parameters));
        }

        public void Debug(string message, params object[] parameters)
        {
            _logger.Debug(CreateLogMessage(message, parameters));
        }

        public void Error(string message, params object[] parameters)
        {
            _logger.Error(CreateLogMessage(message, parameters));
        }

        private string CreateLogMessage(string message, params object[] parameters)
        {
            var logMessage = new StringBuilder();

            logMessage.Append($"{Thread.CurrentThread.ManagedThreadId}{_delimiter}");

            if (!string.IsNullOrEmpty(message))
            {
                logMessage.Append($"{message}{_delimiter}");
            }

            if (parameters?.Length > 0)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                    {
                        if (IsClassType(parameter))
                        {
                            var jsonSerializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

                            var data = JsonConvert.SerializeObject(parameter, Formatting.None, jsonSerializerSettings);

                            logMessage.Append($"{data}{_delimiter}");
                        }
                        else
                        {
                            logMessage.Append($"{parameter}{_delimiter}");
                        }
                    }
                }
            }

            return logMessage.ToString();
        }

        private static bool IsClassType(object @object)
        {
            Type type = @object.GetType();

            return !type.IsValueType && type != typeof(string);
        }
    }
}
