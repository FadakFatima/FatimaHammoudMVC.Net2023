using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CmsShoppingCart.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CmsShoppingCartContext _context;

        public OrdersController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

              
                return RedirectToAction("order", new { username = order.UserName });
                TempData["SuccessMessage"] = "Order placed successfully."; // Set success message in TempData

            }

            return View("~/Cart/Index");
        }

        public IActionResult order(string username)
        {
            var name = _context.Orders.FirstOrDefault(o => o.UserName == username);
            return View(name);
        }

       
    }
}
