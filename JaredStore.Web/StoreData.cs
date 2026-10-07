using Jared_s_Graduation_Project;

namespace JaredStore.Web.Models
{
    public static class StoreData
    {
        public static List<Inventory> Inventory = new List<Inventory>
        {
            new Inventory { InventoryID = "A1", StoreID = 1, ProductID = 1, Quantity = 5 },
            new Inventory { InventoryID = "A2", StoreID = 1, ProductID = 2, Quantity = 8 },
            new Inventory { InventoryID = "A3", StoreID = 1, ProductID = 3, Quantity = 4 },
            new Inventory { InventoryID = "A4", StoreID = 1, ProductID = 4, Quantity = 10 },
            new Inventory { InventoryID = "A5", StoreID = 1, ProductID = 5, Quantity = 3 },
            new Inventory { InventoryID = "A6", StoreID = 1, ProductID = 6, Quantity = 12 },
            new Inventory { InventoryID = "A7", StoreID = 1, ProductID = 7, Quantity = 6 },
            new Inventory { InventoryID = "A8", StoreID = 1, ProductID = 8, Quantity = 4 }
        };
    }
}