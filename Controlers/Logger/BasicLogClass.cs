using System.Globalization;
using System.Text;

namespace Bot_start.Controlers
{
    public abstract class BasicLogClass
    {
        public string createMessage(string message, string funcName = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}| ");
            stringBuilder.Append(String.IsNullOrWhiteSpace(funcName) ? "" : $"{funcName}: ");
            stringBuilder.Append(message);
            return $"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}| {funcName ?? ""}";
        }
    }
}
