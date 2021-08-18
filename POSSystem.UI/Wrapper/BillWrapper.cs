using POS.Model;
using POS.Model.ViewModel;
using System;

namespace POSSystem.UI.Wrapper
{
    public class BillWrapper : WrapperBase<BillModel>
    {
        public BillWrapper(BillModel billModel):base(billModel)
        {

        }

        public Int64 BillNo
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public Int64 CustomerId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public Int64 BranchId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public Int64 UserId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public DateTime BillDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public string BillTo
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string BillingAddress
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string BillingPAN
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        } 
        
        public bool CalculateVAT
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public decimal VAT
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public decimal GrandTotal
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }



    }

    public class BillEntityWrapper : WrapperBase<Bill>
    {
        public BillEntityWrapper(Bill bill) : base(bill)
        {

        }

        public Int64 Id
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public Int64 CustomerId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public DateTime BillDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public decimal VAT
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public Customer Customer
        {
            get { return GetValue<Customer>(); }
            set { SetValue(value); }
        }


    }
}
