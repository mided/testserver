using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommonInterfaces;
using Microsoft.Extensions.Configuration;

namespace OurBackendAPI.Services
{
    public class HttpWrapper : IHttpWrapper
    {
        private readonly Uri _invoiceServiceUrl;

        public HttpWrapper(IConfiguration configuration)
        {
            _invoiceServiceUrl = new Uri(configuration["InvoicesServiceUrl"]);
        }

        public async Task<(HttpStatusCode statusCode, string response)> Send(
            HttpMethod method, 
            string url, 
            string body = null,
            Dictionary<string, string> headers = null)
        {
            using var client = new HttpClient();
            using var message = new HttpRequestMessage();

            message.RequestUri = new Uri(_invoiceServiceUrl, url);
            message.Method = method;
            headers?.Keys.ToList().ForEach(key => headers.Add(key, headers[key]));
                    
            if (body != null)
            {
                message.Content = new StringContent(body);
            }

            var response = await client.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();

            return (response.StatusCode, content);
        }
    }
}
