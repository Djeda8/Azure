using System.Reflection.Metadata;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs.Processor;

public class Program
{
    private const string ehubNamespaceConnectionString = "";
    private const string eventHubName = "myeventhub";
    private const string blobStorageConnectionString = "";
    static EventHubProducerClient bloContainerName = "";
    static BlobContainerClient storageClient;

    static EventProcessorClient processor;
    public static async Task Main(string[] args)
    {
        
    }
}