using Bot_start.Interface;
using Bot_start.Models;
using System;
using System.Globalization;
using System.Linq;

namespace Bot_start.Controlers
{
    [Flags]
    enum Loglevels
    {
        logToFile=1,
        serilog=2
    }
    
    public class MyLogger: IPrivateLogger
    {
        List<IPrivateLogger> _privateLoggers = new List<IPrivateLogger>();

        private static Dictionary<Loglevels, IPrivateLogger> keyValuePairs = new Dictionary<Loglevels, IPrivateLogger>() {
            {Loglevels.logToFile, new FileLogger()},
            {Loglevels.serilog, new SerilogLogger() }
        };
        public MyLogger()
        {
            //fileLogger is default lgger
            _privateLoggers.Add(new FileLogger());
        }
        public MyLogger(LogOptions logOptions)
        {
            Loglevels level = Loglevels.logToFile;
            if (Enum.TryParse(logOptions.Level, out Loglevels result))
            {
                level = result;
            }
            _privateLoggers.AddRange(from Loglevels flag in Enum.GetValues(typeof(Loglevels))
                                     where level.HasFlag(flag)
                                     select keyValuePairs[flag]);
        }

        public void LOG(string message)
        {
            foreach (var logger in _privateLoggers)
                logger.LOG(message);
        }

        public void LOG(string functionName, string message)
        {
            foreach (var logger in _privateLoggers)
                logger.LOG(functionName, message);
        }

        public void LOG_Message(string message)
        {
            foreach (var logger in _privateLoggers)
                logger.LOG(message);
        }
    }
}
