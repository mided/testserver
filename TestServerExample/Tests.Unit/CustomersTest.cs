using System;
using System.Linq;
using System.Threading.Tasks;
using CommonInterfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OurBackendAPI;
using OurBackendAPI.Controllers;
using OurBackendAPI.Models.Out;
using Tests.Integration;
using TestContext = Tests.Integration.TestContext;

namespace Tests.Unit
{
    public class CustomersTest
    {
        private IServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(it => it.GetSection("ConnectionStrings")["SqlServer"]).Returns("no sql connection");
            configurationMock.Setup(it => it["InvoicesServiceUrl"]).Returns("http://connection.fake");

            var services = new ServiceCollection();
            services.AddScoped(c => configurationMock.Object);

            services.AddScoped<CustomerController>();

            services.AddDbContext<DblContext>(o => o.UseInMemoryDatabase("Test"));
            services.AddScoped<IHttpWrapper, HttpFileWrapper>();
            services.AddSingleton<IStartupFilter, StartupFilter>();
            services.AddScoped<TestContext>();

            new Startup(configurationMock.Object).ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public async Task Customers()
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

            _serviceProvider.GetService<TestContext>().TestFolder = "TestData\\GetCustomer\\Invoice";

            var controller = _serviceProvider.GetService<CustomerController>();

            var result = ((OkObjectResult)await controller.GetById(10)).Value as CustomerOutModel;

            Assert.AreEqual(result.EmailAddress, "kathleen0@adventure-works.com");
            Assert.AreEqual(result.CustomerId, 10);
            Assert.AreEqual(result.Invoices.Count(), 19);
        }
    }
}