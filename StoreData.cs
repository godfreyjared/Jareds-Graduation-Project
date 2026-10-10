
namespace Jared_s_Graduation_Project
{
    public static class StoreData
    {
        public static readonly List<Product> Products =
        [
            new()
            {
                ProductID = 1,
                ProductName = "AMD Ryzen 7 5700X",
                ProductCategory = "CPU",
                ProductPrice = 179.99m
            },

            new()
            {
                ProductID = 2,
                ProductName = "Cooler Master Hyper 212",
                ProductCategory = "CPU Cooler",
                ProductPrice = 39.99m
            },

            new()
            {
                ProductID = 3,
                ProductName = "MSI B550 Tomahawk",
                ProductCategory = "Motherboard",
                ProductPrice = 159.99m
            },

            new()
            {
                ProductID = 4,
                ProductName = "Corsair Vengeance 32GB DDR4",
                ProductCategory = "RAM",
                ProductPrice = 74.99m
            },

            new()
            {
                ProductID = 5,
                ProductName = "NVIDIA GeForce RTX 5060",
                ProductCategory = "GPU",
                ProductPrice = 299.99m
            },

            new()
            {
                ProductID = 6,
                ProductName = "Samsung 990 EVO 1TB NVMe SSD",
                ProductCategory = "Storage",
                ProductPrice = 89.99m
            },

            new()
            {
                ProductID = 7,
                ProductName = "Corsair RM750e 750W",
                ProductCategory = "Power Supply",
                ProductPrice = 109.99m
            },

            new()
            {
                ProductID = 8,
                ProductName = "NZXT H5 Flow",
                ProductCategory = "Case",
                ProductPrice = 94.99m
            }
        ];

        public static readonly List<Inventory> Inventory =
        [
            // Houston - Store 1
            new() { InventoryID = "A1", StoreID = 1, ProductID = 1, Quantity = 5 },
            new() { InventoryID = "A2", StoreID = 1, ProductID = 2, Quantity = 8 },
            new() { InventoryID = "A3", StoreID = 1, ProductID = 3, Quantity = 4 },
            new() { InventoryID = "A4", StoreID = 1, ProductID = 4, Quantity = 10 },
            new() { InventoryID = "A5", StoreID = 1, ProductID = 5, Quantity = 3 },
            new() { InventoryID = "A6", StoreID = 1, ProductID = 6, Quantity = 12 },
            new() { InventoryID = "A7", StoreID = 1, ProductID = 7, Quantity = 6 },
            new() { InventoryID = "A8", StoreID = 1, ProductID = 8, Quantity = 4 },

            // Dallas - Store 2
            new() { InventoryID = "B1", StoreID = 2, ProductID = 1, Quantity = 3 },
            new() { InventoryID = "B2", StoreID = 2, ProductID = 2, Quantity = 6 },
            new() { InventoryID = "B3", StoreID = 2, ProductID = 3, Quantity = 2 },
            new() { InventoryID = "B4", StoreID = 2, ProductID = 4, Quantity = 7 },
            new() { InventoryID = "B5", StoreID = 2, ProductID = 5, Quantity = 1 },
            new() { InventoryID = "B6", StoreID = 2, ProductID = 6, Quantity = 9 },
            new() { InventoryID = "B7", StoreID = 2, ProductID = 7, Quantity = 5 },
            new() { InventoryID = "B8", StoreID = 2, ProductID = 8, Quantity = 3 },

            // San Antonio - Store 3
            new() { InventoryID = "C1", StoreID = 3, ProductID = 1, Quantity = 4 },
            new() { InventoryID = "C2", StoreID = 3, ProductID = 2, Quantity = 5 },
            new() { InventoryID = "C3", StoreID = 3, ProductID = 3, Quantity = 3 },
            new() { InventoryID = "C4", StoreID = 3, ProductID = 4, Quantity = 8 },
            new() { InventoryID = "C5", StoreID = 3, ProductID = 5, Quantity = 2 },
            new() { InventoryID = "C6", StoreID = 3, ProductID = 6, Quantity = 10 },
            new() { InventoryID = "C7", StoreID = 3, ProductID = 7, Quantity = 4 },
            new() { InventoryID = "C8", StoreID = 3, ProductID = 8, Quantity = 2 }
        ];
    }
}
