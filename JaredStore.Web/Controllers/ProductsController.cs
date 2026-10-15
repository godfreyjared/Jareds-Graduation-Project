
using Microsoft.AspNetCore.Mvc;
using Jared_s_Graduation_Project;

namespace JaredStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Inventory = StoreData.Inventory;
            ViewBag.StoreID = CartController.SelectedStoreID;
            ViewBag.CartHasItems = CartController.CartHasItems;

            // Sends inventory information, the selected store, and cart status to the webpage.
            // This allows the product page to display the correct information. 

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

            // Prevents users from selecting a store that does not exist.

            var stock = StoreData.Inventory
                .Where(i => i.StoreID == storeID)
                .Select(i => new
                {
                    productID = i.ProductID,
                    quantity = i.Quantity
                })
                .ToList();

            // Finds all inventory records for the selected store.
            // Collects each product's ID and available quantity.

            return Json(stock);

            // Sends the stock information to the website as JSON.
            // JavaScript can use this information to update displayed inventory without refreshing the entire page. 
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Displays the webpage where employees can enter information to create a new product.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            string productName,
            string productCategory,
            decimal productPrice,
            string username,
            string password)

        // Receives the new product information and employee credentials submitted through the form.
        // The antiforgery token helps protect the form from unauthorized cross-site requests.

        {
            if (!EmployeeAccess.IsValid(username, password))
            {
                ViewBag.Message =
                    "Invalid employee username or password.";

                return View();
            }

            // Checks the employee's username and password.
            // Invalid credentials prevent the employee from creating a product.

            if (!IsValidProduct(
                productName, productCategory, productPrice))
            {
                ViewBag.Message =
                    "Please enter valid product information.";

                return View();
            }

            // Checks that the product name, category, and price are valid.
            // Invalid information prevents the product from being created.

            int newProductID = StoreData.Products.Count == 0
                ? 1
                : StoreData.Products.Max(p => p.ProductID) + 1;

            // Creates a unique product ID by taking the highest existing ID and adding one.
            // If no products exist, the first product receives ID 1.

            var newProduct = new Product
            {
                ProductID = newProductID,
                ProductName = productName.Trim(),
                ProductCategory = productCategory.Trim(),
                ProductPrice = productPrice
            };

            // Creates a new Product object using the information entered by the employee.
            // Trim removes unnecessary spaces from the beginning and end of text.

            StoreData.Products.Add(newProduct);

            // Adds the newly created product to the application's product list.

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

                // Creates an inventory record for the new product at all three stores.
                // Each record starts with zero stock and receives a store-specific inventory ID.
            }

            TempData["Message"] =
                $"{newProduct.ProductName} created successfully.";

            // Saves a success message that can be displayed after redirecting the employee.

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == id);

            // Searches the product list for the product matching the selected ID.

            if (product == null)
            {
                return NotFound();
            }

            // Returns a Not Found response if the requested product does not exist.

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

        // Receives the updated product information and employee credentials from the edit form.
        // The antiforgery token helps protect the form from unauthorized cross-site requests.

        {
            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            // Finds the existing product that the employee wants to update.

            if (product == null)
            {
                return NotFound();
            }

            // Stops the update if the requested product does not exist.

            if (!EmployeeAccess.IsValid(username, password))
            {
                ViewBag.Message =
                    "Invalid employee username or password.";

                return View(product);
            }

            // Verifies employee credentials before allowing changes to the product.

            if (!IsValidProduct(
                productName, productCategory, productPrice))
            {
                ViewBag.Message =
                    "Please enter valid product information.";

                return View(product);
            }

            // Checks that the updated product name, category, and price are valid.
            // Invalid information prevents the changes from being saved.

            product.ProductName = productName.Trim();
            product.ProductCategory = productCategory.Trim();
            product.ProductPrice = productPrice;

            // Updates the existing product's name, category, and price.
            // The changes are made directly to the product stored in the shared list.

            TempData["Message"] =
                $"{product.ProductName} updated successfully.";

            return RedirectToAction("Index");

            // Displays a success message and returns the employee to the product page.
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

            // Finds the selected product and displays the deletion confirmation page.
            // No product is deleted until the employee submits the confirmation form.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(
            int productID,
            string username,
            string password)

        // Handles the confirmed deletion request and receives employee credentials.
        // ActionName("Delete") allows this method to respond to the Delete form submission.
        {
            var product = StoreData.Products.FirstOrDefault(p =>
                p.ProductID == productID);

            // Searches the product list for the product the employee wants to delete.

            if (product == null)
            {
                return NotFound();
            }

            // Stops the deletion if the product cannot be found.

            if (!EmployeeAccess.IsValid(username, password))
            {
                ViewBag.Message =
                    "Invalid employee username or password.";

                return View("Delete", product);
            }

            // Verifies employee credentials before allowing the product to be deleted.

            bool hasReservations = StoreData.Inventory.Any(item =>
                item.ProductID == productID &&
                item.ReservedQuantity > 0);

            // Checks whether the product has any reserved inventory at any store.
            // This includes products reserved by customers or employees.

            if (hasReservations)
            {
                TempData["Message"] =
                    "Cannot delete this product while inventory is reserved.";

                return RedirectToAction("Index");
            }

            // Prevents deleting products that currently have reservations.
            // This protects items that have already been set aside.

            StoreData.Inventory.RemoveAll(item =>
                item.ProductID == productID);

            // Removes all inventory records associated with the product from every store.
            // This prevents leftover inventory records from referencing a deleted product.

            StoreData.Products.Remove(product);

            // Removes the product itself from the application's product list.

            TempData["Message"] =
                $"{product.ProductName} deleted successfully.";

            return RedirectToAction("Index");

            // Displays a success message and returns the employee to the product page.
        }
            private static bool IsValidProduct(
                string? productName,
                string? productCategory,
                decimal productPrice)

        // This reusable method checks whether product information is valid.
        // It is used by both the Create and Edit methods.
        {
            return !string.IsNullOrWhiteSpace(productName)
                && !string.IsNullOrWhiteSpace(productCategory)
                && productPrice > 0;

            // Requires a product name, a category, and a price greater than zero.
            // Returns true if all three conditions are met and false otherwise.
        }
    }
}
