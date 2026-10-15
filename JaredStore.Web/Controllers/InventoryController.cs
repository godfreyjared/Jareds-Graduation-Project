using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class InventoryController : Controller
    {
        [HttpGet]
        public IActionResult Index(int? storeID)
        {
            ViewBag.StoreID = storeID;
            ViewBag.Products = StoreData.Products;

            var storeInventory = StoreData.Inventory
                .Where(item => item.StoreID == storeID)
                .ToList();

            return View(storeInventory);
        }
        // Display inventory for the selected store

       [HttpGet]
        public IActionResult GetStock(int storeID)
        {
            if (!IsValidStore(storeID))
            {
                return BadRequest("Invalid store.");
            }

            //Prevents someone from requesting from an invalid store. 

            var stock = StoreData.Inventory
                .Where(item => item.StoreID == storeID)
                .Select(item => new
                {
                    productID = item.ProductID,
                    quantity = item.Quantity,
                    reservedQuantity = item.ReservedQuantity,
                    employeeReservedQuantity = item.EmployeeReservedQuantity
                })
                .ToList();
            return Json(stock);

            // Collects the current inventory quantities for the selected store.
            // Returns them as JSON so the website can update its displayed stock levels.

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdjustStock(
            int storeID,
            int productID,
            int quantity,
            string adjustmentType,
            string? username,
            string? password)
        {
            if (!EmployeeAccess.IsValid(username, password))
            {
                SetError("Invalid employee username or password.");
                return RedirectToAction("Index", new { storeID });
            }

            //This prevents someone from adjusting stock if they don't have a valid username and password.

            if (!IsValidStore(storeID) || quantity <= 0)
            {
                SetError("Select a valid store and enter a quantity greater than zero.");
                return RedirectToAction("Index", new { storeID });
            }

            //Prevents someone from picking a invalid store or entering a quantity of zero or less.

            if (adjustmentType != "add" && adjustmentType != "remove")
            {
                SetError("Select Add Stock or Remove Stock.");
                return RedirectToAction("Index", new { storeID });
            }
            
            //This ensures the adjustment type is either "add" or "remove".

            var inventoryItem = FindInventory(storeID, productID);
            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            // Finds the inventory record for the selected product and store.
            // Also finds the product information, including its name.

            if (inventoryItem == null || product == null)
            {
                SetError("Product could not be found at the selected store.");
                return RedirectToAction("Index", new { storeID });
            }

            // Prevents adjustments if the product or its inventory record cannot be found.
            // An error message is displayed instead.

            if (adjustmentType == "add")
            {
                if (inventoryItem.Quantity > int.MaxValue - quantity)
                {
                    SetError("The quantity is too large to add.");
                    return RedirectToAction("Index", new { storeID });
                }

            // Prevents the inventory quantity from exceeding the maximum value an integer can store.
            // This protects the program from errors caused by excessively large numbers.

                inventoryItem.Quantity += quantity;
                SetSuccess($"Added {quantity} {product.ProductName} to available stock at {GetStoreName(storeID)}.");

            // Adds the requested quantity to available inventory and displays a success message.
            }
            else
            {
                if (inventoryItem.Quantity < quantity)
                {
                    SetError("Cannot remove more than the available stock. Reserved items are protected.");
                    return RedirectToAction("Index", new { storeID });
                }

                //Prevents you from removing more stock than is available in the store. 

                inventoryItem.Quantity -= quantity;
                SetSuccess($"Removed {quantity} {product.ProductName} from available stock at {GetStoreName(storeID)}.");

                //Removes the requested quantity from available inventory and displays a success message..
            }

            return RedirectToAction("Index", new { storeID });
        }

        // Transfer available inventory between stores
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MoveInventory(
            int fromStoreID,
            int toStoreID,
            int productID,
            int quantity,
            string? username,
            string? password)
        {
            if (!EmployeeAccess.IsValid(username, password))
            {
                SetError("Invalid employee username or password.");
                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            if (!IsValidStore(fromStoreID) || !IsValidStore(toStoreID))
            {
                SetError("Invalid store selection.");
                return RedirectToAction("Index");
            }

            //Same checks as the other methods.

            if (fromStoreID == toStoreID)
            {
                SetError("Please select two different stores.");
                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            //Ensures inventory is being transferred between two different stores.

            var fromInventoryItem = FindInventory(fromStoreID, productID);
            var toInventoryItem = FindInventory(toStoreID, productID);

            // Finds the product's inventory record at both stores.
            // This tells the program where to remove stock and where to add it.

            if (fromInventoryItem == null || toInventoryItem == null)
            {
                SetError("Product could not be found.");
                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            // Prevents transfers if the product does not have an inventory record at both stores.

            if (quantity <= 0 || fromInventoryItem.Quantity < quantity)
            {
                SetError("Not enough inventory available to complete the transfer.");
                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            // Ensures the quantity is positive and the sending store has enough available inventory.

            fromInventoryItem.Quantity -= quantity;
            toInventoryItem.Quantity += quantity;

            // Subtracts the transferred quantity from the sending store.
            // Adds that same quantity to the receiving store.

            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            // Finds the product's information so its name can appear in the success message.

            SetSuccess($"Successfully moved {quantity} {product?.ProductName} " +
                $"from {GetStoreName(fromStoreID)} to {GetStoreName(toStoreID)}.");

            return RedirectToAction("Index", new { storeID = fromStoreID });

            // Displays a success message after completing the transfer.
        }

        // Employee moves available stock into reserve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReserveStock(
            int storeID,
            int productID,
            int quantity,
            string? username,
            string? password)
        {
            if (!EmployeeAccess.IsValid(username, password))
            {
                SetError("Invalid employee username or password.");
                return RedirectToAction("Index", new { storeID });
            }

            if (!IsValidStore(storeID) || quantity <= 0)
            {
                SetError("Invalid store or quantity.");
                return RedirectToAction("Index", new { storeID });
            }

            //Same checks, but this time for reserving stock.

            var inventoryItem = FindInventory(storeID, productID);

            if (inventoryItem == null)
            {
                SetError("Product is not available in this store.");
                return RedirectToAction("Index", new { storeID });
            }

            if (inventoryItem.Quantity < quantity)
            {
                SetError("Not enough available inventory to reserve.");
                return RedirectToAction("Index", new { storeID });
            }

            //Can't reserve a product that isn't there or exceeds the available quantity.

            inventoryItem.Quantity -= quantity;
            inventoryItem.ReservedQuantity += quantity;
            inventoryItem.EmployeeReservedQuantity += quantity;

            // Removes the quantity from available stock and adds it to reserved stock.
            // Also tracks how much was reserved specifically by employees.

            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            // Finds the product's name so it can be included in the confirmation message.

            SetSuccess($"Successfully reserved {quantity} {product?.ProductName} at {GetStoreName(storeID)}.");

            return RedirectToAction("Index", new { storeID });

            // Displays a success message after the stock is reserved.
        }

        // Employee returns manually reserved stock to the shelf
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnReservedStock(
            int storeID,
            int productID,
            int quantity,
            string? username,
            string? password)
        {
            if (!EmployeeAccess.IsValid(username, password))
            {
                SetError("Invalid employee username or password.");
                return RedirectToAction("Index", new { storeID });
            }

            if (!IsValidStore(storeID) || quantity <= 0)
            {
                SetError("Invalid store or quantity.");
                return RedirectToAction("Index", new { storeID });
            }

            //Same checks, but this time for returning stock.

            var inventoryItem = FindInventory(storeID, productID);

            // Finds the inventory record for the selected product at the selected store.

            if (inventoryItem == null)
            {
                SetError("Product is not available in this store.");
                return RedirectToAction("Index", new { storeID });
            }

            // The product must have an inventory record at the store before stock can be returned.

            if (inventoryItem.EmployeeReservedQuantity < quantity ||
                inventoryItem.ReservedQuantity < quantity)
            {
                SetError("The requested quantity exceeds employee-reserved stock.");
                return RedirectToAction("Index", new { storeID });
            }

            // Checks that enough employee-reserved stock AND total reserved stock exist.
            // This prevents employees from accidentally returning items reserved by customers.

            inventoryItem.EmployeeReservedQuantity -= quantity;
            inventoryItem.ReservedQuantity -= quantity;
            inventoryItem.Quantity += quantity;

            // Reduces employee reservations and total reservations by the returned quantity.
            // Adds those items back into available inventory for customers to purchase.

            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            // Finds the product's name so it can appear in the success message.

            SetSuccess($"Successfully returned {quantity} {product?.ProductName} " +
                $"to available inventory at {GetStoreName(storeID)}.");

            return RedirectToAction("Index", new { storeID });

            // Displays a success message after returning the reserved stock.
        }

        private static Inventory? FindInventory(int storeID, int productID)
        {
            return StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == storeID && item.ProductID == productID);
        }

        // Searches the inventory list for a specific product at a specific store.
        // This reusable method prevents repeating the same search code throughout the controller.

        private static bool IsValidStore(int storeID)
        {
            return storeID >= 1 && storeID <= 3;
        }

        // Checks whether the store ID is between 1 and 3.
        // These numbers represent Houston, Dallas, and San Antonio.

        private void SetError(string message)
        {
            TempData["Message"] = message;
            TempData["MessageType"] = "error";
        }

        // Stores an error message that can be displayed after redirecting the user.
        // The message type tells the website to display it as an error.

        private void SetSuccess(string message)
        {
            TempData["Message"] = message;
            TempData["MessageType"] = "success";
        }

        // Stores a success message that can be displayed after redirecting the user.
        // The message type tells the website to display it as a success.

        private static string GetStoreName(int storeID)
        {
            return storeID switch
            {
                1 => "Houston",
                2 => "Dallas",
                3 => "San Antonio",
                _ => "Unknown Store"
            };
        }

        // Converts a store ID number into the corresponding store name.
        // This makes messages easier for users to understand. 
    }
}
