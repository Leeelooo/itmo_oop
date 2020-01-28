namespace engine.utils.logger
{
    public class ConsoleLogger : ILogger
    {
        private static ConsoleLogger _instance;
        public static ConsoleLogger Instance => _instance ??= new ConsoleLogger();

        private ConsoleLogger() { }
        public void LogError(string message) => System.Console.Error
            .WriteLine($"Error!\n\tCurrent time: {GetCurrentTime()}.\n\tError message: {message}");
        public void LogMessage(string message) => System.Console.WriteLine(message);

        private string GetCurrentTime() => System.DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
    }
}
