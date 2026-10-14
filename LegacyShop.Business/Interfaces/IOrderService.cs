using System;
using System.Collections.Generic;
using LegacyShop.Data.Entities;

namespace LegacyShop.Business.Interfaces
{
    public interface IOrderService : IDisposable
    {
        IEnumerable<Order> GetOrders();
        Order GetOrder(int id);
        Order CreateOrder(int customerId, IDictionary<int, int> productQuantities);
        void UpdateOrderStatus(int id, string status);
        void DeleteOrder(int id);
    }
}
