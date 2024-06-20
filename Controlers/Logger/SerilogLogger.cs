using Bot_start.Interface;
using Microsoft.Extensions.Primitives;
using Serilog;
using System.Globalization;
using System.Text;

namespace Bot_start.Controlers
{
    public class SerilogLogger : BasicLogClass, IPrivateLogger
    {
        public SerilogLogger()
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        }
        public void LOG(string message)
        {
            Log.Information(createMessage(message));
        }

        public void LOG(string functionName, string message)
        {
            Log.Information(createMessage(message, functionName));
        }

        public void LOG_Message(string message)
        {
            Log.Information(createMessage(message));
        }
        
    }
}
