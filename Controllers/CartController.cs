using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {


        private readonly UserManager<AppUser> userManager;
        private readonly CmsShoppingCartContext context;

        public CartController(CmsShoppingCartContext context)
        {
            this.context = context;
        }

        //Get /cart  
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.
                GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            return View(cartVM);
        }


        //Get /cart/ add/5
       public async Task<IActionResult> Add(int id)
       {

            Product product = await context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.
                GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x =>
            x.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }

            return ViewComponent("SmallCart");


       }
        public async Task<IActionResult> AddToCart(int id, string discountCode = null)
        {
            Product product = await context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session
                .GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (!string.IsNullOrEmpty(discountCode) && discountCode.Equals("MU", StringComparison.OrdinalIgnoreCase))
            {
                // Apply a 10% discount
                foreach (var item in cart)
                {
                    // Convert Price to a floating-point type (e.g., decimal) if it's not already
                    decimal itemPrice = Convert.ToDecimal(item.Price);
                    itemPrice *= 0.9m; // 10% discount
                    item.Price = itemPrice;
                }
            }

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }

            return ViewComponent("SmallCart");
        }



        //Get /cart/ decrease/S
        public IActionResult Decrease(int id)
        {


            List<CartItem> cart = HttpContext.Session.
                GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(x =>
            x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);

            }

            return RedirectToAction("Index");

        }



        //Get /cart/ remove/S
        public IActionResult Remove(int id)
        {

            List<CartItem> cart = HttpContext.Session.
                GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(x => x.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);

            }

            return RedirectToAction("Index");

        }


        //Get /cart/ clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            //return RedirectToAction("Page", "Pages");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();

        }
        [HttpPost]
        public IActionResult Rate(int rating)
        {
            // Save the rating to the database
            var newRating = new Rating
            {
                RatingValue = rating
            };

            context.Ratings.Add(newRating);
            context.SaveChanges();

            // Calculate the overall average rating
            double averageRating = context.Ratings.Average(r => r.RatingValue);

            int averageRatingInt = (int)Math.Round(averageRating);

            // Store the integer average rating in TempData
            TempData["AverageRating"] = averageRatingInt;

            return RedirectToAction("Index", TempData["Success"] = "Thanks for choosing us");
        }




    }
}
