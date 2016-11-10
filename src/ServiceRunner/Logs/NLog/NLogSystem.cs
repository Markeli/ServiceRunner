using NLogInternal = NLog;

namespace ServiceRunner.Logs.NLog
{
    public class NLogSystem : ILogSystem
    {

        public static NLogSystem CreateByConfig(string customConfigPath)
        {
            NLogInternal.LogManager.Configuration = new NLogInternal.Config.XmlLoggingConfiguration(customConfigPath);
            return new NLogSystem();
        }


        public ILogger CreateLogger(string logName)
        {
            return new NLogLogger(logName);
        }
    }
}