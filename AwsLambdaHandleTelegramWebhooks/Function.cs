using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using AwsLamdaRssCore;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using AwsLamdaCore.Managers;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsLambdaHandleTelegramWebhooks
{
    public class Function
    {
        public  APIGatewayProxyResponse FunctionHandler(JObject input,ILambdaContext context)
        {
            Console.WriteLine("Function started");

            try
            {
                TelegramSenderManager sender = new TelegramSenderManager();
                sender.Init();

                try
                {

                    Update webhookData = JsonConvert.DeserializeObject<Update>(input["body"].ToString());
                    sender.HandleWebhookUpdate(webhookData);
                }
                catch (Exception ex)
                {
                    sender.SendMessageToDebug(ex.ToString());
                    sender.SendMessageToDebug(input["body"].ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(new { msg = "Welcome to Belarus! :)" }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            return response;

        }
    }

    
}
