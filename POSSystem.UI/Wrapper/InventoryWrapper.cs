using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Wrapper
{
    public class InventoryWrapper : WrapperBase<Inventory>
    {
        public InventoryWrapper(Inventory obj) : base(obj)
        {
        }

        public Int64 Id
        {
            get { return GetValue<Int64>(); }
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

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Size
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Color
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public Int64 CategoryId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public string CategoryName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
