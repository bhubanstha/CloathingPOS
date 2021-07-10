using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Wrapper
{
    public class CategoryWrapper : WrapperBase<Category>
    {

        public CategoryWrapper(Category category):base(category)
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

        public CategoryWrapper ReturnWrapperFromModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            return this;
        }

    }
}
