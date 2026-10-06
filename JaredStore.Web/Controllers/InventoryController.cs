using Jared_s_Graduation_Project;
using Microsoft.AspNetCore.Mvc;

namespace JaredStore.Web.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index(int? storeID)
        {
            ViewBag.StoreID = storeID;

            var inventory = new List<Inventory>
            {
                new Inventory { InventoryID = "A1", StoreID = 1, ProductID = 1, Quantity = 5 },
                new Inventory { InventoryID = "B1", StoreID = 2, ProductID = 1, Quantity = 3 },
                new Inventory { InventoryID = "C1", StoreID = 3, ProductID = 1, Quantity = 4 }
            };

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

            var storeInventory = inventory
            .Where(item => item.StoreID == storeID)
            .ToList();

            ViewBag.Products = products;

            return View(storeInventory);
        }
    }
}
