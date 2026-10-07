using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;
using JaredStore.Web.Models;

namespace JaredStore.Web.Controllers
{
    public class CartController : Controller
    {
        private static List<CartItem> cart = new List<CartItem>();

        public IActionResult Index()
        {
            ViewBag.Products = StoreData.Products;
            return View(cart);
        }

        public IActionResult AddToCart(int productID)
        {
            var inventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == 1 &&
                item.ProductID == productID);

            if (inventoryItem == null || inventoryItem.Quantity <= 0)
            {
                ViewBag.Products = StoreData.Products;
                ViewBag.Message = "Not enough inventory available.";

                return View("Index", cart);
            }

            var existingItem = cart.FirstOrDefault(item =>
                item.ProductID == productID);

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

            // Move one item from available inventory to reserved inventory
            inventoryItem.Quantity--;
            inventoryItem.ReservedQuantity++;

            ViewBag.Products = StoreData.Products;

            return View("Index", cart);
        }

        public IActionResult RemoveFromCart(int productID)
        {
            var existingItem = cart.FirstOrDefault(item =>
                item.ProductID == productID);

            var inventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == 1 &&
                item.ProductID == productID);

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

                // Return one reserved item back to available inventory
                if (inventoryItem != null && inventoryItem.ReservedQuantity > 0)
                {
                    inventoryItem.Quantity++;
                    inventoryItem.ReservedQuantity--;
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            decimal orderTotal = 0;

            foreach (var cartItem in cart)
            {
                var product = StoreData.Products.FirstOrDefault(p =>
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
                    // Items were already removed from available inventory
                    // when they were added to the cart.
                    inventoryItem.ReservedQuantity -= cartItem.Quantity;
                }
            }

            cart.Clear();

            ViewBag.OrderTotal = orderTotal;

            return View("Checkout");
        }
    }
}