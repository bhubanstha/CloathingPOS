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
            //long? currentMax = genericDataRepository.GetAll().Max(b => b.BillNo);

            long? currentMax = (from bills in genericDataRepository.GetAll()
                                select (long?)bills.BillNo).Max();

            return currentMax.HasValue ? currentMax.Value + 1 : 1;
        }
    }
}
