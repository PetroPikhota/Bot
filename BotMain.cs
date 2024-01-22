using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Bot_start
{

    public class BotMain
    {
        private static TelegramBotClient botClient;
        public BotMain()
        {
            botClient = new TelegramBotClient("YOUR_BOT_TOKEN");
        }
        public async Task StartReceiver()
        {
            var token = new CancellationTokenSource();
            var cancel_token = token.Token;
            var reOpt = new ReceiverOptions { AllowedUpdates = { } };
            await botClient.ReceiveAsync(OnMessage, OnError, reOpt, cancel_token);
        }

        private async Task OnMessage(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Message is Message message)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Hello Bro");
            }
        }

        private async Task OnError(ITelegramBotClient botClient, Exception ex, CancellationToken token)
        {
            if (ex is ApiRequestEventArgs)
            {
                await botClient.SendTextMessageAsync("", "there is some Error bro");
            }
        }
    }
}
