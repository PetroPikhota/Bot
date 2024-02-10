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
            if (tryToGetPictureFromDb(update.Message.Chat.Id.GetHashCode()  ,out string path))
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

        bool tryToGetPictureFromDb(int chatId, out string path)
        {
            path = null;
            try
            {
                List<string> items = dbController.Items.Select(x => x.Path).ToList();
                items.RemoveAll(dbController.SentItems.Where(x => x.ChatId == chatId).Select(x => x.ItemName).ToList().Contains);
                int itemsCount = items.Count;
                if (itemsCount > 0)
                {
                    Random rnd = new Random();
                    int elementNumber=rnd.Next(itemsCount-1);
                    string item = items[elementNumber];
                    if (item != null)
                    {
                        path = item;
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
                SentItem sentItem = new SentItem() { ChatId = chatId.GetHashCode(), ItemName = path };

                dbController.SentItems.Add(sentItem);
                dbController.SaveChanges();
            }catch(Exception ex)
            {
                _logger.LOG(nameof(saveSentDataForUser),ex.Message);
            }
        }
    }
}
