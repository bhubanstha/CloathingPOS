using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Wrapper
{
    public class ShopWrapper : WrapperBase<Shop>
    {
        public ShopWrapper(Shop shop):base(shop)
        {

        }

        public Int64 Id
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public string BranchName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


        public string PANNumber
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LogoPath
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


        public bool CalculateVATOnSales
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool PrintInvoice
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string PdfPassword
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

    }
}
