using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Models;
using System.Text.Json;
using System.Text;

namespace vinncortez.Function
{
    public static class http_trigger_create_client
    {
        [FunctionName("http_trigger_create_client")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "client")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var connectionString = "mongodb://localhost:27017";
            var mongoClient = new MongoClient(connectionString);
            var collection = mongoClient.GetDatabase("azure-function").GetCollection<Client>("clients");

            var streamReader = new StreamReader(req.Body, Encoding.UTF8);

            var bodyString = streamReader.ReadToEnd();

            var client = JsonSerializer.Deserialize<Client>(bodyString);
            await collection.InsertOneAsync(client);

            return new OkResult();
        }
    }
}
