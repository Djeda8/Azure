using System;
using Azure.Messaging.ServiceBus;

namespace MessagePublisher
{
    public class Program
    {
        private const string storageConnectionString = "Endpoint=sb://sbnamespacetrainer.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=uPCViBv1q+wO2WD9MY9Z3Asp3/nxiNGFu+ASbEvltSY=";
        private const string queueName = "messagequeue";
        private const int numOfMessages = 3;
        public static async Task Main(string[] args)
        {
            var client = new ServiceBusClient(storageConnectionString);
            var sender = client.CreateSender(queueName);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numOfMessages; i++)
            {
                if(!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                {
                    throw new Exception($"El mensaje {i} no puede entrar en la pila de mensajes.");
                }
            }

            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"Una pila de {numOfMessages} mensajes ha sido publicada en la cola.");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}