using Bot_start.Interface;

namespace Bot_start.Controlers
{
    public class FileLogger : IPrivateLogger
    {
        private static readonly string logDir = "Log";
        private static readonly string basicLogFile = "basicLog.log";
        public void LOG(string message)
        {
            string errorTime = DateTime.Now.ToString();
            string path = $"{logDir}/{basicLogFile}";
            string messageText = $"{errorTime}| {message}\n";
            File.AppendAllText(path, messageText);
        }
    }
}
