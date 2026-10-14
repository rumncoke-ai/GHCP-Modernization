using System;
using Microsoft.AspNetCore.Mvc;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data.Entities;

namespace LegacyShop.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public ActionResult Index()
        {
            return View(_customerService.GetCustomers());
        }

        public ActionResult Details(int id)
        {
            Customer customer = _customerService.GetCustomer(id);
            return customer == null ? (ActionResult)NotFound() : View(customer);
        }

        public ActionResult Create()
        {
            return View(new Customer());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerService.CreateCustomer(customer);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            Customer customer = _customerService.GetCustomer(id);
            return customer == null ? (ActionResult)NotFound() : View(customer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerService.UpdateCustomer(customer);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(customer);
        }

        public ActionResult Delete(int id)
        {
            Customer customer = _customerService.GetCustomer(id);
            return customer == null ? (ActionResult)NotFound() : View(customer);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _customerService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                Customer customer = _customerService.GetCustomer(id);
                if (customer == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError("", ex.Message);
                return View("Delete", customer);
            }
        }

    }
}
