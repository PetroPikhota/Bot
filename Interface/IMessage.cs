using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Interface
{
    public interface IMessage
    {
        string getMessage();
        Task PerformAction(ITelegramBotClient botClient, Update update);
    }
}
