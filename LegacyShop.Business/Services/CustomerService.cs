using System;
using System.Collections.Generic;
using System.Linq;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data;
using LegacyShop.Data.Entities;
using LegacyShop.Data.Repositories;

namespace LegacyShop.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService()
            : this(new UnitOfWork(new StoreDbContext()))
        {
        }

        public CustomerService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _unitOfWork.Customers.GetAll()
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();
        }

        public Customer GetCustomer(int id)
        {
            return _unitOfWork.Customers.GetCustomerWithOrders(id);
        }

        public void CreateCustomer(Customer customer)
        {
            Validate(customer);
            _unitOfWork.Customers.Add(customer);
            _unitOfWork.Complete();
        }

        public void UpdateCustomer(Customer customer)
        {
            Validate(customer);
            Customer existing = _unitOfWork.Customers.GetById(customer.CustomerId);
            if (existing == null)
            {
                throw new InvalidOperationException("The customer no longer exists.");
            }

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.Email = customer.Email;
            existing.Phone = customer.Phone;
            existing.Address = customer.Address;
            existing.City = customer.City;
            existing.PostalCode = customer.PostalCode;
            _unitOfWork.Complete();
        }

        public void DeleteCustomer(int id)
        {
            Customer customer = _unitOfWork.Customers.GetCustomerWithOrders(id);
            if (customer == null)
            {
                throw new InvalidOperationException("The customer no longer exists.");
            }

            if (customer.Orders.Any())
            {
                throw new InvalidOperationException("A customer with orders cannot be deleted.");
            }

            _unitOfWork.Customers.Remove(customer);
            _unitOfWork.Complete();
        }

        private static void Validate(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            if (string.IsNullOrWhiteSpace(customer.FirstName) ||
                string.IsNullOrWhiteSpace(customer.LastName) ||
                string.IsNullOrWhiteSpace(customer.Email))
            {
                throw new InvalidOperationException("First name, last name, and email are required.");
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
