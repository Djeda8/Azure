using System.Data.Common;
using System.Net.Http.Headers;
using System.Text.Json;
using StackExchange.Redis;

public class Program
{
    private const string connectionString = "rediscachedemostrainer1.redis.cache.windows.net:6380,password=pu0V1gAdZWTaapXax55g7RJA3kpljCR58AzCaMPot3E=,ssl=True,abortConnect=False";

    public static async Task Main(string[] args)
    {
        using (var cache = ConnectionMultiplexer.Connect(connectionString))
        {
            IDatabase db = cache.GetDatabase();
            Console.WriteLine($"Ejecuto el comando ping");
            var result = await db.ExecuteAsync("ping");
            Console.WriteLine($"PING = {result.Type} : {result}");
            Console.WriteLine("");

            Console.WriteLine($"Inserto un valor de 100 a la clave test:key");
            bool setValue = await db.StringSetAsync("test:key", "100");
            Console.WriteLine($"Resultado del SET: {setValue}");

            Console.WriteLine($"Recupero el valor de 100 a la clave test:key");
            string getValue = await db.StringGetAsync("test:key");
            Console.WriteLine($"Valor de la clave: {getValue}");
            Console.WriteLine("");

            Console.WriteLine($"Vacio la Redis");
            result = await db.ExecuteAsync("flushdb");
            Console.WriteLine($"FLUSHDB: {result}");
            Console.WriteLine("");

            // Ejemplo de objeto complejo
            Console.WriteLine($"Añado un objeto complejo");
            var stat = new GameStat("Soccer", new DateTime(1950, 7, 16), "FIFA World Cup", new[] { "Uruguay", "Brazil" }, new[] { ("Uruguay", 2), ("Brazil", 1) });

            string serializedValue = JsonSerializer.Serialize(stat);
            bool added = db.StringSet("event:1950-world-cup", serializedValue);
            Console.WriteLine($"REsultado del SET: " + added);

            Console.WriteLine($"Recuperar el dato por la clave event:1950-world-cup");
            var resultado = db.StringGet("event:1950-world-cup");
            Console.WriteLine(resultado);
        }
    }
}