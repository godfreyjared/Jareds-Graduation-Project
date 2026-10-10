
using Jared_s_Graduation_Project;

// Continental Computing Solutions
// Console demonstration and testing application.
//
// Set runTests to true to run the demonstration.
// Set runTests to false when using the website normally.

bool runTests = false;

if (runTests)
{
    RunDemonstration();
}

void RunDemonstration()
{
    // Use separate copies so tests do not modify StoreData.
    var products = StoreData.Products
        .Select(p => new Product
        {
            ProductID = p.ProductID,
            ProductName = p.ProductName,
            ProductCategory = p.ProductCategory,
            ProductPrice = p.ProductPrice
        })
        .ToList();

    var inventory = StoreData.Inventory
        .Select(i => new Inventory
        {
            InventoryID = i.InventoryID,
            StoreID = i.StoreID,
            ProductID = i.ProductID,
            Quantity = i.Quantity,
            ReservedQuantity = i.ReservedQuantity,
            EmployeeReservedQuantity = i.EmployeeReservedQuantity
        })
        .ToList();

    var cart = new List<CartItem>();

    Console.WriteLine("CONTINENTAL COMPUTING SOLUTIONS");
    Console.WriteLine("Inventory Management Demonstration");
    Console.WriteLine(new string('=', 45));

    ShowSection("1. Display Houston Inventory");
    DisplayInventory(inventory, products, 1);

    ShowSection("2. Add Inventory");
    AddProductToInventory(inventory, 1, 5, 1);
    DisplayInventory(inventory, products, 1);

    ShowSection("3. Add to Cart");
    AddToCart(cart, inventory, 1, 5, 2);
    DisplayOrderSummary(cart, products);

    ShowSection("4. Checkout");
    Checkout(cart, inventory, 1);
    DisplayInventory(inventory, products, 1);

    ShowSection("5. Transfer Inventory");
    MoveInventory(inventory, 1, 2, 1, 2);
    DisplayInventory(inventory, products, 1);
    DisplayInventory(inventory, products, 2);

    ShowSection("6. Reserve Inventory");
    MoveItemToReserve(inventory, 1, 1, 1);
    DisplayInventory(inventory, products, 1);

    ShowSection("7. Return Reserved Inventory");
    ReturnItemFromReserve(inventory, 1, 1, 1);
    DisplayInventory(inventory, products, 1);

    ShowSection("8. Create Product");
    CreateProduct(
        products, 9, "New Product", "Category", 49.99m);
    DisplayProduct(products, 9);

    ShowSection("9. Update Product");
    UpdateProduct(
        products, 9, "Updated Product",
        "Updated Category", 59.99m);
    DisplayProduct(products, 9);

    ShowSection("10. Delete Product");
    DeleteProduct(products, 9);
    DisplayProduct(products, 9);

    ShowSection("Demonstration Complete");
}

static void ShowSection(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('-', 45));
    Console.WriteLine(title);
    Console.WriteLine(new string('-', 45));
}

static void AddToCart(
    List<CartItem> cart,
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
    if (quantity <= 0)
    {
        Console.WriteLine("Quantity must be greater than zero.");
        return;
    }

    var inventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == storeID &&
        item.ProductID == productID);

    if (inventoryItem == null)
    {
        Console.WriteLine("Product is not available at this store.");
        return;
    }

    var cartItem = cart.FirstOrDefault(item =>
        item.ProductID == productID);

    int quantityAlreadyInCart = cartItem?.Quantity ?? 0;

    if (inventoryItem.Quantity < quantityAlreadyInCart + quantity)
    {
        Console.WriteLine("Not enough inventory available.");
        return;
    }

    if (cartItem != null)
    {
        cartItem.Quantity += quantity;
    }
    else
    {
        cart.Add(new CartItem
        {
            ProductID = productID,
            Quantity = quantity
        });
    }

    Console.WriteLine(
        $"Added {quantity} of Product ID {productID} to cart.");
}

static void Checkout(
    List<CartItem> cart,
    List<Inventory> inventory,
    int storeID)
{
    if (cart.Count == 0)
    {
        Console.WriteLine("Your cart is empty.");
        return;
    }

    // Verify all quantities before modifying inventory.
    foreach (var cartItem in cart)
    {
        var inventoryItem = inventory.FirstOrDefault(item =>
            item.StoreID == storeID &&
            item.ProductID == cartItem.ProductID);

        if (inventoryItem == null ||
            inventoryItem.Quantity < cartItem.Quantity)
        {
            Console.WriteLine(
                "Checkout failed: insufficient inventory.");
            return;
        }
    }

    foreach (var cartItem in cart)
    {
        var inventoryItem = inventory.First(item =>
            item.StoreID == storeID &&
            item.ProductID == cartItem.ProductID);

        inventoryItem.Quantity -= cartItem.Quantity;
    }

    cart.Clear();

    Console.WriteLine(
        "Checkout complete. Thank you for your purchase.");
}

static void DisplayOrderSummary(
    List<CartItem> cartItems,
    List<Product> products)
{
    Console.WriteLine("Order Summary:");

    decimal total = 0;

    foreach (var cartItem in cartItems)
    {
        var product = products.FirstOrDefault(p =>
            p.ProductID == cartItem.ProductID);

        if (product == null)
        {
            continue;
        }

        decimal itemTotal =
            product.ProductPrice * cartItem.Quantity;

        total += itemTotal;

        Console.WriteLine(
            $"{product.ProductName} - " +
            $"Quantity: {cartItem.Quantity}, " +
            $"Price: {product.ProductPrice:C}, " +
            $"Total: {itemTotal:C}");
    }

    Console.WriteLine($"Total Order Cost: {total:C}");
}

static void MoveInventory(
    List<Inventory> inventory,
    int fromStoreID,
    int toStoreID,
    int productID,
    int quantity)
{
    if (quantity <= 0 || fromStoreID == toStoreID)
    {
        Console.WriteLine("Invalid transfer request.");
        return;
    }

    var fromInventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == fromStoreID &&
        item.ProductID == productID);

    var toInventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == toStoreID &&
        item.ProductID == productID);

    if (fromInventoryItem == null || toInventoryItem == null)
    {
        Console.WriteLine(
            "Product is not available in one of the stores.");
        return;
    }

    if (fromInventoryItem.Quantity < quantity)
    {
        Console.WriteLine(
            "Not enough inventory available to move.");
        return;
    }

    fromInventoryItem.Quantity -= quantity;
    toInventoryItem.Quantity += quantity;

    Console.WriteLine(
        $"Moved {quantity} of Product ID {productID} " +
        $"from Store {fromStoreID} to Store {toStoreID}.");
}

static void DisplayInventory(
    List<Inventory> inventory,
    List<Product> products,
    int storeID)
{
    Console.WriteLine($"Inventory for Store ID {storeID}:");

    foreach (var inventoryItem in inventory.Where(item =>
        item.StoreID == storeID))
    {
        var product = products.FirstOrDefault(p =>
            p.ProductID == inventoryItem.ProductID);

        if (product != null)
        {
            Console.WriteLine(
                $"{product.ProductName} - " +
                $"Available: {inventoryItem.Quantity}, " +
                $"Reserved: {inventoryItem.ReservedQuantity}");
        }
    }
}

static void MoveItemToReserve(
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
    if (quantity <= 0)
    {
        Console.WriteLine("Quantity must be greater than zero.");
        return;
    }

    var inventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == storeID &&
        item.ProductID == productID);

    if (inventoryItem == null)
    {
        Console.WriteLine(
            "Product is not available in the store.");
        return;
    }

    if (inventoryItem.Quantity < quantity)
    {
        Console.WriteLine(
            "Not enough inventory available to reserve.");
        return;
    }

    inventoryItem.Quantity -= quantity;
    inventoryItem.ReservedQuantity += quantity;

    Console.WriteLine(
        $"Reserved {quantity} of Product ID {productID} " +
        $"in Store {storeID}.");
}

static void ReturnItemFromReserve(
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
    if (quantity <= 0)
    {
        Console.WriteLine("Quantity must be greater than zero.");
        return;
    }

    var inventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == storeID &&
        item.ProductID == productID);

    if (inventoryItem == null)
    {
        Console.WriteLine(
            "Product is not available in this store.");
        return;
    }

    if (inventoryItem.ReservedQuantity < quantity)
    {
        Console.WriteLine(
            "The requested amount exceeds reserved quantity.");
        return;
    }

    inventoryItem.ReservedQuantity -= quantity;
    inventoryItem.Quantity += quantity;

    Console.WriteLine(
        $"Returned {quantity} of Product ID {productID} " +
        "to available inventory.");
}

static void AddProductToInventory(
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
    if (quantity <= 0)
    {
        Console.WriteLine("Quantity must be greater than zero.");
        return;
    }

    var inventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == storeID &&
        item.ProductID == productID);

    if (inventoryItem == null)
    {
        inventory.Add(new Inventory
        {
            StoreID = storeID,
            ProductID = productID,
            Quantity = quantity,
            ReservedQuantity = 0
        });
    }
    else
    {
        inventoryItem.Quantity += quantity;
    }

    Console.WriteLine(
        $"Added {quantity} units of Product ID {productID} " +
        $"to Store {storeID}.");
}

static void CreateProduct(
    List<Product> products,
    int productID,
    string productName,
    string productCategory,
    decimal productPrice)
{
    if (products.Any(p => p.ProductID == productID))
    {
        Console.WriteLine(
            "Please choose another product ID. This ID is taken.");
        return;
    }

    if (productID <= 0 ||
        string.IsNullOrWhiteSpace(productName) ||
        string.IsNullOrWhiteSpace(productCategory) ||
        productPrice < 0)
    {
        Console.WriteLine("Invalid product information.");
        return;
    }

    products.Add(new Product
    {
        ProductID = productID,
        ProductName = productName,
        ProductCategory = productCategory,
        ProductPrice = productPrice
    });

    Console.WriteLine(
        $"{productName} was successfully created.");
}

static void UpdateProduct(
    List<Product> products,
    int productID,
    string productName,
    string productCategory,
    decimal productPrice)
{
    var product = products.FirstOrDefault(p =>
        p.ProductID == productID);

    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    if (string.IsNullOrWhiteSpace(productName) ||
        string.IsNullOrWhiteSpace(productCategory) ||
        productPrice < 0)
    {
        Console.WriteLine("Invalid product information.");
        return;
    }

    product.ProductName = productName;
    product.ProductCategory = productCategory;
    product.ProductPrice = productPrice;

    Console.WriteLine(
        $"Product ID {productID} updated successfully.");
}

static void DisplayProduct(
    List<Product> products,
    int productID)
{
    var product = products.FirstOrDefault(p =>
        p.ProductID == productID);

    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    Console.WriteLine($"Product ID: {product.ProductID}");
    Console.WriteLine($"Product Name: {product.ProductName}");
    Console.WriteLine($"Product Category: {product.ProductCategory}");
    Console.WriteLine($"Product Price: {product.ProductPrice:C}");
}

static void DeleteProduct(
    List<Product> products,
    int productID)
{
    var product = products.FirstOrDefault(p =>
        p.ProductID == productID);

    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    products.Remove(product);

    Console.WriteLine(
        $"Product ID {productID} deleted successfully.");
}
