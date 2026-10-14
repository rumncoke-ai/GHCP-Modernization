using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer GetCustomerWithOrders(int id);
    }
}
