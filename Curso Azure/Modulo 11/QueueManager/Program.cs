using System;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Threading.Tasks;

public class Program
{
    private const string connectionString ="DefaultEndpointsProtocol=https;AccountName=demostrainerstorage;AccountKey=80eGhsM3R5oGFCqjjjEvg4+uj4IvhcOFNhCIijHnaoPOhziWxFmK5PARmfFb/YmZV67PXYwBpL3c+AStMlpolA==;EndpointSuffix=core.windows.net";
    public static async Task Main(string[] args)
    {
        var rnd = new Random();
        var queueName = "demostrainerstorage";
        QueueClient queueClient = new(connectionString, queueName);
        queueClient.Create();

        Console.WriteLine($"El nombre de la Queue creada es: {queueClient.Name}");

        Console.WriteLine($"Añadiendo tres mensajes nuevos.");
        await queueClient.SendMessageAsync("Primer mensaje" + rnd.Next().ToString());
        await queueClient.SendMessageAsync("Segundo mensaje" + rnd.Next().ToString());

        SendReceipt receipt = await queueClient.SendMessageAsync("Tercer mensaje");

        Console.WriteLine($"Recuperando los primeros 10 mensajes");
        PeekedMessage[] peekedMessages = await queueClient.PeekMessagesAsync(maxMessages: 10);
        foreach (var peekedMessage in peekedMessages)
        {
            Console.WriteLine($"Mensaje: {peekedMessage.MessageText}");
        }

        await queueClient.UpdateMessageAsync(receipt.MessageId, receipt.PopReceipt, "El tercermensaje ha sido actualizado");

        Console.WriteLine("Pulsa una tecla para borrar todos los mensajes...");
        Console.ReadLine();
        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 10);
        foreach (var message in messages)
        {
            Console.WriteLine($"Borrado el Mensaje: {message.MessageText}");
            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        Console.WriteLine("Pulsa una tecla para borrar la Queue...");
        Console.ReadLine();
        await queueClient.DeleteAsync();

        Console.WriteLine("Done");
    }
}

