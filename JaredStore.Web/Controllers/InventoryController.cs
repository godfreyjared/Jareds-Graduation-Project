
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

            var storeInventory = StoreData.Inventory
                .Where(item => item.StoreID == storeID)
                .ToList();

            ViewBag.Products = StoreData.Products;

            return View(storeInventory);
        }

        // Provide current inventory without reloading the page
        [HttpGet]
        public IActionResult GetStock(int storeID)
        {
            if (storeID < 1 || storeID > 3)
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
                    employeeReservedQuantity =
                        item.EmployeeReservedQuantity
                })
                .ToList();

            return Json(stock);
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

                return RedirectToAction("Index",
                    new { storeID = fromStoreID });
            }

            if (fromStoreID < 1 || fromStoreID > 3 ||
                toStoreID < 1 || toStoreID > 3)
            {
                SetError("Invalid store selection.");

                return RedirectToAction("Index");
            }

            if (fromStoreID == toStoreID)
            {
                SetError("Please select two different stores.");

                return RedirectToAction("Index",
                    new { storeID = fromStoreID });
            }

            var fromInventoryItem =
                StoreData.Inventory.FirstOrDefault(item =>
                    item.StoreID == fromStoreID &&
                    item.ProductID == productID);

            var toInventoryItem =
                StoreData.Inventory.FirstOrDefault(item =>
                    item.StoreID == toStoreID &&
                    item.ProductID == productID);

            if (fromInventoryItem == null || toInventoryItem == null)
            {
                SetError("Product could not be found.");

                return RedirectToAction("Index",
                    new { storeID = fromStoreID });
            }

            if (quantity <= 0 ||
                fromInventoryItem.Quantity < quantity)
            {
                SetError(
                    "Not enough inventory available to complete the transfer.");

                return RedirectToAction("Index",
                    new { storeID = fromStoreID });
            }

            fromInventoryItem.Quantity -= quantity;
            toInventoryItem.Quantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            string fromStoreName = GetStoreName(fromStoreID);
            string toStoreName = GetStoreName(toStoreID);

            SetSuccess(
                $"Successfully moved {quantity} {product?.ProductName} from {fromStoreName} to {toStoreName}.");

            return RedirectToAction("Index",
                new { storeID = fromStoreID });
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

                return RedirectToAction("Index",
                    new { storeID });
            }

            if (storeID < 1 || storeID > 3 || quantity <= 0)
            {
                SetError("Invalid store or quantity.");

                return RedirectToAction("Index",
                    new { storeID });
            }

            var inventoryItem =
                StoreData.Inventory.FirstOrDefault(item =>
                    item.StoreID == storeID &&
                    item.ProductID == productID);

            if (inventoryItem == null)
            {
                SetError("Product is not available in this store.");

                return RedirectToAction("Index",
                    new { storeID });
            }

            if (inventoryItem.Quantity < quantity)
            {
                SetError("Not enough available inventory to reserve.");

                return RedirectToAction("Index",
                    new { storeID });
            }

            inventoryItem.Quantity -= quantity;
            inventoryItem.ReservedQuantity += quantity;
            inventoryItem.EmployeeReservedQuantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            SetSuccess(
                $"Successfully reserved {quantity} {product?.ProductName} at {GetStoreName(storeID)}.");

            return RedirectToAction("Index",
                new { storeID });
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

                return RedirectToAction("Index",
                    new { storeID });
            }

            if (storeID < 1 || storeID > 3 || quantity <= 0)
            {
                SetError("Invalid store or quantity.");

                return RedirectToAction("Index",
                    new { storeID });
            }

            var inventoryItem =
                StoreData.Inventory.FirstOrDefault(item =>
                    item.StoreID == storeID &&
                    item.ProductID == productID);

            if (inventoryItem == null)
            {
                SetError("Product is not available in this store.");

                return RedirectToAction("Index",
                    new { storeID });
            }

            // Never return stock reserved by customer carts
            if (inventoryItem.EmployeeReservedQuantity < quantity ||
                inventoryItem.ReservedQuantity < quantity)
            {
                SetError(
                    "The requested quantity exceeds employee-reserved stock.");

                return RedirectToAction("Index",
                    new { storeID });
            }

            inventoryItem.EmployeeReservedQuantity -= quantity;
            inventoryItem.ReservedQuantity -= quantity;
            inventoryItem.Quantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            SetSuccess(
                $"Successfully returned {quantity} {product?.ProductName} to available inventory at {GetStoreName(storeID)}.");

            return RedirectToAction("Index",
                new { storeID });
        }

        // Mark failed operations as errors
        private void SetError(string message)
        {
            TempData["Message"] = message;
            TempData["MessageType"] = "error";
        }

        // Mark completed operations as successful
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
