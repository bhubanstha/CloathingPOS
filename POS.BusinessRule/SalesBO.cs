using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POS.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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


        public DbContextTransaction BeginTransation()
        {
            return genericDataRepository.BeginTransaction();
        }
        public Sales ManageCartItem(SalesModel item, ObservableCollection<SalesModel> cart, ref Bill b)
        {
            return null;
            //Sales _existingItem = cart.Where(x => x.ProductId == item.Inventory.Id).FirstOrDefault();
            //if (_existingItem != null)
            //{
            //    cart.Remove(_existingItem);
            //}

            //item.Rate = item.Inventory.RetailRate;
            //item.ProductId = item.Inventory.Id;
            //item.Bill = b;
            //return item;
        }

        public async Task<int> CheckoutSales(Sales item)
        {
            genericDataRepository.Insert(item);
            InventoryBO inventoryBO = new InventoryBO();
            inventoryBO.DeductQuantity(item.ProductId, item.SalesQuantity);
            return await genericDataRepository.SaveAsync();
        }
        public async Task<int> Update(Sales item)
        {
            genericDataRepository.Update(item);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> Remove(Sales item)
        {
            genericDataRepository.Delete(item.Id);
            List<Sales> remainingItems = genericDataRepository.GetAll().Where(x => x.BillNo == item.BillNo && x.Id != item.Id).ToList<Sales>();
            int count = await genericDataRepository.SaveAsync();
            if (count > 0 && (remainingItems == null || remainingItems.Count == 0))
            {
                BillBO billBO = new BillBO();
                await billBO.Remove(item.BillNo);
            }
            return count;
        }

        public async Task<List<Sales>> GetSalesByBillNo(Int64 BillNo)
        {
            List<Sales> sales = await genericDataRepository.GetAll().Where(x => x.BillNo == BillNo).ToListAsync<Sales>();
            return sales;
        }

        public List<Sales> GetSalesHistory(Int64 itemId)
        {
            List<Sales> sales = genericDataRepository.GetAll().Where(x => x.Inventory.Id == itemId).OrderBy(x=>x.Bill.BillDate).ToList();
            return sales;
        }

    }
}
