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

    }
}
