using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class Program
{
    private const string _clientId = "a78c4c82-035b-4f8b-bd6f-7d985f0aa0ce";
    private const string _tenantId = "2d336e3c-818a-4b67-939b-940e97bda6f8";
    private const string _clientSecret = "dHF8Q~MJSNj9~P3Azm4q88POFernik1y5B8Vgay~";

    public static async Task Main(string[] args)
    {
        var scopes = new[] { "User.Read" };

        // using Azure.Identity;
        // var options = new TokenCredentialOptions
        // {
        //     AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        // };

        // Callback function that receives the user prompt
        // Prompt contains the generated device code that you must
        // enter during the auth process in the browser
        // Func<DeviceCodeInfo, CancellationToken, Task> callback = (code, cancellation) =>
        // {
        //     Console.WriteLine(code.Message);
        //     return Task.FromResult(0);
        // };

        // https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
        // var deviceCodeCredential = new DeviceCodeCredential(
        //     callback, _tenantId, _clientId, options);

        //var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);

        // using Azure.Identity;
        var options = new InteractiveBrowserCredentialOptions
        {
            TenantId = _tenantId,
            ClientId = _clientId,
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            // MUST be http://localhost or http://localhost:PORT
            // See https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/System-Browser-on-.Net-Core
            RedirectUri = new Uri("http://localhost:50651"),
        };

        // https://learn.microsoft.com/dotnet/api/azure.identity.interactivebrowsercredential
        var interactiveCredential = new InteractiveBrowserCredential(options);

        var graphClient = new GraphServiceClient(interactiveCredential, scopes);

        var user = await graphClient.Me.GetAsync();
        Console.WriteLine($"Token:\t{user?.DisplayName}");
    }
}
