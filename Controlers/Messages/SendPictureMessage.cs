﻿using Bot_start.Interface;
using Bot_start.Models;
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

        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            if (tryToSelectPicture(out string path))
            {
                if (update.Message is Message message)
                {
                    using (Stream stream = System.IO.File.OpenRead(path))
                    {
                        await botClient.SendPhotoAsync(
                           chatId: message.Chat.Id,
                           photo: new InputFileStream(stream)
                       );
                    }
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

        private void saveSentDataForUser(long chatId, string path)
        {
            try
            {
                SentItem sentItem = new SentItem() { ChatId = chatId, ItemName = path };
                DbController dbController = new DbController();
                AppDbContext DB = dbController.GetDb();
                DB.SentItems.Add(sentItem);
                DB.SaveChanges();
            }catch(Exception ex)
            {
                _logger.LOG($"{nameof(saveSentDataForUser)}: {ex.Message}");
            }
        }
    }
}
