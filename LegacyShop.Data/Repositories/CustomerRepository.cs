using System.Data.Entity;
using System.Linq;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(StoreDbContext context)
            : base(context)
        {
        }

        public Customer GetCustomerWithOrders(int id)
        {
            return Context.Customers
                .Include(c => c.Orders)
                .SingleOrDefault(c => c.CustomerId == id);
        }
    }
}
