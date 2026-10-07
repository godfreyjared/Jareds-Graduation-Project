namespace Jared_s_Graduation_Project
{
    public static class StoreData
    {
        public static List<Product> Products = new List<Product>
        {
            new Product
            {
                ProductID = 1,
                ProductName = "AMD Ryzen 7 5700X",
                ProductCategory = "CPU",
                ProductPrice = 179.99m
            },

            new Product
            {
                ProductID = 2,
                ProductName = "Cooler Master Hyper 212",
                ProductCategory = "CPU Cooler",
                ProductPrice = 39.99m
            },

            new Product
            {
                ProductID = 3,
                ProductName = "MSI B550 Tomahawk",
                ProductCategory = "Motherboard",
                ProductPrice = 159.99m
            },

            new Product
            {
                ProductID = 4,
                ProductName = "Corsair Vengeance 32GB DDR4",
                ProductCategory = "RAM",
                ProductPrice = 74.99m
            },

            new Product
            {
                ProductID = 5,
                ProductName = "NVIDIA GeForce RTX 5060",
                ProductCategory = "GPU",
                ProductPrice = 299.99m
            },

            new Product
            {
                ProductID = 6,
                ProductName = "Samsung 990 EVO 1TB NVMe SSD",
                ProductCategory = "Storage",
                ProductPrice = 89.99m
            },

            new Product
            {
                ProductID = 7,
                ProductName = "Corsair RM750e 750W",
                ProductCategory = "Power Supply",
                ProductPrice = 109.99m
            },

            new Product
            {
                ProductID = 8,
                ProductName = "NZXT H5 Flow",
                ProductCategory = "Case",
                ProductPrice = 94.99m
            }
        };

        public static List<Inventory> Inventory = new List<Inventory>
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
    }
}