using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Wrapper
{
    public class BrandWrapper : WrapperBase<Brand>
    {

        public BrandWrapper(Brand brand):base(brand)
        {

        }


        public Int64 Id
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }


        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
