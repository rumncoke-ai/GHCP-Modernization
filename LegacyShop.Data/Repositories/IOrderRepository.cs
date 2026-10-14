using System.Collections.Generic;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IEnumerable<Order> GetOrdersWithCustomers();
        Order GetOrderWithDetails(int id);
    }
}
