﻿using Bot_start.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot_start.Controlers.Messages
{
    public class HelloMessage : IMessage
    {
        private string _message = "/hello";
        private readonly IPrivateLogger privateLogger;
        public string getMessage()
        {
            return _message;
        }
        public HelloMessage(IPrivateLogger _privateLogger)
        {
            privateLogger = _privateLogger;
        }

        public async Task PerformAction(ITelegramBotClient botClient, Update update)
        {
            if (update.Message is Message message)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "It was Hello command");
            }         
        }
    }
}
