namespace ServiceRunner.Logs
{
    internal class LogManager
    {
        private readonly object _locker = new object();
        
        /// <summary>
        /// Основной лог
        /// </summary>
        public Logger MainLog
        {
            get
            {
                if (_mainLogger != null) return _mainLogger;
                lock (_locker)
                {
                    if (_mainLogger == null)
                        _mainLogger = new Logger( "main");
                }
                return _mainLogger;
            }
        }
        private Logger _mainLogger;


        /// <summary>
        /// Лог исключений
        /// </summary>
        public Logger ExceptionLog
        {
            get
            {
                if (_exceptionLogger != null) return _exceptionLogger;
                lock (_locker)
                {
                    if (_exceptionLogger == null)
                        _exceptionLogger = new Logger("exception");
                }
                return _exceptionLogger;
            }
        }
        private Logger _exceptionLogger;

        /// <summary>
        /// Лог запускаемого сервиса
        /// </summary>
        public Logger ServiceLog
        {
            get
            {
                if (_serviceLogger != null) return _serviceLogger;
                lock (_locker)
                {
                    if (_serviceLogger == null)
                        _serviceLogger = new Logger("service");
                }
                return _serviceLogger;
            }
        }
        private Logger _serviceLogger;


    }
}
