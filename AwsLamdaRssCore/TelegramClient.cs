using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AwsLamdaRssCore
{
    public class TelegramClient
    {
        private readonly string _toChatId;
        private readonly string _botToken;

        public TelegramClient(string toChatId, string botToken)
        {
            _toChatId = toChatId;
            _botToken = botToken;
        }

        private ClientTelegramWrapper _bot = null;

        public void SendMessage(string message, InlineKeyboardMarkup keyboardMarkup = null)
        {
            if (message == null) return;

            if (_bot == null)
            {
                _bot = new ClientTelegramWrapper()
                {
                    Token = _botToken,
                    ChatId = _toChatId,
                    ShowTitle = false
                };
            }


            if (_bot.Bot == null && !_bot.BotInited)
            {

                if (!string.IsNullOrEmpty(_bot.Token))
                {
                    _bot.Bot = new TelegramBotClient(_bot.Token);
                }

                _bot.BotInited = true;
            }

            SendMessage(_bot.Bot, _bot.ChatId, message, keyboardMarkup);
        }      

        

        private void SendMessage(TelegramBotClient bot, string chatId, string message, InlineKeyboardMarkup keyboardMarkup = null)
        {
            if (!chatId.StartsWith("@") && long.TryParse(chatId, out var id))
            {
                var result = bot.SendTextMessageAsync(id, message, replyMarkup: keyboardMarkup).Result;
            }
            else
            {
                var result = bot.SendTextMessageAsync(chatId, message, replyMarkup: keyboardMarkup).Result;
            }
        }
    }




    public class TelegramSettings
    {
        public List<TelegramBotSettings> Bots { get; set; }

        public TelegramSettings()
        {
            Bots = new List<TelegramBotSettings>();
        }
    }

    public class TelegramBotSettings
    {
        public string Token { get; set; }

        public bool ShowTitle { get; set; }

        public string ChatId { get; set; }
        public int Offset { get; set; }

        public TelegramBotSettings()
        {

        }
    }

    class ClientTelegramWrapper
    {
        public bool BotInited { get; set; }
        public string Token { get; set; }
        public int Offset { get; set; }
        public string ChatId { get; set; }
        public TelegramBotClient Bot { get; set; }
        public bool ShowTitle { get; set; }

        public ClientTelegramWrapper()
        {

        }


    }
}
