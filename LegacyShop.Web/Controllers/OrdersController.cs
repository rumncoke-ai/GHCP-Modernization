using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data.Entities;
using LegacyShop.Web.ViewModels;

namespace LegacyShop.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public OrdersController(
            IOrderService orderService,
            ICustomerService customerService,
            IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
        }

        public ActionResult Index()
        {
            return View(_orderService.GetOrders());
        }

        public ActionResult Details(int id)
        {
            Order order = _orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(MapDetails(order));
        }

        public ActionResult Create()
        {
            return View(BuildCreateViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreateViewModel model)
        {
            if (model.Products == null || !model.Products.Any(p => p.Selected))
            {
                ModelState.AddModelError("", "Select at least one product.");
            }

            if (model.Products != null && model.Products.Any(p => p.Selected && p.Quantity <= 0))
            {
                ModelState.AddModelError("", "Selected products must have a quantity greater than zero.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IDictionary<int, int> quantities = model.Products
                        .Where(p => p.Selected)
                        .ToDictionary(p => p.ProductId, p => p.Quantity);

                    Order order = _orderService.CreateOrder(model.CustomerId, quantities);
                    return RedirectToAction("Details", new { id = order.OrderId });
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            OrderCreateViewModel refreshed = BuildCreateViewModel(model.CustomerId);
            if (model.Products != null)
            {
                foreach (OrderProductSelectionViewModel product in refreshed.Products)
                {
                    OrderProductSelectionViewModel posted =
                        model.Products.SingleOrDefault(p => p.ProductId == product.ProductId);
                    if (posted != null)
                    {
                        product.Selected = posted.Selected;
                        product.Quantity = posted.Quantity;
                    }
                }
            }

            return View(refreshed);
        }

        public ActionResult Edit(int id)
        {
            Order order = _orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Statuses = StatusSelectList(order.Status);
            return View(order);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string status)
        {
            try
            {
                _orderService.UpdateOrderStatus(id, status);
                return RedirectToAction("Details", new { id = id });
            }
            catch (InvalidOperationException ex)
            {
                Order order = _orderService.GetOrder(id);
                if (order == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError("", ex.Message);
                ViewBag.Statuses = StatusSelectList(status);
                return View(order);
            }
        }

        public ActionResult Delete(int id)
        {
            Order order = _orderService.GetOrder(id);
            return order == null ? (ActionResult)NotFound() : View(MapDetails(order));
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        private OrderCreateViewModel BuildCreateViewModel(int selectedCustomerId = 0)
        {
            return new OrderCreateViewModel
            {
                CustomerId = selectedCustomerId,
                Customers = _customerService.GetCustomers()
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerId.ToString(),
                        Text = c.LastName + ", " + c.FirstName,
                        Selected = c.CustomerId == selectedCustomerId
                    })
                    .ToList(),
                Products = _productService.GetProducts(null, false)
                    .Select(p => new OrderProductSelectionViewModel
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Price = p.Price,
                        AvailableStock = p.StockQuantity
                    })
                    .ToList()
            };
        }

        private static IEnumerable<SelectListItem> StatusSelectList(string selected)
        {
            return new[] { "Pending", "Processing", "Completed", "Cancelled" }
                .Select(s => new SelectListItem { Text = s, Value = s, Selected = s == selected });
        }

        private static OrderDetailsViewModel MapDetails(Order order)
        {
            return new OrderDetailsViewModel
            {
                OrderId = order.OrderId,
                CustomerName = order.Customer.FirstName + " " + order.Customer.LastName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(i => new OrderLineViewModel
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.LineTotal
                }).ToList()
            };
        }

    }
}
