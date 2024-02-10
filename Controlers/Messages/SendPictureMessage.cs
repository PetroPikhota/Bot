using Bot_start.Interface;
using Bot_start.Models;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Controlers.Messages
{
    public class SendPictureMessage : IMessage
    {
        IPrivateLogger _logger = MyLogger.GetLogger();
        private string _message = "/givememem";
        public string getMessage()
        {
            return _message;
        }
        AppDbContext dbController = new DbController().GetDb();
        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            if (tryToGetPictureFromDb(out string path))
            {
                if (update.Message is Message message)
                {
                    /*
                    using (Stream stream = System.IO.File.OpenRead(path))
                    {
                        await botClient.SendPhotoAsync(
                           chatId: message.Chat.Id,
                           photo: new InputFileStream(stream)
                       );
                    }*/
                    await botClient.SendPhotoAsync(message.Chat.Id, InputFileUrl.FromUri(path));
                    saveSentDataForUser(message.Chat.Id, path);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Ther is no files to send");
            }
        }

        bool tryToSelectPicture(out string path)
        {
            path = null;
            string[] allFiles = Directory.GetFiles("images");
            if (allFiles.Length > 0)
            {
                Random rnd = new Random();
                path = allFiles[rnd.Next(allFiles.Length - 1)];
                return true;
            }
            return false;
        }

        bool tryToGetPictureFromDb(out string path)
        {
            path = null;
            try
            {              
                if (dbController.Items.Count() > 0)
                {
                    Random rnd = new Random();
                    Item item = dbController.Items.FirstOrDefault(x => x.Id == rnd.Next(dbController.Items.Count() - 1));
                    if (item != null)
                    {
                        path = item.Path;
                        return true;
                    }
                }
            }catch(Exception ex)
            {
                _logger.LOG($"{nameof(tryToGetPictureFromDb)}: {ex.Message}");

            }
            return false;
        }

        private void saveSentDataForUser(long chatId, string path)
        {
            try
            {
                SentItem sentItem = new SentItem() { ChatId = chatId, ItemName = path };

                dbController.SentItems.Add(sentItem);
                dbController.SaveChanges();
            }catch(Exception ex)
            {
                _logger.LOG($"{nameof(saveSentDataForUser)}: {ex.Message}");
            }
        }
    }
}
