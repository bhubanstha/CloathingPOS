using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Wrapper
{
    public class BranchWrapper : WrapperBase<Branch>
    {
        public BranchWrapper(Branch branch) : base(branch)
        {

        }

        public Int64 Id
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public Int64 ShopId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public string BranchName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string BranchAddress
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
