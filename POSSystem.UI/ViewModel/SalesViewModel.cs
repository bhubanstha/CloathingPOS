using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel
{
    public class SalesViewModel: ViewModelBase
    {
        private string _productName ="";
        private InventoryBO inventoryBO;
        public List<Inventory> Products { get; set; }
        public List<Inventory> FilterProducts { get; set; }
        public CultureInfo CultureInfo { get; set; }

        public string ProductName
        {
            get { return _productName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    FilterProducts = null;
                }
                else
                {
                    FilterProducts = Products.Where(p=> p.Name.Contains(value)).ToList();
                    _productName = value;
                }
            }
        }

        public SalesViewModel()
        {
            CultureInfo = StaticContainer.CultureInfo;
            GetInventory();
        }

        private void GetInventory()
        {
            inventoryBO = new InventoryBO();
            Products = inventoryBO.GetAll();
        }

       
    }
}
