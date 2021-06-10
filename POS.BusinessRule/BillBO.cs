﻿using POS.Data;
using POS.Data.Repository;
using POS.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class BillBO
    {
        private IGenericDataRepository<Bill> genericDataRepository;

        public BillBO()
        {
            genericDataRepository = new DataRepository<Bill>(new POSDataContext());
        }

        public void AddBillToMemory(ref Bill bill)
        {
            genericDataRepository.Insert(bill);
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

        public async Task<int> Remove(Int64 id)
        {
            genericDataRepository.Delete(id);
            return await genericDataRepository.SaveAsync();
        }
    }
}
