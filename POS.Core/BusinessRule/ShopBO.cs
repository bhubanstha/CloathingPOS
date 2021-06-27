using POS.Core.Model;
using POS.Core.Repo;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Core.BusinessRule
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
