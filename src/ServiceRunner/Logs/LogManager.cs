using System;

namespace ServiceRunner.Logs
{
    internal class LogManager
    {
        private readonly ILogSystem _factory;
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
                        _mainLogger = new Logger(_factory, "main");
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
                        _exceptionLogger = new Logger(_factory, "exception");
                }
                return _exceptionLogger;
            }
        }
        private Logger _exceptionLogger;

        /// <summary>
        /// Лог запускаемого сервиса
        /// </summary>
        public Logger ServiceMainMainLog
        {
            get
            {
                if (_serviceMainLogger != null) return _serviceMainLogger;
                lock (_locker)
                {
                    if (_serviceMainLogger == null)
                        _serviceMainLogger = new Logger(_factory, "serviceMain");
                }
                return _serviceMainLogger;
            }
        }
        private Logger _serviceMainLogger;


        /// <summary>
        /// Лог запускаемого сервиса
        /// </summary>
        public Logger ServiceExceptionLog
        {
            get
            {
                if (_serviceExceptionLog != null) return _serviceExceptionLog;
                lock (_locker)
                {
                    if (_serviceExceptionLog == null)
                        _serviceExceptionLog = new Logger(_factory, "serviceException");
                }
                return _serviceExceptionLog;
            }
        }
        private Logger _serviceExceptionLog;

        public LogManager(ILogSystem factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _factory = factory;
        }
    }
}
