using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommonInterfaces;
using Newtonsoft.Json;

namespace Tests.Integration
{
    public class HttpFileWrapper : IHttpWrapper
    {
        private readonly TestContext _testContext;

        public HttpFileWrapper(TestContext testContext)
        {
            _testContext = testContext;
        }

        public async Task<(HttpStatusCode statusCode, string response)> Send(HttpMethod method, string url, string body = null, Dictionary<string, string> headers = null)
        {
            var jsonHeaders = headers != null ? JsonConvert.SerializeObject(headers) : null;

            var fileName = $"{method}_{GetDeterministicHashCode(url)}_{GetDeterministicHashCode(jsonHeaders)}_{GetDeterministicHashCode(body)}.json";

            var result = await File.ReadAllTextAsync(Path.Combine(_testContext.TestFolder, fileName));
            
            return (HttpStatusCode.OK, result);
        }

        private static string GetDeterministicHashCode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "0";
            }

            unchecked
            {
                var hash1 = (5381 << 16) + 5381;
                var hash2 = hash1;

                for (var i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return Math.Abs((hash1 + (hash2 * 1566083941))).ToString();
            }
        }
	}
}
