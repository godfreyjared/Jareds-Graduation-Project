using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = new List<CartItem>
            {
                new CartItem { ProductID = 1, Quantity = 2 }
            };

            return View(cart);
        }
    }
}