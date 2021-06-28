using POS.Core.Model;

namespace POSSystem.WPF.UI.UIModel
{
    public class InventoryChangedEventArgs
    {
        public Inventory Inventory { get; set; }
        public EventAction Action { get; set; }
    }
}
