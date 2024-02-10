using Bot_start.Interface;
using System.Globalization;

namespace Bot_start.Controlers
{
    public class FileLogger : IPrivateLogger
    {
        private static readonly string logDir = "./Log/";
        private static readonly string basicLogFile = "basicLog.log";
        private static readonly string messageLogFile = "commands.log";
        private BasicLogData _LogData;
        public void LOG(string message)
        {
            _LogData = createBasicData(basicLogFile);
            string messageText = $"{_LogData.time}| {message}\n";
            File.AppendAllText(_LogData.path, messageText);
        }

        public void LOG(string functionName, string message)
        {
            _LogData = createBasicData(basicLogFile);
            string messageText = $"{_LogData.time}| {functionName}: {message}\n";
            File.AppendAllText(_LogData.path, messageText);
        }

        public void LOG_Message(string message)
        {
            _LogData = createBasicData(messageLogFile);
            string messageText = $"{_LogData.time}| {message}\n";
            File.AppendAllText(_LogData.path, messageText);
        }

        private BasicLogData createBasicData(string file)
        {
            return new BasicLogData(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture), $"{logDir}{file}");
        }

        private record BasicLogData(string time, string path);
    }
}
