using Bot_start.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Controlers
{
    public class ListOfMessage : IMessage
    {
        private readonly string funcName = nameof(ListOfMessage);
        private const string _message = "/list";
        private readonly IPrivateLogger privateLogger;
        private readonly List<string> messagelist;
        public ListOfMessage(IPrivateLogger _privateLogger, List<string> _messagelist)
        {
            privateLogger = _privateLogger;
            messagelist = _messagelist;
        }
        public string getMessage()
        {
            return _message;
        }

        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            try
            {
                if (update.Message is Message message)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, String.Join('\n', messagelist));
                }
            }
            catch(Exception ex)
            {
                privateLogger.LOG(funcName, ex.Message);
            }
        }
    }
}
