using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AwsLamdaRssCore;
using AwsLamdaRssCore.Models;
using BotCore.Managers;
using Humanizer;
using Telegram.Bot.Types;

namespace AwsLamdaCore.Managers
{
    public class TelegramSenderManager
    {
        private TelegramClientIntegration _ownerClient;
        private TelegramClientIntegration _debugClient;
        private BotModuleManager _bot;
        public TelegramSenderManager()
        {

        }

        public void Init()
        {
            var botConfiguration = new BotConfiguration()
            {
                ClientIntegrationSettings = new List<ClientIntegrationSettings>()
                {
                    new ClientIntegrationSettings("Owner", AppConfig.OwnerChatId, AppConfig.MonkeyJobBotToken, ClientIntegrationType.Owner),
                    new ClientIntegrationSettings("Debug", AppConfig.OwnerChatId, AppConfig.DebugBotToken, ClientIntegrationType.Debug),
                }
            };

            var ownerConf = botConfiguration.ClientIntegrationSettings.Single(x => x.IntegrationType == ClientIntegrationType.Owner);
            var debugConf = botConfiguration.ClientIntegrationSettings.Single(x => x.IntegrationType == ClientIntegrationType.Debug);

            _ownerClient = new TelegramClientIntegration()
            {
                Client = new TelegramClient(ownerConf.ToChatId, ownerConf.BotToken),
                Settings = ownerConf
            };

            _debugClient = new TelegramClientIntegration()
            {
                Client = new TelegramClient(debugConf.ToChatId, debugConf.BotToken),
                Settings = debugConf
            };

            

            _bot = new BotModuleManager();

            Console.WriteLine("TelegramSenderManager inited");
        }

        

        private void SendMessageToOwner(string message)
        {
            _ownerClient.Client.SendMessage(message);
        }

        public void SendMessageToDebug(string message)
        {
            _debugClient.Client.SendMessage("FunBot debug : " + message);
        }


        private void SendMessageToChat(long chatId, string message)
        {
            new TelegramClient(chatId.ToString(), AppConfig.MonkeyJobBotToken).SendMessage(message);
        }

        public void HandleWebhookUpdate(Update webhook)
        {
            switch (webhook.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    {
                        var tgMessage = webhook.EditedMessage ?? webhook.Message;
                        var mention = tgMessage?.Entities?.SingleOrDefault(x =>
                            x.Type == Telegram.Bot.Types.Enums.MessageEntityType.Mention);
                        string message = tgMessage?.Text;
                        long? chatId = tgMessage?.Chat?.Id;

                        if (message != null && mention != null)
                        {

                            // to do : hide bot name

                            bool isBotMentioned = mention.Offset == 0 &&
                                                  message.Substring(0, mention.Length) == "@MonkeyJobBot";


                            if (isBotMentioned)
                            {
                                string messageWithoutBotMention = message.Substring(mention.Length).Trim();
                                string answer = _bot.HandleMessage(messageWithoutBotMention);
                                SendMessageToChat(chatId.Value,answer);
                            }
                        }

                        break;
                    }
            }
        }
    }
}
