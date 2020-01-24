using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AwsLamdaCore;
using AwsLamdaCore.Managers;
using AwsLamdaRssCore;
using Telegram.Bot.Types;
using Update = Amazon.DynamoDBv2.Model.Update;

namespace ConsoleApp4
{
    class Program
    {

        public static void Main(string[] args)
        {
            

            Task t = MainAsync(args);
            t.Wait();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static async Task MainAsync(string[] args)
        {
            var webhook = JsonConvert.DeserializeObject<Telegram.Bot.Types.Update>(System.IO.File.ReadAllText("./webhook.test.json"));
            var s = new TelegramSenderManager();
            s.Init();
            s.HandleWebhookUpdate(webhook);
        }
    }


    
}
