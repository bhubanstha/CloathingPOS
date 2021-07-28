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
    public class BranchBO
    {
        private IGenericDataRepository<Branch> genericDataRepository;

        public BranchBO()
        {
            genericDataRepository = new DataRepository<Branch>(new Data.POSDataContext());
        }


        public List<Branch> GetAll()
        {
            return genericDataRepository.GetAll().ToList();
        }

        public List<Branch> GetAll(Int64 branchId, bool canAccessAllBranch)
        {
            return genericDataRepository
                .GetAll()
                .Where(x=>x.Id == branchId || canAccessAllBranch == true)
                .ToList();
        }

        public Branch GetById(Int64 id)
        {
            return genericDataRepository.GetByID(id);
        }

        public async Task<int> Save(Branch branch)
        {
            genericDataRepository.Insert(branch);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> Update(Branch branch)
        {
            genericDataRepository.Update(branch);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> Delete(Int64 Id)
        {
            genericDataRepository.Delete(Id);
            return await genericDataRepository.SaveAsync();
        }

    }
}
