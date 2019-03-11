using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DemoApps.Infrastructure
{
    public class PortunusSSOService
    {
        private const string BaseUrl = "http://localhost:8000";
        private const string IssueUrlFormat = "api/{0}/issue";
        private const string VerifyUrlFormat = "api/{0}/verify/{1}";
        private readonly IHttpClientFactory httpClientFactory;

        public PortunusSSOService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetTargetAuthAddressAsync<T>(string targetAppName, T payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            using (var client = httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var resp = await client.PostAsync(
                    string.Format(IssueUrlFormat, targetAppName),
                    new StringContent(json, Encoding.UTF8, "application/json"));

                return await resp.Content.ReadAsStringAsync();
            }
        }

        public async Task<T> VerifyToken<T>(string appName, string token) where T : class
        {
            using (var client = httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var resp = await client.GetAsync(string.Format(VerifyUrlFormat, appName, token));

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<T>(await resp.Content.ReadAsStringAsync());
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                else
                {
                    throw new Exception("Something went wrong when accessing Portunus SSO service");
                }
            }
        }
    }
}