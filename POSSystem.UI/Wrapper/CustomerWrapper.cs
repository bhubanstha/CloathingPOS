using POS.Model;
using System;

namespace POSSystem.UI.Wrapper
{
    public class CustomerWrapper : WrapperBase<Customer>
    {
        private long _sn;
        public CustomerWrapper(Customer customer) : base(customer)
        {

        }

        public long SN
        {
            get { return _sn; }
            set
            {
                _sn = value;
                OnPropertyChanged();
            }
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

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


        public string GoogleMap
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Mobile1
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Mobile2
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

    }
}
