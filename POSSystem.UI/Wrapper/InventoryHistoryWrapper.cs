using POS.Model;
using System;

namespace POSSystem.UI.Wrapper
{
    public class InventoryHistoryWrapper : WrapperBase <InventoryHistory>
    {

        public InventoryHistoryWrapper(InventoryHistory inventoryHistory) : base(inventoryHistory)
        {

        }

        public int Quantity
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public decimal PurchaseRate
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public decimal RetailRate
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public DateTime PurchaseDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }


        public Int64 InventoryId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        private string _heading;

        public string Heading
        {
            get { return _heading; }
            set { _heading = value; OnPropertyChanged(); }
        }

    }
}
