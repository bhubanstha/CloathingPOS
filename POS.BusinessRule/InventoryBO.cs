﻿using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await genericDataRepository.SaveAsync();
        }

        public List<Inventory> GetAllActiveProducts()
        {
            return genericDataRepository.GetAll().Where(x=> x.IsDeleted == false).ToList();
        }

        public Inventory GetById(int id)
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

    }
}