using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POS.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BusinessRule.EF
{
    public class ShopBO
    {
        private IGenericDataRepository<Shop> genericDataRepository;

        public ShopBO()
        {
            genericDataRepository = new DataRepository<Shop>(new POSDataContext());
        }

        public Shop GetShop()
        {
            return genericDataRepository.GetAll().FirstOrDefault();
        }

        public async Task<int> SaveShop(Shop shop)
        {
            genericDataRepository.Insert(shop);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> UpdateShop(Shop shop)
        {
            genericDataRepository.Update(shop);
            return await genericDataRepository.SaveAsync();
        }
    }
}
