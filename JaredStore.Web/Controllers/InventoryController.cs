using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;
using JaredStore.Web.Models;

namespace JaredStore.Web.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index(int? storeID)
        {
            ViewBag.StoreID = storeID;

           

            var storeInventory = StoreData.Inventory
                .Where(item => item.StoreID == storeID)
                .ToList();

            ViewBag.Products = StoreData.Products;

            return View(storeInventory);
        }

        public IActionResult MoveInventory(
            
            int fromStoreID,
            int toStoreID,
            int productID,
            int quantity)
        
        {
            var fromInventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == fromStoreID &&
                item.ProductID == productID);

            var toInventoryItem = StoreData.Inventory.FirstOrDefault(item =>
                item.StoreID == toStoreID &&
                item.ProductID == productID);

            if (fromStoreID == toStoreID)
            {
                TempData["Message"] = "Please select two different stores.";

                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            if (fromInventoryItem == null || toInventoryItem == null)
            {
                TempData["Message"] = "Product could not be found.";

                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            if (quantity <= 0 || fromInventoryItem.Quantity < quantity)
            {
                TempData["Message"] = "Not enough inventory available to complete the transfer.";

                return RedirectToAction("Index", new { storeID = fromStoreID });
            }

            fromInventoryItem.Quantity -= quantity;
            toInventoryItem.Quantity += quantity;

            var product = StoreData.Products.FirstOrDefault(p => p.ProductID == productID);

            string fromStoreName = fromStoreID == 1 ? "Houston" :
                   fromStoreID == 2 ? "Dallas" :
                   "San Antonio";

            string toStoreName = toStoreID == 1 ? "Houston" :
                   toStoreID == 2 ? "Dallas" :
                   "San Antonio";

            TempData["Message"] =
                $"Successfully moved {quantity} {product?.ProductName} from {fromStoreName} to {toStoreName}.";

            return RedirectToAction("Index", new { storeID = fromStoreID });
        }
    }
}
