
namespace Jared_s_Graduation_Project
{
    public class Inventory
    {
        public string InventoryID { get; set; } = "";
        public int StoreID { get; set; }
        public int ProductID { get; set; }

        // Stock available for customers or employees
        public int Quantity { get; set; }

        // All reserved stock, including customer carts
        public int ReservedQuantity { get; set; }

        // Portion of reserved stock held by employees
        public int EmployeeReservedQuantity { get; set; }
    }
}
