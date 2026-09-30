using Jared_s_Graduation_Project;
using System.Runtime.InteropServices;
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

var product1 = new Product
{
    ProductID = 1,
    ProductName = "AMD Ryzen 7 5700X",
    ProductCategory = "CPU",
    ProductPrice = 179.99m
};

var product2 = new Product
{
    ProductID = 2,
    ProductName = "Cooler Master Hyper 212",
    ProductCategory = "CPU Cooler",
    ProductPrice = 39.99m
};

var product3 = new Product
{
    ProductID = 3,
    ProductName = "MSI B550 Tomahawk",
    ProductCategory = "Motherboard",
    ProductPrice = 159.99m
};

var product4 = new Product
{
    ProductID = 4,
    ProductName = "Corsair Vengeance 32GB DDR4",
    ProductCategory = "RAM",
    ProductPrice = 74.99m
};

var product5 = new Product
{
    ProductID = 5,
    ProductName = "NVIDIA GeForce RTX 5060",
    ProductCategory = "GPU",
    ProductPrice = 299.99m
};

var product6 = new Product
{
    ProductID = 6,
    ProductName = "Samsung 990 EVO 1TB NVMe SSD",
    ProductCategory = "Storage",
    ProductPrice = 89.99m
};

var product7 = new Product
{
    ProductID = 7,
    ProductName = "Corsair RM750e 750W",
    ProductCategory = "Power Supply",
    ProductPrice = 109.99m
};

var product8 = new Product
{
    ProductID = 8,
    ProductName = "NZXT H5 Flow",
    ProductCategory = "Case",
    ProductPrice = 94.99m
};

var products = new List<Product>
            {
                product1,
                product2,
                product3,
                product4,
                product5,
                product6,
                product7,
                product8
            };

var inventory = new List<Inventory>
            {   
            // Houston - Store 1
                new Inventory { InventoryID = "A1", StoreID = 1, ProductID = 1, Quantity = 5 },
                new Inventory { InventoryID = "A2", StoreID = 1, ProductID = 2, Quantity = 8 },
                new Inventory { InventoryID = "A3", StoreID = 1, ProductID = 3, Quantity = 4 },
                new Inventory { InventoryID = "A4", StoreID = 1, ProductID = 4, Quantity = 10 },
                new Inventory { InventoryID = "A5", StoreID = 1, ProductID = 5, Quantity = 3 },
                new Inventory { InventoryID = "A6", StoreID = 1, ProductID = 6, Quantity = 12 },
                new Inventory { InventoryID = "A7", StoreID = 1, ProductID = 7, Quantity = 6 },
                new Inventory { InventoryID = "A8", StoreID = 1, ProductID = 8, Quantity = 4 },

            // Dallas - Store 2
                new Inventory { InventoryID = "B1", StoreID = 2, ProductID = 1, Quantity = 3 },
                new Inventory { InventoryID = "B2", StoreID = 2, ProductID = 2, Quantity = 6 },
                new Inventory { InventoryID = "B3", StoreID = 2, ProductID = 3, Quantity = 2 },
                new Inventory { InventoryID = "B4", StoreID = 2, ProductID = 4, Quantity = 7 },
                new Inventory { InventoryID = "B5", StoreID = 2, ProductID = 5, Quantity = 1 },
                new Inventory { InventoryID = "B6", StoreID = 2, ProductID = 6, Quantity = 9 },
                new Inventory { InventoryID = "B7", StoreID = 2, ProductID = 7, Quantity = 5 },
                new Inventory { InventoryID = "B8", StoreID = 2, ProductID = 8, Quantity = 3 },

            // San Antonio - Store 3
                new Inventory { InventoryID = "C1", StoreID = 3, ProductID = 1, Quantity = 4 },
                new Inventory { InventoryID = "C2", StoreID = 3, ProductID = 2, Quantity = 5 },
                new Inventory { InventoryID = "C3", StoreID = 3, ProductID = 3, Quantity = 3 },
                new Inventory { InventoryID = "C4", StoreID = 3, ProductID = 4, Quantity = 8 },
                new Inventory { InventoryID = "C5", StoreID = 3, ProductID = 5, Quantity = 2 },
                new Inventory { InventoryID = "C6", StoreID = 3, ProductID = 6, Quantity = 10 },
                new Inventory { InventoryID = "C7", StoreID = 3, ProductID = 7, Quantity = 4 },
                new Inventory { InventoryID = "C8", StoreID = 3, ProductID = 8, Quantity = 2 }
            };

//Testing location 

var cart = new List<CartItem>();

AddToCart(cart, inventory, 1, 5, 2);

DisplayOrderSummary(cart, products);

Checkout(cart, inventory, 1);

MoveInventory(inventory, 1, 2, 1, 2);

DisplayInventory(inventory, products, 1);

MoveItemToReserve(inventory, 1, 1, 2);

ReturnItemFromReserve(inventory, 1, 1, 2);

CreateProduct(products, 9, "New Product", "Category", 49.99m);

//End of testing location 

//Message and variable Center. 

var reservedCPU = inventory.FirstOrDefault(item => item.StoreID == 1 && item.ProductID == 1);

Console.WriteLine($"Available CPUs after return: {reservedCPU.Quantity}");
Console.WriteLine($"Reserved CPUs after return: {reservedCPU.ReservedQuantity}");

Console.WriteLine($"Available CPUs: {reservedCPU.Quantity}");
Console.WriteLine($"Reserved CPUs: {reservedCPU.ReservedQuantity}");

var houstonCPU = inventory.FirstOrDefault(item => item.StoreID == 1 && item.ProductID == 1);
var dallasCPU = inventory.FirstOrDefault(item => item.StoreID == 2 && item.ProductID == 1);

Console.WriteLine($"Houston CPUs: {houstonCPU.Quantity}");
Console.WriteLine($"Dallas CPUs: {dallasCPU.Quantity}");

var gpuInventory = inventory.FirstOrDefault(item => item.StoreID == 1 && item.ProductID == 5);

Console.WriteLine($"GPUs remaining in Houston: {gpuInventory.Quantity}");

var createdProduct = products.FirstOrDefault(p => p.ProductID == 9);
Console.WriteLine($"Created Product: {createdProduct.ProductName}");

//End of Message and variable center.

//Method Center (Methods may also include messages). 

static void AddToCart(List<CartItem> cart, List<Inventory> inventory, int storeID, int productID, int quantity)
{
    var inventoryItem = inventory.FirstOrDefault(item => item.StoreID == storeID && item.ProductID == productID);

    if (inventoryItem == null)
    {
        Console.WriteLine("Product is not available at this store.");
        return;
    }

    var cartItem = cart.FirstOrDefault(item => item.ProductID == productID);

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
        cart.Add(new CartItem { ProductID = productID, Quantity = quantity });
    }

}

static void Checkout(List<CartItem> cart, List<Inventory> inventory, int storeID)
{
    foreach (var cartItem in cart)
    {
        var inventoryItem = inventory.FirstOrDefault(item => item.StoreID == storeID && item.ProductID == cartItem.ProductID);
        if (inventoryItem != null)
        {
            inventoryItem.Quantity -= cartItem.Quantity;
        }

    }

    cart.Clear();
    Console.WriteLine("Checkout complete. Thank you for your purchase.");

}

static void DisplayOrderSummary(List<CartItem> cartItems, List<Product> products)
{
    Console.WriteLine("Order Summary:");
    decimal total = 0;
    foreach (var cartItem in cartItems)
    {
        var product = products.FirstOrDefault(p => p.ProductID == cartItem.ProductID);
        if (product != null)
        {
            decimal itemTotal = product.ProductPrice * cartItem.Quantity;
            total += itemTotal;
            Console.WriteLine($"{product.ProductName} - Quantity: {cartItem.Quantity}, Price: {product.ProductPrice:C}, Total: {itemTotal:C}");
        }
    }
    Console.WriteLine($"Total Order Cost: {total:C}");
}

static void MoveInventory(List<Inventory> inventory, int fromStoreID, int toStoreID, int productID, int quantity)
{
    var fromInventoryItem = inventory.FirstOrDefault(item => item.StoreID == fromStoreID && item.ProductID == productID);
    var toInventoryItem = inventory.FirstOrDefault(item => item.StoreID == toStoreID && item.ProductID == productID);
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
    Console.WriteLine($"Moved {quantity} of Product ID {productID} from Store {fromStoreID} to Store {toStoreID}.");
}

static void DisplayInventory(List<Inventory> inventory, List<Product> products, int storeID)
{
    Console.WriteLine($"Inventory for Store ID {storeID}:");
    foreach (var inventoryItem in inventory.Where(item => item.StoreID == storeID))
    {
        var product = products.FirstOrDefault(p => p.ProductID == inventoryItem.ProductID);
        if (product != null)
        {
            Console.WriteLine($"{product.ProductName} - Quantity: {inventoryItem.Quantity}");
        }
    }
}

static void MoveItemToReserve(List<Inventory> inventory, int storeID, int productID, int quantity)
{
    var inventoryItem = inventory.FirstOrDefault(item => item.StoreID == storeID && item.ProductID == productID);
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
    Console.WriteLine($"Moved {quantity} of Product ID {productID} to reserve in Store {storeID}.");
}

static void ReturnItemFromReserve(List<Inventory> inventory, int storeID, int productID, int quantity)
{
    var inventoryItem = inventory.FirstOrDefault(item => item.StoreID == storeID && item.ProductID == productID);

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

    static void AddProductToInventory(List<Inventory> inventory, int storeID, int productID, int quantity)
    { 
        var inventoryItem = inventory.FirstOrDefault(item => item.StoreID == storeID && item.ProductID == productID);
        if (inventoryItem == null)
        {
            inventory.Add(new Inventory { StoreID = storeID, ProductID = productID, Quantity = quantity, ReservedQuantity = 0 });
        }
        else
        {
            inventoryItem.Quantity += quantity;
        }
    }

    static void CreateProduct(List<Product> products, int productID, string productName, string productCategory, decimal productPrice)
    {
        var existingProduct = products.FirstOrDefault(p => p.ProductID == productID);

        if (existingProduct != null)
        {
            Console.WriteLine("Please choose another product ID, the suggested ID is taken.");
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
    //static void UpdateProduct(List<Inventory> inventory, int storeID, int productID, int quantity)
    //{ 
    //    var inventoryItem = inventory.FirstOrDefault(item => item.StoreID == storeID && item.ProductID == productID);
    //    if (inventoryItem != null)
    //    {
    //        inventoryItem.Quantity = quantity;
    //    }

    //    else
    //    {

    //    }

    //}

    //End of Method Center 

