using System;
using System.Collections.Generic;
using System.Linq;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data;
using LegacyShop.Data.Entities;
using LegacyShop.Data.Repositories;

namespace LegacyShop.Business.Services
{
    public class OrderService : IOrderService
    {
        private static readonly string[] AllowedStatuses = { "Pending", "Processing", "Completed", "Cancelled" };
        private readonly IUnitOfWork _unitOfWork;

        public OrderService()
            : this(new UnitOfWork(new StoreDbContext()))
        {
        }

        public OrderService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _unitOfWork.Orders.GetOrdersWithCustomers();
        }

        public Order GetOrder(int id)
        {
            return _unitOfWork.Orders.GetOrderWithDetails(id);
        }

        public Order CreateOrder(int customerId, IDictionary<int, int> productQuantities)
        {
            if (_unitOfWork.Customers.GetById(customerId) == null)
            {
                throw new InvalidOperationException("Select a valid customer.");
            }

            if (productQuantities == null || productQuantities.Count == 0)
            {
                throw new InvalidOperationException("An order must contain at least one product.");
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            foreach (KeyValuePair<int, int> requestedItem in productQuantities)
            {
                if (requestedItem.Value <= 0)
                {
                    throw new InvalidOperationException("Order quantities must be greater than zero.");
                }

                Product product = _unitOfWork.Products.GetById(requestedItem.Key);
                if (product == null || !product.IsActive)
                {
                    throw new InvalidOperationException("An ordered product does not exist or is inactive.");
                }

                if (product.StockQuantity < requestedItem.Value)
                {
                    throw new InvalidOperationException("There is not enough stock for " + product.Name + ".");
                }

                product.StockQuantity -= requestedItem.Value;
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = requestedItem.Value,
                    UnitPrice = product.Price
                });
            }

            order.TotalAmount = order.OrderItems.Sum(i => i.LineTotal);
            _unitOfWork.Orders.Add(order);
            _unitOfWork.Complete();
            return order;
        }

        public void UpdateOrderStatus(int id, string status)
        {
            if (!AllowedStatuses.Contains(status))
            {
                throw new InvalidOperationException("Select a valid order status.");
            }

            Order order = _unitOfWork.Orders.GetById(id);
            if (order == null)
            {
                throw new InvalidOperationException("The order no longer exists.");
            }

            order.Status = status;
            _unitOfWork.Complete();
        }

        public void DeleteOrder(int id)
        {
            Order order = _unitOfWork.Orders.GetOrderWithDetails(id);
            if (order == null)
            {
                throw new InvalidOperationException("The order no longer exists.");
            }

            foreach (OrderItem item in order.OrderItems)
            {
                item.Product.StockQuantity += item.Quantity;
            }

            _unitOfWork.Orders.Remove(order);
            _unitOfWork.Complete();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
