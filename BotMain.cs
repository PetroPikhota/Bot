using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using Bot_start.Interface;
using Bot_start.Controlers;
using Bot_start.Controlers.Messages;
using Bot_start.Models;

namespace Bot_start
{

    public class BotMain
    {
        private static LogOptions _logOptions;
        readonly IPrivateLogger _logger; 
        private static TelegramBotClient botClient;
        private List<IMessage> messages;
        public BotMain()
        {
            
            _logOptions = LoginParameters.GetLogOptions();
            _logger = new MyLogger(_logOptions);
            botClient = new TelegramBotClient(LoginParameters.GetTelegram().botToken);
            messages = new List<IMessage>
            {
                new HelloMessage(_logger),
                new UpdateImagesMessage(_logger),
                new SendPictureMessage(_logger),
                new CheckState(_logger)
            };
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
                _logger.LOG_Message(message.Text);
                IMessage msg = messages.FirstOrDefault(x => x.getMessage() == message.Text);
                if (msg != null)
                {
                    await msg.PerformAction(botClient, update);
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Unknown message");
                    _logger.LOG($"Unknown message: {message.Text}");
                }
            }
        }

        private async Task OnError(ITelegramBotClient botClient, Exception ex, CancellationToken token)
        {
            if (ex is ApiRequestEventArgs)
            {
                await botClient.SendTextMessageAsync("", "there is some Error bro");
                _logger.LOG(ex.Message);
            }
        }
    }
}
