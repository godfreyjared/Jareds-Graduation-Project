using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;
using JaredStore.Web.Models;

namespace JaredStore.Web.Controllers
{
    public class CartController : Controller
    {
        private static List<CartItem> cart = new List<CartItem>();

        private static List<Product> products = new List<Product>
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

        public IActionResult Index()
        {
            ViewBag.Products = products;
            return View(cart);
        }

        public IActionResult AddToCart(int productID)
        {
            var inventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == 1 &&
                item.ProductID == productID);

            var existingItem = cart.FirstOrDefault(item =>
                item.ProductID == productID);

            int quantityAlreadyInCart = 0;

            if (existingItem != null)
                {
                    quantityAlreadyInCart = existingItem.Quantity;
                }

            if (inventoryItem == null || quantityAlreadyInCart >= inventoryItem.Quantity)
                {
                    ViewBag.Products = products;
                    ViewBag.Message = "Not enough inventory available.";

                    return View("Index", cart);
                }

            if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
            else
                {
                    cart.Add(new CartItem
                    {
                        ProductID = productID,
                        Quantity = 1
                    });
                }

            ViewBag.Products = products;

            return View("Index", cart);
        }

        public IActionResult RemoveFromCart(int productID)
        {
            var existingItem = cart.FirstOrDefault(item => item.ProductID == productID);

            if (existingItem != null)
            {
                if (existingItem.Quantity > 1)
                {
                    existingItem.Quantity--;
                }
                else
                {
                    cart.Remove(existingItem);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            decimal orderTotal = 0;

            foreach (var cartItem in cart)
            {
                var product = products.FirstOrDefault(p =>
                    p.ProductID == cartItem.ProductID);

                if (product != null)
                {
                    orderTotal += product.ProductPrice * cartItem.Quantity;
                }

                var inventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                    item.StoreID == 1 &&
                    item.ProductID == cartItem.ProductID);

                if (inventoryItem != null)
                {
                    inventoryItem.Quantity -= cartItem.Quantity;
                }
            }

            cart.Clear();

            ViewBag.OrderTotal = orderTotal;

            return View("Checkout");
        }
    }
}