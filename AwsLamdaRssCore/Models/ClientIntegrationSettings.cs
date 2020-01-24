using System;
using System.Collections.Generic;
using System.Text;

namespace AwsLamdaRssCore.Models
{
    public class ClientIntegrationSettings
    {
        public string ToChatId { get; set; }
        public string BotToken { get; set; }
        public string Name { get; set; }
        public ClientIntegrationType IntegrationType { get; set; }

        public ClientIntegrationSettings(string name,string toChatId, string botToken, ClientIntegrationType integrationType = ClientIntegrationType.IntegrationClient)
        {
            Name = name;
            ToChatId = toChatId;
            BotToken = botToken;
            IntegrationType = integrationType;
        }

       
    }
}
