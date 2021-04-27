using POS.Data;
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



    }
}
