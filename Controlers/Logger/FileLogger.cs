using Bot_start.Interface;

namespace Bot_start.Controlers
{
    public class FileLogger : BasicLogClass, IPrivateLogger
    {
        private static readonly string logDir = "./Log/";
        private static readonly string basicLogFile = "basicLog.log";

        public FileLogger()
        {
            if (!Directory.Exists("./Log"))
            {
                _ = Directory.CreateDirectory("./Log");
            }
        }
        public void LOG(string message)
        {
            string messageText = $"{createMessage(message)}\n";
            File.AppendAllText(fullPath(basicLogFile), messageText);
        }

        public void LOG(string functionName, string message)
        {
            string messageText = $"{createMessage(message, functionName)}\n";
            File.AppendAllText(fullPath(basicLogFile), messageText);
        }

        public void LOG_Message(string message)
        {
            string messageText = $"{createMessage(message)}\n";
            File.AppendAllText(fullPath(basicLogFile), messageText);
        }

        private string fullPath(string file)
        {
            return $"{logDir}{file}";
        }
    }
}
