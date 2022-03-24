using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Net.Http;

namespace GraphHttpTest
{
    internal class Program
    {

        private static readonly string TenantId = "";
        private static readonly string ClientId = "";
        private static readonly string ClientSecret = "";

        private static void Main(string[] args)
        {
            var credentials = new ClientSecretCredential(TenantId, ClientId, ClientSecret);
            var client = new GraphServiceClient(credentials);
            var request = client.Domains
                .Request()
                .GetHttpRequestMessage();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetCredentials().AccessToken);
            //request.Version = new Version(1, 1);

            var response = new HttpClient().SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine($"Status {response.StatusCode}: {content}");
        }

        private static AuthenticationResult GetCredentials()
        {
            return ConfidentialClientApplicationBuilder
                .Create(ClientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, TenantId)
                .WithClientSecret(ClientSecret)
                .Build()
                .AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" })
                .ExecuteAsync().Result;
        }
    }
}
