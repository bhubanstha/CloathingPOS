using iText.Layout.Properties;
using System.Collections.Generic;

namespace POS.Core.Utilities.PDF
{
    public static class SalesMockData
    {

        public static List<MockSales> GetMockSalesData(int totalMockSalesItem=10)
        {
            List<MockSales> sales = new List<MockSales>();
            for (int i = 1; i <= totalMockSalesItem; i++)
            {
                sales.Add(new MockSales
                {
                    ItemName = $"Sales item {i}",
                    Qty = i,
                    Discount = i * 50,
                    Rate = i * 250,
                    Total = ((i*(i*250)))
                }) ;
            }

            return sales;
        }

    }


    public class MockSales
    {
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal Discount { get; set; }
        public decimal Rate { get; set; }
        public decimal Total { get; set; }
    }

    public class TableHeader
    {

        private int _rowSpan=1;
        private int _colSpan=1;
        public string Text { get; set; }
        public TextAlignment TextAlignment { get; set; }

        

        public int RowSpan
        {
            get { return _rowSpan; }
            set { _rowSpan = value; }
        }

        
        public int ColSpan
        {
            get { return _colSpan; }
            set { _colSpan = value; }
        }

        public float CellWidth { get; set; }

    }
}
