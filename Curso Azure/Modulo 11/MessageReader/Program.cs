using Azure.Messaging.ServiceBus;

namespace MessageReader
{
    public class Program
    {
        private const string storageConnectionString = "Endpoint=sb://sbnamespacetrainer.servicebus.windows.net/;SharedAccessKeyName=prinpipal;SharedAccessKey=5ZOlDSrUEwav6bSky0ePtDxn3yvsJEQzJ+ASbHuUoLM=;EntityPath=messagequeue";
        private const string queueName = "messagequeue";
        public static async Task Main(string[] args)
        {
            var client = new ServiceBusClient(storageConnectionString);
            var processor = client.CreateProcessor(queueName);
            try
            {

            
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();
            Console.WriteLine("Espera un minuto y pulsa una tecla para finalizar el procesamiento");
            Console.ReadKey();

            Console.WriteLine("\nDeteniendo el receptor...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Detenido recibir mensajes");
            }
            finally
            {
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.Write(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Recieved: {body}");
            await args.CompleteMessageAsync(args.Message);
        }
    }
}
