using Bot_start.Interface;
using Bot_start.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Controlers.Messages
{
    public class SendPictureMessage : IMessage
    {
        private readonly IPrivateLogger _logger = MyLogger.GetLogger();
        private readonly string _message = "/givememem";
        public string getMessage()
        {
            return _message;
        }

        private readonly AppDbContext dbController = new DbController().GetDb();
        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            if (tryToGetPictureFromDb(update.Message.Chat.Id.GetHashCode(), out string path) && path != null)
            {
                if (update.Message is Message message)
                {
                    _ = await botClient.SendPhotoAsync(message.Chat.Id, InputFileUrl.FromUri(path));
                    saveSentDataForUser(message.Chat.Id, path);
                }
            }
            else
            {

                if (update.Message is Message message)
                {
                    if (tryToSelectPicture(out path))
                    {
                        using Stream stream = System.IO.File.OpenRead(path);
                        _ = await botClient.SendPhotoAsync(
                           chatId: message.Chat.Id,
                           photo: new InputFileStream(stream)
                       );
                    }
                    else
                    {
                        _ = await botClient.SendTextMessageAsync(update.Message.Chat.Id, "There is no files to send");
                    }
                }

            }
        }

        private bool tryToSelectPicture(out string? path)
        {
            path = null;
            string[] allFiles = Directory.GetFiles("images");
            if (allFiles.Length > 0)
            {
                Random rnd = new();
                path = allFiles[rnd.Next(allFiles.Length - 1)];
                return true;
            }
            return false;
        }

        private bool tryToGetPictureFromDb(int chatId, out string? path)
        {
            path = null;
            try
            {
                List<string> items = dbController.Items.Select(x => x.Path).ToList();
                _ = items.RemoveAll(dbController.SentItems.Where(x => x.ChatId == chatId).Select(x => x.ItemName).ToList().Contains);
                int itemsCount = items.Count;
                if (itemsCount > 0)
                {
                    Random rnd = new();
                    int elementNumber = rnd.Next(itemsCount - 1);
                    string item = items[elementNumber];
                    if (item != null)
                    {
                        path = item;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LOG($"{nameof(tryToGetPictureFromDb)}: {ex.Message}");

            }
            return false;
        }

        private void saveSentDataForUser(long chatId, string path)
        {
            try
            {
                SentItem sentItem = new() { ChatId = chatId.GetHashCode(), ItemName = path };

                _ = dbController.SentItems.Add(sentItem);
                _ = dbController.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LOG(nameof(saveSentDataForUser), ex.Message);
            }
        }
    }
}
