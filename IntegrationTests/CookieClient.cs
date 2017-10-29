using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IntegrationTests.Utilities;

namespace IntegrationTests
{
    public class CookieClient
    {
        readonly HttpClient Client;

        public IDictionary<string, string> Cookies { get; }

        public CookieClient(HttpClient client)
        {
            Client = client;
            Cookies = new Dictionary<string, string>();
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            CookiesHelper.PutCookiesOnRequest(httpRequestMessage, Cookies);

            return await Send(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, IDictionary<string, string> formPostBodyData)
        {
            var httpRequestMessage = PostRequestHelper.Create(requestUri, formPostBodyData);
            CookiesHelper.PutCookiesOnRequest(httpRequestMessage, Cookies);

            return await Send(httpRequestMessage);
        }

        async Task<HttpResponseMessage> Send(HttpRequestMessage request)
        {
            var response = await Client.SendAsync(request);
            UpdateCookies(CookiesHelper.ExtractCookiesFromResponse(response));

            return response;
        }

        void UpdateCookies(IDictionary<string, string> newCookies)
        {
            foreach(var newCookie in newCookies)
            {
                if(Cookies.ContainsKey(newCookie.Key))
                {
                    Cookies[newCookie.Key] = newCookie.Value;
                }
                else
                {
                    Cookies.Add(newCookie);    
                }
            }
        }
    }
}
