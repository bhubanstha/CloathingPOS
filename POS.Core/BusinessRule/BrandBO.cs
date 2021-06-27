using POS.Core.Model;
using POS.Core.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Core.BusinessRule
{
    public class BrandBO
    {
        private IGenericDataRepository<Brand> genericDataRepository;

        public BrandBO()
        {
            genericDataRepository = new DataRepository<Brand>(new POSDataContext());
        }


        public List<Brand> GetBrands()
        {
            return genericDataRepository.GetAll().ToList();
        }

        public Brand GetBrand(Int64 id)
        {
            return genericDataRepository.GetByID(id);
        }

        public async Task<int> Save(Brand brand)
        {
            genericDataRepository.Insert(brand);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> Update(Brand obj)
        {
            genericDataRepository.Update(obj);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> Delete(Int64 id)
        {
            genericDataRepository.Delete(id);
            return  await genericDataRepository.SaveAsync();
        }

    }
}
