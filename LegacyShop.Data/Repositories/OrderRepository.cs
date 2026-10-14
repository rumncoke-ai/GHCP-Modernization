using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(StoreDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Order> GetOrdersWithCustomers()
        {
            return Context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public Order GetOrderWithDetails(int id)
        {
            return Context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems.Select(i => i.Product))
                .SingleOrDefault(o => o.OrderId == id);
        }
    }
}
