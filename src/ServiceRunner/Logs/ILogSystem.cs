namespace ServiceRunner.Logs
{
    public interface ILogSystem
    {
        ILogger CreateLogger(string logName);
    }
}