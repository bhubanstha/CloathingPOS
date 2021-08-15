using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model.ViewModel
{
    public class Year
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public Year()
        {

        }

        public Year(int yearId, int yearValue)
        {
            this.Id = yearId;
            this.Value = yearValue;
        }
    }

    public class Month
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Month()
        {

        }

        public Month(int monthId, string monthName)
        {
            this.Id = monthId;
            this.Name = monthName;
        }

    }
}
