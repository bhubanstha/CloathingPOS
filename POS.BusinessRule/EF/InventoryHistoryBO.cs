using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.BusinessRule.EF
{
    public class InventoryHistoryBO
    {

        private IGenericDataRepository<InventoryHistory> genericDataRepository;
        public InventoryHistoryBO()
        {
            genericDataRepository = new DataRepository<InventoryHistory>(new POSDataContext());
        }

        public List<InventoryHistory> GetHistory(Int64 itemId)
        {
            List<InventoryHistory> records = genericDataRepository.GetAll().Where(x => x.Inventory.Id == itemId).OrderBy(x=>x.PurchaseDate).ToList();
            return records;

        }

        public async void AddToHistory(Inventory inventory)
        {
            InventoryHistory inventoryHistory = new InventoryHistory
            {
                Quantity = inventory.Quantity,
                PurchaseRate = inventory.PurchaseRate,
                RetailRate = inventory.RetailRate,
                PurchaseDate = inventory.FirstPurchaseDate,
                InventoryId = inventory.Id
            };
            genericDataRepository.Insert(inventoryHistory);
            await genericDataRepository.SaveAsync();
        }

        public async void AddToHistory(InventoryHistory history)
        {
            if(history.Inventory != null)
            {
                history.Inventory = null;
            }
            genericDataRepository.Insert(history);
            await genericDataRepository.SaveAsync();
        }


    }
}
