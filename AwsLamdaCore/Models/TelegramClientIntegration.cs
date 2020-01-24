using System;
using System.Collections.Generic;
using System.Text;

namespace AwsLamdaRssCore.Models
{
    public class TelegramClientIntegration
    {
        public TelegramClient Client { get; set; }
        public ClientIntegrationSettings Settings { get; set; }
    }
}
