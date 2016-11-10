
using System;
using NLogInternal = NLog;

namespace ServiceRunner.Logs.NLog
{
    /// <summary>
    /// Логгер NLog
    /// </summary>
    public class NLogLogger : ILogger
    {
        private readonly NLogInternal.Logger _logger;

        /// <summary>
        /// Имя лога
        /// </summary>
        public string Name
        {
            get
            {
                return (_logger != null) ? _logger.Name : null;
            }
        }


        /// <summary>
        /// Записать в лог
        /// </summary>
        /// <param name="logEntry">Запись</param>
        /// <returns>Получилось ли записать</returns>
        public bool Log(LogEntry logEntry)
        {
            if (_logger == null) return false;

            var nlogEntry = new NLogInternal.LogEventInfo { LoggerName = Name };
            switch (logEntry.Level)
            {
                case (ErrorLevel.None): nlogEntry.Level = NLogInternal.LogLevel.Off; break;
                case (ErrorLevel.Trace): nlogEntry.Level = NLogInternal.LogLevel.Trace; break;
                case (ErrorLevel.Debug): nlogEntry.Level = NLogInternal.LogLevel.Debug; break;
                case (ErrorLevel.Info): nlogEntry.Level = NLogInternal.LogLevel.Info; break;
                case (ErrorLevel.Warning): nlogEntry.Level = NLogInternal.LogLevel.Warn; break;
                case (ErrorLevel.Error): nlogEntry.Level = NLogInternal.LogLevel.Error; break;
                case (ErrorLevel.Fatal): nlogEntry.Level = NLogInternal.LogLevel.Fatal; break;
            }
            nlogEntry.Message = logEntry.Message;
            nlogEntry.TimeStamp = logEntry.TimeStamp;
            if (logEntry.Exception != null) nlogEntry.Exception = logEntry.Exception;

            if ((logEntry.Data != null) && (logEntry.Data.Count > 0))
            {
                foreach (var prop in logEntry.Data)
                {
                    nlogEntry.Properties.Add(prop.Key, prop.Value);
                }
            }

            try
            {
                if (!_logger.IsEnabled(nlogEntry.Level)) return false;

                _logger.Log(nlogEntry);

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                // возникает при Level = Off
                return false;
            }
        }

        /// <summary>
        /// Логгер NLog
        /// </summary>
        /// <param name="name">Имя лога</param>
        internal NLogLogger(string name = null)
        {
            _logger = !String.IsNullOrEmpty(name) ? NLogInternal.LogManager.GetLogger(name) : NLogInternal.LogManager.GetCurrentClassLogger();
        }
    }
}