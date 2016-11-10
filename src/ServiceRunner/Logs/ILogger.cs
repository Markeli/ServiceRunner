namespace ServiceRunner.Logs
{
    /// <summary>
    /// Логгер
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Имя логгера
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Записать в лог
        /// </summary>
        /// <param name="logEntry">Запись</param>
        /// <returns>Получилось ли записать</returns>
        bool Log(LogEntry logEntry);
    }
}