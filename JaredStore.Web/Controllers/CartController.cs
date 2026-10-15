using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class CartController : Controller
    {
        // One shared cart and selected store for this demo
        private static readonly List<CartItem> cart = [];

        private static int? selectedStoreID;

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

            // Prevents the user from switching stores while their cart contains items.
            // This ensures reserved products remain associated with the correct store. 

            if (cart.Count > 0 && selectedStoreID != storeID)
            {
                TempData["Message"] =
                    "Please empty your cart or complete checkout before changing stores.";

                return RedirectToAction("Index", "Products");
            }

            // Stores the selected store ID in a static variable so it persists across requests.
            // This allows the application to remember which store the customer is shopping at.

            selectedStoreID = storeID;

            return RedirectToAction("Index", "Products");
        }

        // Displays the shopping cart and passes product and store information to the view.
        public IActionResult Index()
        {
            ViewBag.Products = StoreData.Products;
            ViewBag.StoreID = selectedStoreID;

            return View(cart);
        }
                
        // Reserve one item from the selected store NOT SURE ABOUT THIS ONE 

        public IActionResult AddToCart(int productID)
        {
            if (selectedStoreID == null)
            {
                TempData["Message"] =
                    "Please select a store before shopping.";

                return RedirectToAction("Index", "Products");
            }

            // Searches the selected store's inventory for the requested product.
            // If the product doesn't exist or is out of stock, the item cannot be added.

            var inventoryItem = FindInventory(
                selectedStoreID.Value, productID);

            if (inventoryItem == null || inventoryItem.Quantity <= 0)
            {
                ViewBag.Products = StoreData.Products;
                ViewBag.StoreID = selectedStoreID;
                ViewBag.Message = "Not enough inventory available.";

                return View("Index", cart);
            }

            // Checks whether the product is already in the cart.
            // If it is, increase the quantity; otherwise, add it as a new item.

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

            // Removes one unit from available inventory and adds it to reserved inventory.
            // This prevents other customers from purchasing an item already in someone's cart.

            inventoryItem.Quantity--;
            inventoryItem.ReservedQuantity++;

            ViewBag.Products = StoreData.Products;
            ViewBag.StoreID = selectedStoreID;

            return View("Index", cart);

            // Sends the updated product information and selected store to the cart page.
            // The page then displays the cart with the newly added item.
        }


        public IActionResult RemoveFromCart(int productID)
        {
            if (selectedStoreID == null)
            {
                return RedirectToAction("Index");
            }

            var existingItem = cart.FirstOrDefault(item =>
                item.ProductID == productID);

            var inventoryItem = FindInventory(
                selectedStoreID.Value, productID);

            // Searches for the product in the customer's cart and the selected store's inventory.
            // This identifies which cart item and inventory record need to be updated.

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

                // Returns one unit to available inventory and reduces reserved inventory.
                // This makes the removed item available for other customers to purchase.
            }

            return RedirectToAction("Index");
        }

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

                // Finds each product and multiplies its price by the quantity purchased.
                // The results are added together to calculate the customer's total purchase price.

                var inventoryItem = FindInventory(
                    selectedStoreID.Value, cartItem.ProductID);

                if (inventoryItem != null)
                {
                    inventoryItem.ReservedQuantity -= cartItem.Quantity;
                }

                // Removes purchased items from reserved inventory because the sale is complete.
                // Available inventory was already reduced when the items were added to the cart. 
            }

            cart.Clear();

            selectedStoreID = null;

            ViewBag.OrderTotal = orderTotal;

            return View("Checkout");

            //This clears the cart after checkout. 
        }

        private static Inventory? FindInventory(
            int storeID, int productID)
        {
            return StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == storeID &&
                item.ProductID == productID);
        }
        // Searches the inventory list for a specific product at a specific store.
        // This reusable method prevents repeating the same search code throughout the controller. 
    }
}
