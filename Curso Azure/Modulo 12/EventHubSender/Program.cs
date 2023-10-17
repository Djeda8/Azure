using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

public class Program
{
    private const string connectionString = "Endpoint=sb://contosoeventhub2011.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1q79HLHxvCW6j+wbvAasJXenyMkhaLY1o+AEhFEqukc=";
    private const string eventHubName = "myeventhub";
    private const int numOfEvents = 3;
    static EventHubProducerClient producerClient;
    public static async Task Main(string[] args)
    {
        producerClient = new EventHubProducerClient(connectionString, eventHubName);

        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
        for (int i = 1; i <= numOfEvents; i++)
        {
            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
            {
                throw new Exception($"El evento {i} es demasiado largo y no puwde ser enviado");
            }
        }

        try
        {
            await producerClient.SendAsync(eventBatch);
        }
        finally
        {
            await producerClient.DisposeAsync();
        }
    }
}