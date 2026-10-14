using LegacyShop.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyShop.Tests.Data
{
    [TestClass]
    public class StoreDbContextTests
    {
        [TestMethod]
        public void Constructor_UsesConfiguredConnectionString()
        {
            const string connectionString =
                "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=LegacyShopTests;Integrated Security=True";

            using var context = new StoreDbContext(connectionString);

            Assert.AreEqual(connectionString, context.Database.Connection.ConnectionString);
        }
    }
}
