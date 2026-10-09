
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
                    quantity = item.Quantity
                })
                .ToList();

            return Json(stock);
        }

        // Transfer inventory between stores
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
            // Employee authorization
            if (!EmployeeAccess.IsValid(username, password))
            {
                TempData["Message"] =
                    "Invalid employee username or password.";

                return RedirectToAction("Index",
                    new { storeID = fromStoreID });
            }

            // Validate store IDs
            if (fromStoreID < 1 || fromStoreID > 3 ||
                toStoreID < 1 || toStoreID > 3)
            {
                TempData["Message"] = "Invalid store selection.";

                return RedirectToAction("Index");
            }

            // Prevent transferring to the same store
            if (fromStoreID == toStoreID)
            {
                TempData["Message"] =
                    "Please select two different stores.";

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

            // Verify inventory exists in both stores
            if (fromInventoryItem == null || toInventoryItem == null)
            {
                TempData["Message"] =
                    "Product could not be found.";

                return RedirectToAction("Index",
                    new { storeID = fromStoreID });
            }

            // Prevent invalid or excessive transfers
            if (quantity <= 0 ||
                fromInventoryItem.Quantity < quantity)
            {
                TempData["Message"] =
                    "Not enough inventory available to complete the transfer.";

                return RedirectToAction("Index",
                    new { storeID = fromStoreID });
            }

            // Move available inventory between stores
            fromInventoryItem.Quantity -= quantity;
            toInventoryItem.Quantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            string fromStoreName = fromStoreID == 1 ? "Houston" :
                                   fromStoreID == 2 ? "Dallas" :
                                   "San Antonio";

            string toStoreName = toStoreID == 1 ? "Houston" :
                                 toStoreID == 2 ? "Dallas" :
                                 "San Antonio";

            TempData["Message"] =
                $"Successfully moved {quantity} {product?.ProductName} from {fromStoreName} to {toStoreName}.";

            return RedirectToAction("Index",
                new { storeID = fromStoreID });
        }
    }
}
