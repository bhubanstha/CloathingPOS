using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class SalesBO
    {
        private IGenericDataRepository<Sales> genericDataRepository;

        public SalesBO()
        {
            genericDataRepository = new DataRepository<Sales>(new POSDataContext());
        }

        public Sales ManageCartItem(Sales item, ObservableCollection<Sales> cart, ref Bill b, out bool addItemToCart)
        {
            Sales _existingItem = cart.Where(x => x.ProductId == item.Inventory.Id).FirstOrDefault();
            if (_existingItem == null)
            {
                item.Rate = item.Inventory.RetailRate;
                item.ProductId = item.Inventory.Id;
                item.Bill = b;
                addItemToCart = true;
                return item;
            }
            else
            {
                _existingItem.SalesQuantity = item.SalesQuantity;
                _existingItem.Discount = item.Discount;
                addItemToCart = false;
                return _existingItem;
            }
        }
    }
}
