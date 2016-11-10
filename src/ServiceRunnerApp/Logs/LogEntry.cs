using System;
using System.Collections.Generic;

namespace ServiceRunner.Logs
{
    /// <summary>
    /// Запись в лог
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Текст записи
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Уровень
        /// </summary>
        public ErrorLevel Level { get; set; }        

        /// <summary>
        /// Набор дополнительных данных
        /// </summary>
        public Dictionary<string, object> Data { get; set; }

        /// <summary>
        /// Дата записи
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Прилагаемое исключение
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Запись в лог
        /// </summary>
        public LogEntry()
        {
            Data = new Dictionary<string, object>();
            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// Запись в лог
        /// </summary>
        /// <param name="logLevel">Уровень</param>
        /// <param name="message">Текст записи</param>
        public LogEntry(ErrorLevel logLevel, string message)
            : this()
        {
            Level = logLevel;
            Message = message;
        }

        /// <summary>
        /// Запись в лог
        /// </summary>
        /// <param name="logLevel">Уровень</param>
        /// <param name="message">Текст записи</param>
        /// <param name="exception">Исключение</param>
        public LogEntry(ErrorLevel logLevel, string message, Exception exception)
            : this()
        {
            Level = logLevel;
            Message = message;
            Exception = exception;
        }

        /// <summary>
        /// Запись в лог
        /// </summary>		
        /// <param name="exception">Исключение</param>
        public LogEntry(Exception exception)
            : this(ErrorLevel.Error, exception?.Message ?? String.Empty, exception)
        {
        }
    }	
}
