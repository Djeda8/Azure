using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
public static class FileParser
{
    [FunctionName("FileParser")]
    public static async Task<IActionResult> Run(
        [HttpTrigger("GET")] HttpRequest request)
    {
        string connectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
        /* Create a new instance of the BlobClient class by passing in your
           connectionString variable, a  "drop" string value, and a
           "records.json" string value to the constructor */
        BlobClient blob = new BlobClient(connectionString, "drop", "records.json");

        /* Use the BlobClient.DownloadAsync method to download the contents of
           the referenced blob asynchronously, and then store the result in
           a variable named "response" */
        var response = await blob.DownloadAsync();

         /* Return the value of the various content stored in the content
            variable by using the FileStreamResult class constructor */
        return new FileStreamResult(response?.Value?.Content, response?.Value?.ContentType);
    }
}