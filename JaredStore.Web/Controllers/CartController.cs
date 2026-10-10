
using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class CartController : Controller
    {
        // One shared cart and selected store for this demo
        private static readonly List<CartItem> cart = new();

        private static int? selectedStoreID = null;

        // Allow other controllers to see the selected store
        public static int? SelectedStoreID => selectedStoreID;

        public static bool CartHasItems => cart.Count > 0;

        // Select a store before shopping
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectStore(int storeID)
        {
            if (storeID < 1 || storeID > 3)
            {
                TempData["Message"] = "Invalid store selection.";
                return RedirectToAction("Index", "Products");
            }

            if (cart.Count > 0 && selectedStoreID != storeID)
            {
                TempData["Message"] =
                    "Please empty your cart or complete checkout before changing stores.";

                return RedirectToAction("Index", "Products");
            }

            selectedStoreID = storeID;

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Index()
        {
            ViewBag.Products = StoreData.Products;
            ViewBag.StoreID = selectedStoreID;

            return View(cart);
        }

        // Reserve one item from the selected store
        public IActionResult AddToCart(int productID)
        {
            if (selectedStoreID == null)
            {
                TempData["Message"] =
                    "Please select a store before shopping.";

                return RedirectToAction("Index", "Products");
            }

            var inventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == selectedStoreID.Value &&
                item.ProductID == productID);

            if (inventoryItem == null || inventoryItem.Quantity <= 0)
            {
                ViewBag.Products = StoreData.Products;
                ViewBag.StoreID = selectedStoreID;
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

            // Reserve inventory at the selected store
            inventoryItem.Quantity--;
            inventoryItem.ReservedQuantity++;

            ViewBag.Products = StoreData.Products;
            ViewBag.StoreID = selectedStoreID;

            return View("Index", cart);
        }

        // Return one item to its original store
        public IActionResult RemoveFromCart(int productID)
        {
            if (selectedStoreID == null)
            {
                return RedirectToAction("Index");
            }

            var existingItem = cart.FirstOrDefault(item =>
                item.ProductID == productID);

            var inventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == selectedStoreID.Value &&
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

                if (inventoryItem != null &&
                    inventoryItem.ReservedQuantity > 0)
                {
                    inventoryItem.Quantity++;
                    inventoryItem.ReservedQuantity--;
                }
            }

            return RedirectToAction("Index");
        }

        // Complete purchase at the selected store
        public IActionResult Checkout()
        {
            if (selectedStoreID == null || cart.Count == 0)
            {
                return RedirectToAction("Index");
            }

            decimal orderTotal = 0;

            foreach (var cartItem in cart)
            {
                var product = StoreData.Products.FirstOrDefault(p =>
                    p.ProductID == cartItem.ProductID);

                if (product != null)
                {
                    orderTotal +=
                        product.ProductPrice * cartItem.Quantity;
                }

                var inventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                    item.StoreID == selectedStoreID.Value &&
                    item.ProductID == cartItem.ProductID);

                if (inventoryItem != null)
                {
                    // Available stock was reduced when reserved
                    inventoryItem.ReservedQuantity -= cartItem.Quantity;
                }
            }

            cart.Clear();

            // Allow a new store selection after checkout
            selectedStoreID = null;

            ViewBag.OrderTotal = orderTotal;

            return View("Checkout");
        }
    }
}
