using POS.Model;

namespace POSSystem.UI.UIModel
{
    public class BrandChangedEventArgs
    {
        public Brand Brand { get; set; }
        public EventAction Action { get; set; }
    }

    public class BranchChangedEventArgs
    {
        public Branch Branch { get; set; }
        public EventAction Action { get; set; }
    }


}
