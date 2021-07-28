using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.BusinessRule.EF
{
    public class BillBO
    {
        private IGenericDataRepository<Bill> genericDataRepository;

        public BillBO()
        {
            genericDataRepository = new DataRepository<Bill>(new POSDataContext());
        }

        public List<Bill> GetAll()
        {
            return genericDataRepository.GetAll().ToList();
        }

        public int GetRemainingSalesCount(Int64 BillNo)
        {
            int remainingRecords =  genericDataRepository.GetAll()
                .Where(x=>x.Id == BillNo)
                .Select(x=>x.Sales)
                .Count();
            return remainingRecords;
        }

        public long GetNewBillNo()
        {
            
            long currentMax = (from bills in genericDataRepository.GetAll()
                                select (long)bills.Id).DefaultIfEmpty(0).Max() + 1;

            return currentMax;
        }

        public Int64  CreateNewBill(ref Bill bill)
        {
            genericDataRepository.Insert(bill);
            if (genericDataRepository.Save() > 0)
            {
                return  bill.Id;
            }
            return 1;
        }

        public Bill GetById(Int64 billId)
        {
            Bill b = genericDataRepository.GetByID(billId);
            return b;
        }

        public async Task<int> Update(Bill bill)
        {
            genericDataRepository.Update(bill);
            return await genericDataRepository.SaveAsync();
        }
        public async Task<int> Remove(Int64 id)
        {
            genericDataRepository.Delete(id);
            return await genericDataRepository.SaveAsync();
        }
    }
}
