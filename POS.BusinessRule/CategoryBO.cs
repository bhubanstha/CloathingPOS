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
    public class CategoryBO
    {
        private IGenericDataRepository<Category> genericDataRepository;

        public CategoryBO()
        {
            genericDataRepository = new DataRepository<Category>(new POSDataContext());
        }


        public List<Category> GetCategories()
        {
            return genericDataRepository.GetAll().ToList();
        }

        public Category GetCategory(int id)
        {
            return genericDataRepository.GetByID(id);
        }

        public async Task<int> Save(Category category)
        {
            genericDataRepository.Insert(category);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> Update(Category obj)
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
