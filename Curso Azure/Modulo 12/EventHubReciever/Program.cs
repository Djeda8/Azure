using System.Reflection.Metadata;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Consumer;

public class Program
{
    private const string ehubNamespaceConnectionString = "Endpoint=sb://contosoeventhub2011.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VHqMbFK9NNAkK3Mw9gandtGZJcbP2DesP+AEhFt8fI4=";
    private const string eventHubName = "myeventhub";
    private const string blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=mystoragedou;AccountKey=YNscCJo61lDF2+ZdnKjy8cuU1D9BdG5CPk3+0x37gUipUcffSIudHCFocugn2jO4YUQ1sjW7GGfL+ASt2NjT2A==;EndpointSuffix=core.windows.net";
    static string blobContainerName = "mycontainer";
    static BlobContainerClient storageClient;

    static EventProcessorClient processor;
    public static async Task Main(string[] args)
    {
        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

        storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

        processor = new EventProcessorClient(storageClient, consumerGroup, ehubNamespaceConnectionString, eventHubName);
        processor.ProcessEventAsync += ProcessEventHandler;
        processor.ProcessErrorAsync += ProcessErrorHandler;

        await processor.StartProcessingAsync();
        await Task.Delay(TimeSpan.FromSeconds(10));
        await processor.StopProcessingAsync();
    }

    static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
    {
        Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
        await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
    }
    static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
    {
        Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
        Console.WriteLine(eventArgs.Exception.Message);
        return Task.CompletedTask;
    }
}