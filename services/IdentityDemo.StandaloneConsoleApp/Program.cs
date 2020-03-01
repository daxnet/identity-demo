using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityDemo.StandaloneConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();
            var discoResponse = await client.GetDiscoveryDocumentAsync("https://localhost:7890");
            if (discoResponse.IsError)
            {
                Console.WriteLine(discoResponse.Error);
                return;
            }

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoResponse.TokenEndpoint,
                ClientId = "webapi",
                Scope = "api.weather.full_access",
                ClientSecret = "mysecret",
                UserName = "daxnet",
                Password = "P@ssw0rd123"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost:9000/api/weather");
            Console.WriteLine(response.IsSuccessStatusCode ?
                $"{response.StatusCode} {await response.Content.ReadAsStringAsync()}" : 
                response.StatusCode.ToString());
        }
    }
}
