using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.BusinessRule
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
    }
}
