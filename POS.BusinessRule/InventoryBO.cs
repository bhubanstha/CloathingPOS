using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class InventoryBO
    {

        private IGenericDataRepository<Inventory> genericDataRepository;

        public InventoryBO()
        {
            genericDataRepository = new DataRepository<Inventory>(new POSDataContext());
        }

        public async Task<int> Save (Inventory inventory)
        {
            genericDataRepository.Insert(inventory);
            int i =  await genericDataRepository.SaveAsync();
            if(i>0)
            {
                InventoryHistoryBO inventoryHistory = new InventoryHistoryBO();
                inventoryHistory.AddToHistory(inventory);

            }
            return i;
        }

        public List<Inventory> GetAllActiveProducts(Int64 branchId)
        {
            return genericDataRepository
                .GetAll()
                .Where(x=> x.IsDeleted == false && x.Quantity>0 && x.BranchId == branchId)
                .ToList();
        }

        public List<Inventory> GetAllActiveProducts()
        {
            return genericDataRepository
                .GetAll()
                .Where(x => x.IsDeleted == false && x.Quantity > 0 )
                .ToList();
        }


        public List<Inventory> GetAllActiveProducts(string productName, Int64 branchId)
        {
            return genericDataRepository
                .GetAll()
                .Where(x => x.IsDeleted == false && x.Quantity > 0 && x.BranchId == branchId  && x.Name.ToLower().StartsWith(productName))
                .ToList();
        }

        public Inventory GetById(Int64 id)
        {
            return genericDataRepository.GetByID(id);
        }

        public async void RemoveItem(Inventory item)
        {
            item.IsDeleted = true;
            genericDataRepository.Update(item);

            await genericDataRepository.SaveAsync();

            //Inventory itm = GetById(item.Id);
            //if(itm != null)
            //{
            //    itm.IsDeleted = true;

               
            //}
            
        }

        public void DeductQuantity(Int64 productId, int deductionQty)
        {
            Inventory itm = genericDataRepository.GetByID(productId);
            if(itm!= null)
            {
                itm.Quantity -= deductionQty;
                genericDataRepository.Update(itm);

                genericDataRepository.Save();
            }
        }

        public async Task<int> UpdateInventory(Inventory inventory, InventoryHistory history)
        {
            genericDataRepository.Update(inventory);
            InventoryHistoryBO inventoryHistoryBO = new InventoryHistoryBO();
            inventoryHistoryBO.AddToHistory(history);
            return await genericDataRepository.SaveAsync();
        }
        public async Task<int> UpdateInventory(Inventory inventory)
        {
            genericDataRepository.Update(inventory);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> Restock(Inventory inventory, int salesReturn)
        {
            inventory.Quantity += salesReturn;
            genericDataRepository.Update(inventory);
            return await genericDataRepository.SaveAsync();
        }
    }
}
