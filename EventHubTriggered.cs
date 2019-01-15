using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class EventHubTriggered
    {
        [FunctionName("EventHubTriggered")]
        public static async Task Run([EventHubTrigger("testfiltername", Connection = "TestFilterHub")] string myEventHubMessage, ILogger log)
        {
            log.LogInformation($"C# Event Hub trigger function processed a message: {myEventHubMessage}");

            // get some connection strings
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            string inputConnectionString = config.GetConnectionString("InputHub");
            string outputConnectionString = config.GetConnectionString("OutputHub");

            dynamic msg = JsonConvert.DeserializeObject(myEventHubMessage);
            if (msg.type == "input")
            {
                log.LogInformation($"Creating event hub client for {inputConnectionString}");
                EventHubClient client = EventHubClient.CreateFromConnectionString(inputConnectionString);

                EventData data = new EventData(System.Text.Encoding.UTF8.GetBytes(myEventHubMessage));
                await client.SendAsync(data);
            }
            else if (msg.type == "output")
            {
                log.LogInformation($"Creating event hub client for {outputConnectionString}");
                EventHubClient client = EventHubClient.CreateFromConnectionString(outputConnectionString);

                EventData data = new EventData(System.Text.Encoding.UTF8.GetBytes(myEventHubMessage));
                await client.SendAsync(data);
            }
        }
    }
}
