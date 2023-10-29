using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin, editor")]
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly CmsShoppingCartContext _context;

        public OrdersController(CmsShoppingCartContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }
        //GET /admin/Orders/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Order order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                TempData["Error"] = "The order does not exist!";
            }
            else
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The order has been deleted!";

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            var orders =  _context.Orders.ToList();
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }


    }

}
