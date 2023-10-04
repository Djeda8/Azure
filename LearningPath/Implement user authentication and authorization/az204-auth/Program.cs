// dotnet add package Microsoft.Identity.Client
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace az204_auth
{
    class Program
    {
        private const string _clientId = "2322fc2b-88a3-4623-a6dd-5114dc0aecbe"; //APPLICATION_CLIENT_ID
        private const string _tenantId = "2d336e3c-818a-4b67-939b-940e97bda6f8"; //DIRECTORY_TENANT_ID

        public static async Task Main(string[] args)
        {
            var app = PublicClientApplicationBuilder
                .Create(_clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                .WithRedirectUri("http://localhost")
                .Build(); 
            string[] scopes = { "user.read" };
            AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

            Console.WriteLine($"Token:\t{result.AccessToken}");
        }
    }
}