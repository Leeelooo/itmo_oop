namespace engine.utils.logger
{
    public interface ILogger
    {
        void LogMessage(string message);
        void LogError(string message);
    }
}
