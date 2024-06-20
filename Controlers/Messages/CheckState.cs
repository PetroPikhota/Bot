using Bot_start.Interface;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Controlers.Messages
{
    public class CheckState : IMessage
    {
        private readonly IPrivateLogger privateLogger;
        private readonly string _message = "/status";
        public string getMessage()
        {
            return _message;
        }
        public CheckState(IPrivateLogger _privateLogger)
        {
            privateLogger = _privateLogger;
        }
        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            if (update.Message is Message message)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, GetResourcesStatus());
            }
        }

        string GetResourcesStatus()
        {
            Process proc = Process.GetCurrentProcess();
            return proc.PrivateMemorySize64.ToString();
        }
    }
}
