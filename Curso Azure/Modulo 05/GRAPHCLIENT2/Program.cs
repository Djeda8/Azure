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
    private const string _clientId = "c5c2616b-fb7e-418d-b5aa-d905e090acae";
    private const string _tenantId = "2d336e3c-818a-4b67-939b-940e97bda6f8";
    private const string _clientSecret = "Afy8Q~rb0sCDZ9f4.SuIDW1OIW-PXJ4p4GAk8dvJ";

    public static async Task Main(string[] args)
    {
        PublicClientApplicationOptions options = new PublicClientApplicationOptions
        {
            AzureCloudInstance = AzureCloudInstance.AzurePublic,
            AadAuthorityAudience = AadAuthorityAudience.AzureAdMultipleOrgs,
            ClientId = _clientId,
        };

        var app = PublicClientApplicationBuilder
            .CreateWithApplicationOptions(options)
            .WithRedirectUri("http://localhost")
            .Build();
        List<string> scopes = new List<string>
        {"https://graph.microsoft.com/.default"};
        
        //MSAL
        // AuthenticationResult result;

        // result = await app
        //     .AcquireTokenInteractive(scopes)
        //     .ExecuteAsync();

        // Console.WriteLine($"Token:\t{result.AccessToken}");






       // using Azure.Identity;
        var options2 = new ClientSecretCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
        };

        // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            _tenantId, _clientId, _clientSecret, options2);

        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        var result = await graphClient.Me.GetAsync();

        Console.WriteLine($"Token:\t{result?.DisplayName}");
    }
}
