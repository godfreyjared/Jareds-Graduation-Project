using Jared_s_Graduation_Project;

//3 Mock store locations 
// Add to cart function 
// Checkout function
// Display order summary function
// Move inventory between stores function
// Display inventory function
// Autorefresh to display inventory function
// CRUD functions for products and stores
// Take off site to keep in store inventory function

var store1 = new Store
{
    StoreName = "Continental Computing Solutions HQ",
    StoreLocation = "Houston",
    StoreID = 1
};

var store2 = new Store
{
    StoreName = "Continental Computing Solutions in Dallas",
    StoreLocation = "Dallas",
    StoreID = 2
};

var store3 = new Store
{
    StoreName = "Continental Computing Solutions in San Antonio",
    StoreLocation = "San Antonio",
    StoreID = 3
};


// ONE central product list
var products = StoreData.Products;

// ONE central inventory list
var inventory = StoreData.Inventory;


bool runTests = false;

if (runTests)
{

    // Testing location

    AddProductToInventory(inventory, 1, 5, 1);

    var cart = new List<CartItem>();

    AddToCart(cart, inventory, 1, 5, 2);

    DisplayOrderSummary(cart, products);

    Checkout(cart, inventory, 1);

    MoveInventory(inventory, 1, 2, 1, 2);

    DisplayInventory(inventory, products, 1);

    MoveItemToReserve(inventory, 1, 1, 2);

    ReturnItemFromReserve(inventory, 1, 1, 2);

    CreateProduct(products, 9, "New Product", "Category", 49.99m);

    UpdateProduct(products, 9, "Updated Product", "Updated Category", 59.99m);

    DisplayProduct(products, 9);

    // End of testing location

// Message and variable Center.

var updatedProduct = products.FirstOrDefault(p => p.ProductID == 9);

if (updatedProduct != null)
{
    Console.WriteLine($"Updated Name: {updatedProduct.ProductName}");
    Console.WriteLine($"Updated Category: {updatedProduct.ProductCategory}");
    Console.WriteLine($"Updated Price: {updatedProduct.ProductPrice:C}");
}

var reservedCPU = inventory.FirstOrDefault(item =>
    item.StoreID == 1 &&
    item.ProductID == 1);

if (reservedCPU != null)
{
    Console.WriteLine($"Available CPUs after return: {reservedCPU.Quantity}");
    Console.WriteLine($"Reserved CPUs after return: {reservedCPU.ReservedQuantity}");

    Console.WriteLine($"Available CPUs: {reservedCPU.Quantity}");
    Console.WriteLine($"Reserved CPUs: {reservedCPU.ReservedQuantity}");
}

var houstonCPU = inventory.FirstOrDefault(item =>
    item.StoreID == 1 &&
    item.ProductID == 1);

var dallasCPU = inventory.FirstOrDefault(item =>
    item.StoreID == 2 &&
    item.ProductID == 1);

if (houstonCPU != null)
{
    Console.WriteLine($"Houston CPUs: {houstonCPU.Quantity}");
}

if (dallasCPU != null)
{
    Console.WriteLine($"Dallas CPUs: {dallasCPU.Quantity}");
}

var gpuInventory = inventory.FirstOrDefault(item =>
    item.StoreID == 1 &&
    item.ProductID == 5);

if (gpuInventory != null)
{
    Console.WriteLine($"GPUs remaining in Houston: {gpuInventory.Quantity}");
}

var createdProduct = products.FirstOrDefault(p =>
    p.ProductID == 9);

if (createdProduct != null)
{
    Console.WriteLine($"Created Product: {createdProduct.ProductName}");
}

// End of Message and variable center.


// Delete test is isolated here to avoid accidental deletion of products
// used during runtime. We want the product to delete after everything
// needed has been used for display testing.


    DeleteProduct(products, 9);

}
// Method Center

static void AddToCart(
    List<CartItem> cart,
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
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

    int quantityAlreadyInCart = 0;

    if (cartItem != null)
    {
        quantityAlreadyInCart = cartItem.Quantity;
    }

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
}


static void Checkout(
    List<CartItem> cart,
    List<Inventory> inventory,
    int storeID)
{
    foreach (var cartItem in cart)
    {
        var inventoryItem = inventory.FirstOrDefault(item =>
            item.StoreID == storeID &&
            item.ProductID == cartItem.ProductID);

        if (inventoryItem != null)
        {
            inventoryItem.Quantity -= cartItem.Quantity;
        }
    }

    cart.Clear();

    Console.WriteLine("Checkout complete. Thank you for your purchase.");
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

        if (product != null)
        {
            decimal itemTotal =
                product.ProductPrice * cartItem.Quantity;

            total += itemTotal;

            Console.WriteLine(
                $"{product.ProductName} - Quantity: {cartItem.Quantity}, Price: {product.ProductPrice:C}, Total: {itemTotal:C}");
        }
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
    var fromInventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == fromStoreID &&
        item.ProductID == productID);

    var toInventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == toStoreID &&
        item.ProductID == productID);

    if (fromInventoryItem == null || toInventoryItem == null)
    {
        Console.WriteLine("Product is not available in one of the stores.");
        return;
    }

    if (fromInventoryItem.Quantity < quantity)
    {
        Console.WriteLine("Not enough inventory available to move.");
        return;
    }

    fromInventoryItem.Quantity -= quantity;
    toInventoryItem.Quantity += quantity;

    Console.WriteLine(
        $"Moved {quantity} of Product ID {productID} from Store {fromStoreID} to Store {toStoreID}.");
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
                $"{product.ProductName} - Quantity: {inventoryItem.Quantity}");
        }
    }
}


static void MoveItemToReserve(
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
    var inventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == storeID &&
        item.ProductID == productID);

    if (inventoryItem == null)
    {
        Console.WriteLine("Product is not available in the store.");
        return;
    }

    if (inventoryItem.Quantity < quantity)
    {
        Console.WriteLine("Not enough inventory available to move to reserve.");
        return;
    }

    inventoryItem.Quantity -= quantity;
    inventoryItem.ReservedQuantity += quantity;

    Console.WriteLine(
        $"Moved {quantity} of Product ID {productID} to reserve in Store {storeID}.");
}


static void ReturnItemFromReserve(
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
    var inventoryItem = inventory.FirstOrDefault(item =>
        item.StoreID == storeID &&
        item.ProductID == productID);

    if (inventoryItem == null)
    {
        Console.WriteLine("Product is not available in this store.");
        return;
    }
    else if (inventoryItem.ReservedQuantity < quantity)
    {
        Console.WriteLine("The requested amount exceeds the reserved quantity.");
        return;
    }
    else
    {
        inventoryItem.ReservedQuantity -= quantity;
        inventoryItem.Quantity += quantity;

        Console.WriteLine("Thank you for bringing this back into inventory!");
    }
}


static void AddProductToInventory(
    List<Inventory> inventory,
    int storeID,
    int productID,
    int quantity)
{
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
}


static void CreateProduct(
    List<Product> products,
    int productID,
    string productName,
    string productCategory,
    decimal productPrice)
{
    var existingProduct = products.FirstOrDefault(p =>
        p.ProductID == productID);

    if (existingProduct != null)
    {
        Console.WriteLine(
            "Please choose another product ID, the suggested ID is taken.");

        return;
    }

    var newProduct = new Product
    {
        ProductID = productID,
        ProductName = productName,
        ProductCategory = productCategory,
        ProductPrice = productPrice
    };

    products.Add(newProduct);

    Console.WriteLine($"{productName} was successfully created.");
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

    if (product != null)
    {
        product.ProductName = productName;
        product.ProductCategory = productCategory;
        product.ProductPrice = productPrice;

        Console.WriteLine(
            $"Product ID {productID} updated successfully.");
    }
    else
    {
        Console.WriteLine("Product not found.");
    }
}


static void DisplayProduct(
    List<Product> products,
    int productID)
{
    var product = products.FirstOrDefault(p =>
        p.ProductID == productID);

    if (product != null)
    {
        Console.WriteLine($"Product ID: {product.ProductID}");
        Console.WriteLine($"Product Name: {product.ProductName}");
        Console.WriteLine($"Product Category: {product.ProductCategory}");
        Console.WriteLine($"Product Price: {product.ProductPrice:C}");
    }
    else
    {
        Console.WriteLine("Product not found.");
    }
}


static void DeleteProduct(
    List<Product> products,
    int productID)
{
    var product = products.FirstOrDefault(p =>
        p.ProductID == productID);

    if (product != null)
    {
        products.Remove(product);

        Console.WriteLine(
            $"Product ID {productID} deleted successfully.");
    }
    else
    {
        Console.WriteLine("Product not found.");
    }
}

// End of Method Center