using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using AwsLamdaRssCore;

namespace AwsLamdaCore
{
    public class StorageService : IDisposable
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly AmazonDynamoDBClient _client ;
        private readonly BasicAWSCredentials _credentials;
        private readonly DynamoDBContext _context;

        public StorageService()
        {
            _accessKey = AppConfig.AwsAccessKey;
            _secretKey = AppConfig.AwsSecretKey;
            Console.WriteLine(_accessKey + " "+ _secretKey);

            _credentials = new BasicAWSCredentials(_accessKey, _secretKey);
            _client = new AmazonDynamoDBClient(_credentials, RegionEndpoint.USEast2);
            _context = new DynamoDBContext(_client);
            
        }

        public async Task CreateTableIfNeed(string tableName, string hashKey)
        {

            Console.WriteLine("Creating credentials and initializing DynamoDB client");

            Console.WriteLine("Verify table => " + tableName);
            var tableResponse = await _client.ListTablesAsync();
            if (!tableResponse.TableNames.Contains(tableName))
            {
                Console.WriteLine("Table not found, creating table => " + tableName);
                await _client.CreateTableAsync(new CreateTableRequest
                {
                    TableName = tableName,
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 1,
                        WriteCapacityUnits = 1
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = hashKey,
                            KeyType = KeyType.HASH
                        }
                    },
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = hashKey, AttributeType=ScalarAttributeType.S }
                    }
                });
                int tryCount = 0;
                bool isTableAvailable = false;
                while (!isTableAvailable)
                {
                    if(tryCount>5) throw new ApplicationException("TryCount>5 failed to create table");
                    Console.WriteLine("Waiting for table to be active..."); Thread.Sleep(5000);
                    var tableStatus = await _client.DescribeTableAsync(tableName);
                    isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
                    tryCount++;

                }
            }
        }

        public async void AddEntity<T>(T entity)
        {
            await _context.SaveAsync<T>(entity);

        }

        public async Task<List<T>> GetEntities<T>(int count) 
        {
            Console.WriteLine("Getting a list");
            List<ScanCondition> conditions = new List<ScanCondition>();
            var allDocs = await _context.ScanAsync<T>(conditions).GetRemainingAsync();
            Console.WriteLine("retrieved docs count : "+allDocs.Count);
            return allDocs;
        }

        public void Dispose()
        {
            _context?.Dispose();
            _client?.Dispose();
        }
    }
}
