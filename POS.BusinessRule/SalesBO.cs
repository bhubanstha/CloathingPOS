﻿using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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


        public DbContextTransaction BeginTransation()
        {
            return genericDataRepository.BeginTransaction();
        }
        public Sales ManageCartItem(Sales item, ObservableCollection<Sales> cart, ref Bill b)
        {
            Sales _existingItem = cart.Where(x => x.ProductId == item.Inventory.Id).FirstOrDefault();
            if (_existingItem != null)
            {
                cart.Remove(_existingItem);
            }

            item.Rate = item.Inventory.RetailRate;
            item.ProductId = item.Inventory.Id;
            item.Bill = b;
            return item;
        }

        public async Task<int> CheckoutSales(List<Sales> items, Bill bill)
        {
           
            foreach (Sales item in items)
            {
                InventoryBO inventoryBO = new InventoryBO();
                item.BillNo = bill.Id;
                item.ProductId = item.Inventory.Id;

                item.Bill = null;
                item.Inventory = null;

                genericDataRepository.Insert(item);
                inventoryBO.DeductQuantity(item.ProductId, item.SalesQuantity);
            }

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
    }
}
