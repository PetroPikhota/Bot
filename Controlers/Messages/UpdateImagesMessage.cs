using Bot_start.Controlers.RedditControllers;
using Bot_start.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Controlers.Messages
{
    public class UpdateImagesMessage : IMessage
    {
        private string _message = "/update";
        private readonly IPrivateLogger privateLogger;
        public string getMessage()
        {
            return _message;
        }
        public UpdateImagesMessage(IPrivateLogger _privateLogger)
        {
            privateLogger = _privateLogger;     
        }
        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            UpdateImagesFromReddit updateImages = new UpdateImagesFromReddit(privateLogger);
            int i = -1;
            if (updateImages.UpdateImages(ref i))
            {
                await SendMessage(botClient, update, $"Succesfully updated. Added {i} records");
            }
            else
            {
                await SendMessage(botClient, update, "Some problem occured");
            }
        }

        private async Task SendMessage(ITelegramBotClient botClient, Update update, string textMessage)
        {
            if (update.Message is Message message)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, textMessage);
            }
        }
    }
}
