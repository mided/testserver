using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface IHttpWrapper
    {
        public Task<(HttpStatusCode statusCode, string response)> Send(
            HttpMethod method,
            string url,
            string body = null,
            Dictionary<string, string> headers = null);
    }
}
