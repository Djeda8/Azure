using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;


public class Program
{
    private const string blobServiceEndpoint = "https://mediastortrainer.blob.core.windows.net/";
    private const string storageAccountName = "mediastortrainer";
    private const string storageAccountKey = "";

    public static async Task Main(string[] args)
    {
        StorageSharedKeyCredential accountCredentials = new(storageAccountName, storageAccountKey);
        BlobServiceClient serviceClient = new(new Uri(blobServiceEndpoint), accountCredentials);

        AccountInfo info = await serviceClient.GetAccountInfoAsync();
        await Console.Out.WriteLineAsync($"Connected to Azure Sorage Account");
        await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");
        await Console.Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");
        await Console.Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

        await EnumerateContainersAsync(serviceClient);

        string existingContainerName = "raster-graphics";
        await EnumerateBlobsAsync(serviceClient, existingContainerName);

        string newContainerName = "raster-graphics";
        BlobContainerClient containerClient = await GetContainerAsync(serviceClient, newContainerName);

        string uploadedBlobName = "veggie.jpg";
        BlobClient blobClient = await GetBlobAsync(containerClient, uploadedBlobName);

        var filePath = @"C:\Users\dojeda\Desktop\Curso Azure\Azure\Modulo 2\Starter\Images\veggie.jpg";
        filePath = await UploadBlobAsync(blobClient, filePath);
        await Console.Out.WriteLineAsync($"File uploaded:\t{filePath}");

    }


    private static async Task EnumerateContainersAsync(BlobServiceClient serviceClient)
    {
        await foreach (BlobContainerItem container in serviceClient.GetBlobContainersAsync())
        {
            await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
        }
    }

    private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
    {
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        await Console.Out.WriteLineAsync($"Searching:\t{container.Name}");
        await foreach (BlobItem blob in container.GetBlobsAsync())
        {
            await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
        }
    }

    private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
    {
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
        await Console.Out.WriteLineAsync($"New Container:\t{container.Name}");
        return container;
    }

    private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
    {
        BlobClient blob = client.GetBlobClient(blobName);
        bool exists = await blob.ExistsAsync();
        if (!exists)
        {
            await Console.Out.WriteLineAsync($"Blob {blob.Name} not found!");
        }
        else
            await Console.Out.WriteLineAsync($"Blob Found, URI:\t{blob.Uri}");
        return blob;
    }


    private static async Task<string> UploadBlobAsync(BlobClient client, string blobPath)
    {
        BlobContentInfo info = await client.UploadAsync(blobPath, true);
        return info.LastModified.ToString();
    }
}