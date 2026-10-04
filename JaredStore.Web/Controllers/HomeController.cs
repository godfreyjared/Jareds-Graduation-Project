using JaredStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Jared_s_Graduation_Project;


namespace JaredStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var testProduct = new Product
            {
                ProductID = 1,
                ProductName = "AMD Ryzen 7 5700X",
                ProductCategory = "CPU",
                ProductPrice = 179.99m
            };

            return View(testProduct);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
