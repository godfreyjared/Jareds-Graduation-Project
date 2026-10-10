
using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Inventory = StoreData.Inventory;

            // Store selected through the CartController
            ViewBag.StoreID = CartController.SelectedStoreID;
            ViewBag.CartHasItems = CartController.CartHasItems;

            return View(StoreData.Products);
        }

        // Live stock updates for the selected store
        [HttpGet]
        public IActionResult GetStock(int storeID)
        {
            if (storeID < 1 || storeID > 3)
            {
                return BadRequest("Invalid store selection.");
            }

            var stock = StoreData.Inventory
                .Where(i => i.StoreID == storeID)
                .Select(i => new
                {
                    productID = i.ProductID,
                    quantity = i.Quantity
                })
                .ToList();

            return Json(stock);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            string productName,
            string productCategory,
            decimal productPrice,
            string username,
            string password)
        {
            if (!EmployeeAccess.IsValid(username, password))
            {
                ViewBag.Message = "Invalid employee username or password.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(productName) ||
                string.IsNullOrWhiteSpace(productCategory) ||
                productPrice <= 0)
            {
                ViewBag.Message = "Please enter valid product information.";
                return View();
            }

            int newProductID = StoreData.Products.Count == 0
                ? 1
                : StoreData.Products.Max(p => p.ProductID) + 1;

            var newProduct = new Product
            {
                ProductID = newProductID,
                ProductName = productName.Trim(),
                ProductCategory = productCategory.Trim(),
                ProductPrice = productPrice
            };

            StoreData.Products.Add(newProduct);

            // Create inventory records for all three stores
            foreach (int storeID in new[] { 1, 2, 3 })
            {
                StoreData.Inventory.Add(new Inventory
                {
                    InventoryID =
                        $"{(char)('A' + storeID - 1)}{newProductID}",
                    StoreID = storeID,
                    ProductID = newProductID,
                    Quantity = 0,
                    ReservedQuantity = 0
                });
            }

            TempData["Message"] =
                $"{newProduct.ProductName} created successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            int productID,
            string productName,
            string productCategory,
            decimal productPrice,
            string username,
            string password)
        {
            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            if (product == null)
            {
                return NotFound();
            }

            if (!EmployeeAccess.IsValid(username, password))
            {
                ViewBag.Message = "Invalid employee username or password.";
                return View(product);
            }

            if (string.IsNullOrWhiteSpace(productName) ||
                string.IsNullOrWhiteSpace(productCategory) ||
                productPrice <= 0)
            {
                ViewBag.Message = "Please enter valid product information.";
                return View(product);
            }

            product.ProductName = productName.Trim();
            product.ProductCategory = productCategory.Trim();
            product.ProductPrice = productPrice;

            TempData["Message"] =
                $"{product.ProductName} updated successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(
            int productID,
            string username,
            string password)
        {
            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            if (product == null)
            {
                return NotFound();
            }

            if (!EmployeeAccess.IsValid(username, password))
            {
                ViewBag.Message =
                    "Invalid employee username or password.";

                return View("Delete", product);
            }

            bool hasReservations = StoreData.Inventory.Any(item =>
                item.ProductID == productID &&
                item.ReservedQuantity > 0);

            if (hasReservations)
            {
                TempData["Message"] =
                    "Cannot delete this product while items are reserved in a cart.";

                return RedirectToAction("Index");
            }

            StoreData.Inventory.RemoveAll(item =>
                item.ProductID == productID);

            StoreData.Products.Remove(product);

            TempData["Message"] =
                $"{product.ProductName} deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
