using System;
using System.Collections.Generic;
using LegacyShop.Data.Entities;

namespace LegacyShop.Business.Interfaces
{
    public interface ICustomerService : IDisposable
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomer(int id);
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
    }
}
