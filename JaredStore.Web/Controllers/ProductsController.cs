using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
            {
                new Product
                {
                    ProductID = 1,
                    ProductName = "AMD Ryzen 7 5700X",
                    ProductCategory = "CPU",
                    ProductPrice = 179.99m
                }
            };

            return View(products);
        }
    }
}