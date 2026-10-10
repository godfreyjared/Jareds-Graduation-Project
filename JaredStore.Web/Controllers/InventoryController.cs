using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class InventoryController : Controller
    {
        // Display inventory for the selected store
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

        // Provide current inventory without reloading the page
        [HttpGet]
        public IActionResult GetStock(int storeID)
        {
            if (!IsValidStore(storeID))
            {
                return BadRequest("Invalid store.");
            }

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
        }

        // Employee adds stock or removes available (unreserved) stock.
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

            if (!IsValidStore(storeID) || quantity <= 0)
            {
                SetError("Select a valid store and enter a quantity greater than zero.");
                return RedirectToAction("Index", new { storeID });
            }

            if (adjustmentType != "add" && adjustmentType != "remove")
            {
                SetError("Select Add Stock or Remove Stock.");
                return RedirectToAction("Index", new { storeID });
            }

            var inventoryItem = FindInventory(storeID, productID);
            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            if (inventoryItem == null || product == null)
            {
                SetError("Product could not be found at the selected store.");
                return RedirectToAction("Index", new { storeID });
            }

            if (adjustmentType == "add")
            {
                if (inventoryItem.Quantity > int.MaxValue - quantity)
                {
                    SetError("The quantity is too large to add.");
                    return RedirectToAction("Index", new { storeID });
                }

                inventoryItem.Quantity += quantity;
                SetSuccess($"Added {quantity} {product.ProductName} to available stock at {GetStoreName(storeID)}.");
            }
            else
            {
                if (inventoryItem.Quantity < quantity)
                {
                    SetError("Cannot remove more than the available stock. Reserved items are protected.");
                    return RedirectToAction("Index", new { storeID });
                }

                inventoryItem.Quantity -= quantity;
                SetSuccess($"Removed {quantity} {product.ProductName} from available stock at {GetStoreName(storeID)}.");
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

            if (fromStoreID == toStoreID)
            {
                SetError("Please select two different stores.");
                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            var fromInventoryItem = FindInventory(fromStoreID, productID);
            var toInventoryItem = FindInventory(toStoreID, productID);

            if (fromInventoryItem == null || toInventoryItem == null)
            {
                SetError("Product could not be found.");
                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            if (quantity <= 0 || fromInventoryItem.Quantity < quantity)
            {
                SetError("Not enough inventory available to complete the transfer.");
                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            fromInventoryItem.Quantity -= quantity;
            toInventoryItem.Quantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            SetSuccess($"Successfully moved {quantity} {product?.ProductName} " +
                $"from {GetStoreName(fromStoreID)} to {GetStoreName(toStoreID)}.");

            return RedirectToAction("Index", new { storeID = fromStoreID });
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

            inventoryItem.Quantity -= quantity;
            inventoryItem.ReservedQuantity += quantity;
            inventoryItem.EmployeeReservedQuantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            SetSuccess($"Successfully reserved {quantity} {product?.ProductName} at {GetStoreName(storeID)}.");

            return RedirectToAction("Index", new { storeID });
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

            var inventoryItem = FindInventory(storeID, productID);

            if (inventoryItem == null)
            {
                SetError("Product is not available in this store.");
                return RedirectToAction("Index", new { storeID });
            }

            // Never return stock reserved by customer carts
            if (inventoryItem.EmployeeReservedQuantity < quantity ||
                inventoryItem.ReservedQuantity < quantity)
            {
                SetError("The requested quantity exceeds employee-reserved stock.");
                return RedirectToAction("Index", new { storeID });
            }

            inventoryItem.EmployeeReservedQuantity -= quantity;
            inventoryItem.ReservedQuantity -= quantity;
            inventoryItem.Quantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            SetSuccess($"Successfully returned {quantity} {product?.ProductName} " +
                $"to available inventory at {GetStoreName(storeID)}.");

            return RedirectToAction("Index", new { storeID });
        }

        private static Inventory? FindInventory(int storeID, int productID)
        {
            return StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == storeID && item.ProductID == productID);
        }

        private static bool IsValidStore(int storeID)
        {
            return storeID >= 1 && storeID <= 3;
        }

        private void SetError(string message)
        {
            TempData["Message"] = message;
            TempData["MessageType"] = "error";
        }

        private void SetSuccess(string message)
        {
            TempData["Message"] = message;
            TempData["MessageType"] = "success";
        }

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
    }
}
