using POS.Model;

namespace POSSystem.UI.UIModel
{
    public class InventoryChangedEventArgs
    {
        public Inventory Inventory { get; set; }
        public EventAction Action { get; set; }
    }
}
