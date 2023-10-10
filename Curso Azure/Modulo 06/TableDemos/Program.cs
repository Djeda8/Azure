using System;
using System.Linq;
using Azure;
using Azure.Data.Tables;


public class Program
{
    private const string storageUri = "https://mediastortrainer.table.core.windows.net/";
    private const string storageAccountName = "mediastortrainer";
    private const string storageAccountKey = "OOHIKk1/RuM/9JfRoT2K26qqGIBzF8dKLXllcA5ah3E28Y43A8s70cH8iKGHg7WgOQq17dOHmwkZy1ECtNpL6A==";

    public static void Main(string[] args)
    {
        Console.Clear();

        string tableName = "TableDemo";

        var tableClient = new TableClient(
        new Uri(storageUri),
        tableName,
        new TableSharedKeyCredential(storageAccountName, storageAccountKey));
        Console.WriteLine($"El nombre de la cuenta es {tableClient.AccountName}.");
        //-- dotnetrun

        tableClient.CreateIfNotExists();
        Console.WriteLine($"El nombre de la tabla creada es {tableClient.Name}.");
        //-- dotnetrun
        //-- Vete al portal a ver si la ha creado

        Random rnd = new Random();
        var partitionKey = "Producto";
        var rowKey = rnd.Next().ToString();
        var entity = new TableEntity(partitionKey, rowKey)
        {
            { "Producto", "Articulo" + rnd.Next(1, 10).ToString() },
            { "Precio", rnd.Next(1, 10) },
            { "Cantidad", rnd.Next(1, 21) }
        };

        tableClient.AddEntity(entity);
        Console.WriteLine($"Hemos añadido un nuevo registro.");

        Console.WriteLine($"Vamos a recuperar todos los registros añadidos con una query.");
        Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");
        
        foreach (TableEntity qEntity in queryResultsFilter)
        { Console.WriteLine($"Rowkey: " + qEntity.RowKey.ToString() + " - Producto: " + qEntity.GetString("Producto") + " - Precio:: " + qEntity.GetInt32("Precio") + " - Cantidad: " + qEntity.GetInt32("Cantidad")); }

        Console.WriteLine($"Vamos a recuperar todos los registros añadidos con Linq.");
        var query = (from qEntity in tableClient.Query<TableEntity>()
                     where qEntity.PartitionKey == "Producto"
                     select qEntity);

        foreach (TableEntity qEntity in queryResultsFilter)
        { Console.WriteLine($"Rowkey: " + qEntity.RowKey.ToString() + " - Producto: " + qEntity.GetString("Producto") + " - Precio:: " + qEntity.GetInt32("Precio") + " - Cantidad: " + qEntity.GetInt32("Cantidad")); }

        Console.WriteLine($"Se han encontrado un total de {queryResultsFilter.Count()} registros.");

        Console.Write("Pulsa Y para borrar la tabla. ");
        if (Console.ReadKey().Key.ToString() == "Y")
        { tableClient.Delete(); }
        //-- dotnet run
    }


}
