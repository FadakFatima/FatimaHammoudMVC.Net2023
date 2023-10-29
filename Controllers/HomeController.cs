using Microsoft.AspNetCore.Mvc;

namespace CmsShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
