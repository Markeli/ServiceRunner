using System;
using NLog;

namespace ServiceRunner.Logs
{
    internal class Logger 
    {
        private readonly NLog.Logger _logger;
        private readonly object _locker = new object();

        /// <summary>
        /// Имя лога
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Логгер
        /// </summary>
        /// <param name="loggerName">Имя лога.</param>
        internal Logger(string loggerName = null)
        {
            Name = loggerName;
        }

        /// <summary>
        /// Записать в лог
        /// </summary>
        /// <param name="logEntry">Запись</param>
        public void Log(LogEntry logEntry)
        {
            lock (_locker)
            {
                _logger.Log(Convert(logEntry));
            }
        }

        private LogEventInfo Convert(LogEntry logEntry)
        {
            var logEntry = new LogEventInfo { LoggerName = Name };
            switch (logEntry.Level)
            {
                case (ErrorLevel.None): logEntry.Level = LogLevel.Off; break;
                case (ErrorLevel.Trace): logEntry.Level = LogLevel.Trace; break;
                case (ErrorLevel.Debug): logEntry.Level = LogLevel.Debug; break;
                case (ErrorLevel.Info): logEntry.Level = LogLevel.Info; break;
                case (ErrorLevel.Warning): logEntry.Level = LogLevel.Warn; break;
                case (ErrorLevel.Error): logEntry.Level = LogLevel.Error; break;
                case (ErrorLevel.Fatal): logEntry.Level = LogLevel.Fatal; break;
            }
            logEntry.Message = logEntry.Message;
            logEntry.TimeStamp = logEntry.TimeStamp;
            if (logEntry.Exception != null) logEntry.Exception = logEntry.Exception;

            if ((logEntry.Data != null) && (logEntry.Data.Count > 0))
            {
                foreach (var prop in logEntry.Data)
                {
                    logEntry.Properties.Add(prop.Key, prop.Value);
                }
            }
            return logEntry;
        }

        /// <summary>
        /// Записать в лог
        /// </summary>
        /// <param name="level">Уровень</param>
        /// <param name="message">Текст</param>
        public void Log(ErrorLevel level, string message)
        {
            Log(new LogEntry { Level = level, Message = message });
        }

        /// <summary>
        /// Записать в лог с уровнем Trace
        /// </summary>
        /// <param name="message">Текст</param>
        public void Trace(string message)
        {
            Log(ErrorLevel.Trace, message);
        }

        /// <summary>
        /// Записать в лог с уровнем Debug
        /// </summary>
        /// <param name="message">Текст</param>
        public void Debug(string message)
        {
            Log(ErrorLevel.Debug, message);
        }

        /// <summary>
        /// Записать в лог с уровнем Info
        /// </summary>
        /// <param name="message">Текст</param>
        public void Info(string message)
        {
            Log(ErrorLevel.Info, message);
        }

        /// <summary>
        /// Записать в лог с уровнем Warning
        /// </summary>
        /// <param name="message">Текст</param>
        public void Warning(string message)
        {
            Log(ErrorLevel.Warning, message);
        }

        /// <summary>
        /// Записать в лог с уровнем Error
        /// </summary>
        /// <param name="message">Текст</param>
        public void Error(string message)
        {
            Log(ErrorLevel.Error, message);
        }

        /// <summary>
        /// Записать в лог исключения (с уровнем Error)
        /// </summary>
        /// <param name="exception">Исключение.</param>
        public void Error(Exception exception)
        {
            ErrorException(exception.Message, exception);
        }

        /// <summary>
        /// Записать в лог с уровнем Fatal
        /// </summary>
        /// <param name="message">Текст</param>
        public void Fatal(string message)
        {
            Log(ErrorLevel.Fatal, message);
        }

        /// <summary>
        /// Записать в лог с приложением исключения
        /// </summary>
        /// <param name="level">Уровень записи</param>
        /// <param name="message">Текст</param>
        /// <param name="exception">Исключение</param>
        public void Exception(ErrorLevel level, string message, Exception exception)
        {
            Log(new LogEntry { Level = level, Message = message, Exception = exception });
        }

        /// <summary>
        /// Записать в лог с приложением исключения
        /// </summary>        
        /// <param name="message">Текст</param>
        /// <param name="exception">Исключение</param>
        public void ErrorException(string message, Exception exception)
        {
            Log(new LogEntry { Level = ErrorLevel.Error, Message = message, Exception = exception });
        }

        /// <summary>
        /// Записать в лог с приложением исключения
        /// </summary>                
        /// <param name="exception">Исключение</param>
        public void ErrorException(Exception exception)
        {
            Log(new LogEntry { Level = ErrorLevel.Error, Message = exception.Message, Exception = exception });
        }

        /// <summary>
        /// Записать в лог с приложением исключения
        /// </summary>                
        /// <param name="exception">Исключение</param>
        public void WarningException(Exception exception)
        {
            Log(new LogEntry { Level = ErrorLevel.Warning, Message = exception.Message, Exception = exception });
        }

    }
}