using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests.Integration
{
    public class CustomersTest
    {
        private HttpClient _httpClient;
        private string _path;
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<OurBackendAPI.Startup>()
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseTestServer();

            builder.ConfigureServices((services) =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.AddDbContext<DblContext>(o => o.UseInMemoryDatabase("Test"));
                services.AddScoped<IHttpWrapper, HttpFileWrapper>();
                services.AddSingleton<IStartupFilter, StartupFilter>();
                services.AddScoped<TestContext>();
                _serviceProvider = services.BuildServiceProvider();
            });

            var testServer = new TestServer(builder);
            _httpClient = testServer.CreateClient();

            _path = Path.Join(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "TestData");
        }

        [Test]
        public async Task GetCustomerById()
        {
            var context = _serviceProvider.GetService<DblContext>();
            context.Set<Customer>().Add(new Customer
            {
                CustomerId = 10,
                Title = "Ms.",
                FirstName = "Kathleen",
                MiddleName = "M.",
                LastName = "Garza",
                CompanyName = "Rural Cycle Emporium",
                EmailAddress = "kathleen0@adventure-works.com",
                Phone = "150-555-0127",
            });
            await context.SaveChangesAsync();

            var result = await Send(HttpMethod.Get, "/Customer/10", null, Path.Join(_path, "GetCustomer\\Invoice"));

            await Utils.CompareStringWithFile(result, Path.Join(_path, "GetCustomer", "get_customer_response.json"));
        }

        protected async Task<string> Send(HttpMethod method, string url, string content = null, string testFolder = null)
        {
            var message = new HttpRequestMessage(method, url);

            if (content != null)
            {
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            if (testFolder != null)
            {
                message.Headers.Add(TestContextMiddleware.TestFolderHeader, testFolder);
            }

            var result = await _httpClient.SendAsync(message);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }

            return result.Content != null ? await result.Content.ReadAsStringAsync() : string.Empty;
        }
    }
}