namespace ServiceRunner
{
    internal class ServiceInfo
    {
        /// <summary>
        /// Название сервиса
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Название сервиса, которое будет отображаться в списке процессов
        /// </summary>
        public string ServiceDisplayName { get; set; }

        /// <summary>
        /// Описание сервиса
        /// </summary>
        public string ServiceDescription { get; set; }

        /// <summary>
        /// Путь к запускаемому файлу
        /// </summary>
        public string ServicePath { get; set; }

        /// <summary>
        /// Аргументы, с которым нужно запускать сервис
        /// </summary>
        public string ServiceArguments { get; set; }

        /// <summary>
        /// Необходимо ли перезапускать сервис после ошибки
        /// </summary>
        public bool RestartAfterCrash { get; set; }

        /// <summary>
        /// Таймаут в минутах перезапуска сервиса после падения
        /// </summary>
        public int RestartTimeoutMin { get; set; }
    }
}