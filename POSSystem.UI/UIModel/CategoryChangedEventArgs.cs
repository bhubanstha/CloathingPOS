using POS.Model;
using System;

namespace POSSystem.UI.UIModel
{
    public class CategoryChangedEventArgs
    {
        public Category Category { get; set; }
        public EventAction Action { get; set; }
    }

    public class BillingInfoUpdateEventArgs
    {
        public Int64 BillId { get; set; }
        public EventAction Action { get; set; }
    }
}
