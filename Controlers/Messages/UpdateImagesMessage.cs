using Bot_start.Controlers.RedditControllers;
using Bot_start.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Controlers.Messages
{
    public class UpdateImagesMessage : IMessage
    {
        private string _message = "/update";
        public string getMessage()
        {
            return _message;
        }

        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            UpdateImagesFromReddit updateImages = new UpdateImagesFromReddit();
            if (updateImages.UpdateImages())
            {
                await SendMessage(botClient, update, "Succesfully updated");
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
