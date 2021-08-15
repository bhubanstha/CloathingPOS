using POS.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities
{
    public class MonthProvider
    {
        public static List<Month> GetMonths()
        {
            List<Month> months = new List<Month>
            {
                new Month(1, "January"),
                new Month(2, "February"),
                new Month(3, "March"),
                new Month(4, "April"),
                new Month(5, "May"),
                new Month(6, "June"),
                new Month(7, "July"),
                new Month(8, "August"),
                new Month(9, "September"),
                new Month(10, "October"),
                new Month(11, "November"),
                new Month(12, "December")
            };
            return months;
        }
    }

    public class YearProvider
    {
        public static List<Year> GetYears(int appDeployedYear)
        {
            List<Year> years = new List<Year>();
            int currentYear = DateTime.Now.Year;
            for (int i = appDeployedYear; i <= currentYear; i++)
            {
                years.Add(new Year(i, i));
            }
            return years;
        }
    }
}
