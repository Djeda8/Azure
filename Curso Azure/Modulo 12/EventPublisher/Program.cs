using Azure;
using Azure.Messaging.EventGrid;


 public class Program
    {
        private const string topicEndpoint = "https://hrtopictrainer.ukwest-1.eventgrid.azure.net/api/events";
        private const string topicKey = "V0C4wVjf7cdBdFV34HtMfenCi+jJmdjT+rS6fJ8V/pE=";
        public static async Task Main(string[] args)
        {
            Uri endpoint = new Uri(topicEndpoint);
            AzureKeyCredential credential = new AzureKeyCredential(topicKey);
            EventGridPublisherClient client = new EventGridPublisherClient(endpoint,credential);

            EventGridEvent firstEvent = new EventGridEvent(
                subject: $"New Employee: Alba Sutton",
                eventType: "Employees.Registration.New",
                dataVersion: "1.0",
                data: new
                {
                    FullName = "Alba Sutton",
                    Address = "4567 Pine Avenue, Edison, WA 97202"
                }
            );

            EventGridEvent secondEvent = new EventGridEvent(
                subject: $"New Employee: Alexandre Doyon",
                eventType: "Employees.Registration.New",
                dataVersion: "1.0",
                data: new
                {
                    FullName = "Alexandre Doyon",
                    Address = "456 College Street, Bow, WA 98107"
                }
            );

            await client.SendEventAsync(firstEvent);
            Console.WriteLine("First event published");

            await client.SendEventAsync(secondEvent);
            Console.WriteLine("Second event published");

        }
    }