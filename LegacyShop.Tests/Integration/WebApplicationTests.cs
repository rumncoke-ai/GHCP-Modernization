using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LegacyShop.Tests.Integration
{
    [TestClass]
    public class WebApplicationTests
    {
        private static WebApplicationFactory<Program> CreateFactory()
        {
            return new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");
                    builder.ConfigureServices(services =>
                    {
                        var products = new Mock<IProductService>();
                        products.Setup(service => service.GetProducts(null, false))
                            .Returns(new List<Product>());
                        products.Setup(service => service.GetProducts(null, true))
                            .Returns(new List<Product>());

                        var categories = new Mock<ICategoryService>();
                        categories.Setup(service => service.GetCategories())
                            .Returns(new List<Category>());

                        services.RemoveAll<IProductService>();
                        services.RemoveAll<ICategoryService>();
                        services.AddSingleton(products.Object);
                        services.AddSingleton(categories.Object);
                    });
                });
        }

        [TestMethod]
        public async Task AnonymousHomeRoute_ReturnsSuccess()
        {
            using WebApplicationFactory<Program> factory = CreateFactory();
            using HttpClient client = factory.CreateClient(
                new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            HttpResponseMessage response = await client.GetAsync("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task CategoryCreateForm_PreservesAntiForgeryProtection()
        {
            using WebApplicationFactory<Program> factory = CreateFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/Categories/Create");
            string body = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            StringAssert.Contains(body, "__RequestVerificationToken");
        }

        [TestMethod]
        public async Task StaticAssets_AreServedFromExistingPath()
        {
            using WebApplicationFactory<Program> factory = CreateFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/css/site.css");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task AdminAreaRoute_PreservesExistingUrl()
        {
            using WebApplicationFactory<Program> factory = CreateFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/Admin/Products");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
