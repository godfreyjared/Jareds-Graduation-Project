using Jared_s_Graduation_Project;
using JaredStore.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace JaredStore.Web.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index(int? storeID)
        {
            ViewBag.StoreID = storeID;

            var products = new List<Product>
            {
                new Product { ProductID = 1, ProductName = "AMD Ryzen 7 5700X", ProductCategory = "CPU", ProductPrice = 179.99m },
                new Product { ProductID = 2, ProductName = "Cooler Master Hyper 212", ProductCategory = "CPU Cooler", ProductPrice = 39.99m },
                new Product { ProductID = 3, ProductName = "MSI B550 Tomahawk", ProductCategory = "Motherboard", ProductPrice = 159.99m },
                new Product { ProductID = 4, ProductName = "Corsair Vengeance 32GB DDR4", ProductCategory = "RAM", ProductPrice = 74.99m },
                new Product { ProductID = 5, ProductName = "NVIDIA GeForce RTX 5060", ProductCategory = "GPU", ProductPrice = 299.99m },
                new Product { ProductID = 6, ProductName = "Samsung 990 EVO 1TB NVMe SSD", ProductCategory = "Storage", ProductPrice = 89.99m },
                new Product { ProductID = 7, ProductName = "Corsair RM750e 750W", ProductCategory = "Power Supply", ProductPrice = 109.99m },
                new Product { ProductID = 8, ProductName = "NZXT H5 Flow", ProductCategory = "Case", ProductPrice = 94.99m }
            };

            var storeInventory = StoreData.Inventory
                .Where(item => item.StoreID == storeID)
                .ToList();

            ViewBag.Products = products;

            return View(storeInventory);
        }
    }
}
