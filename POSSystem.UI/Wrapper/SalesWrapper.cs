using POS.Model.ViewModel;
using POSSystem.UI.UIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Wrapper
{
    public class SalesWrapper : WrapperBase<SalesModel>
    {

        public SalesWrapper(SalesModel model) :base(model)
        {

        }

        public Int64 ProductId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public int SalesQuantity 
        { 
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }


        public decimal Discount
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        

        public decimal RetailRate
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public string ProductName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Size
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Color
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string ColorName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public Int64 CategoryId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public string CategoryName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public Int64 BrandId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }
        public string BrandName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public void Copy(SalesWrapper source)
        {
            this.ProductId = source.ProductId;
            this.ProductName = source.ProductName;
            this.RetailRate = source.RetailRate;
            this.SalesQuantity = source.SalesQuantity;
            this.Size = source.Size;
            this.Discount = source.Discount;
            this.Color = source.Color;
            this.CategoryId = source.CategoryId;
            this.CategoryName = source.CategoryName;
            this.BrandId = source.BrandId;
            this.BrandName = source.BrandName;
            this.ColorName = source.ColorName;
        }
    }

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
}
