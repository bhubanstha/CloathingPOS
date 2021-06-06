using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using POS.Model;
using POS.Utilities.NumToWord;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.Utilities.PDF
{
    public static class PDFUtility
    {
        public static Paragraph CreateParagraph(string text, TextAlignment textAlignment = TextAlignment.JUSTIFIED, float lineSpacing = 1.0f)
        {
            Paragraph p = new Paragraph(text);
            p.SetTextAlignment(textAlignment);
            p.SetMultipliedLeading(lineSpacing);
            return p;
        }

        public static Cell CreateCell(string text, TextAlignment textAlignment = TextAlignment.CENTER, int rowSpan = 1, int colSpan = 1, float width=10 )
        {
            Cell c = new Cell(rowSpan, colSpan);
            c.SetWidth(width);
            Paragraph p = CreateParagraph(text, textAlignment);
            c.Add(p);
            Border b = new SolidBorder(ColorConstants.LIGHT_GRAY, 1);
            c.SetTextAlignment(textAlignment);
            c.SetBorderBottom(b);
            return c;
        }
        public static Image CreateLogoAtPoint(string logoPath, Point point)
        {
            ImageData imageData = ImageDataFactory.Create(logoPath);
            Image logo = new Image(imageData);
            logo.SetFixedPosition((float)point.x, ((float)point.y));
            logo.ScaleAbsolute(55, 55);
            return logo;
        }

        public static void CreateTableHeader(ref Table table, List<TableHeader> headerCollection)
        {
            foreach (TableHeader header in headerCollection)
            {
                table.AddHeaderCell(CreateCell(header.Text, header.TextAlignment, header.RowSpan, header.ColSpan, header.CellWidth));
            }
        }

        public static void CreateInvoiceTableHeader(ref Table invoiceTable)
        {
            List<TableHeader> headerItems = new List<TableHeader>
            {
                new TableHeader{Text = "S.N", TextAlignment= TextAlignment.CENTER, CellWidth=5},
                new TableHeader{Text = "Particular", TextAlignment= TextAlignment.CENTER, CellWidth=150},
                new TableHeader{Text = "Qty", TextAlignment= TextAlignment.CENTER, CellWidth=30},
                new TableHeader{Text = "Rate", TextAlignment= TextAlignment.CENTER, CellWidth=60},
                new TableHeader{Text = "Total", TextAlignment= TextAlignment.CENTER, CellWidth=60}
            };

            CreateTableHeader(ref invoiceTable, headerItems);
        }

        public static void CreateInoiceTableRecord(ref Table invoiceTable, Sales mockSales, int sn)
        {
            Cell c = CreateCell(sn.ToString(), TextAlignment.CENTER);
            invoiceTable.AddCell(c);
            invoiceTable.AddCell(CreateCell(mockSales.Inventory.Name, TextAlignment.LEFT));
            invoiceTable.AddCell(CreateCell(mockSales.SalesQuantity.ToString(), TextAlignment.CENTER));
            invoiceTable.AddCell(CreateCell(mockSales.Rate.ToString(), TextAlignment.CENTER));
            invoiceTable.AddCell(CreateCell($"{mockSales.SalesQuantity * mockSales.Rate}", TextAlignment.RIGHT));

        }

        public static void CreateInvoiceTotal(ref Table inoiceTable, List<Sales> sales)
        {

            decimal total = sales.Sum(x => x.SalesQuantity * x.Rate);
            decimal totalDiscount = sales.Sum(x => x.Discount);
            decimal vatAmount = Math.Ceiling((13 * total) / 100);
            decimal grandTotal = total + vatAmount - totalDiscount;

            string amountInWord = grandTotal>0?  NumberToWord.Parse($"{grandTotal}") : "";
            inoiceTable.AddCell(CreateCell($"{amountInWord}.", TextAlignment.CENTER, 4, 2).SetVerticalAlignment(VerticalAlignment.MIDDLE));

            inoiceTable.AddCell(CreateCell("Taxable Total", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(total>0?  total.ToString() : "" )}", TextAlignment.RIGHT));

            inoiceTable.AddCell(CreateCell("Discount", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(totalDiscount>0? totalDiscount.ToString() : "" )}", TextAlignment.RIGHT));

            inoiceTable.AddCell(CreateCell("VAT(13%)", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(vatAmount > 0? vatAmount.ToString() : "" )}", TextAlignment.RIGHT));

            inoiceTable.AddCell(CreateCell("Grand Total", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(grandTotal>0? grandTotal.ToString() : "" ) }", TextAlignment.RIGHT));
        }


        public static void CreateEmptyRowInInoiceTable(ref Table inoiceTable, int rowsToCreate, int snToStart)
        {
            if(rowsToCreate>0)
            {
                for (int i = 0; i < rowsToCreate; i++)
                {
                    inoiceTable.AddCell(CreateCell($"{snToStart + i}"));
                    inoiceTable.AddCell(CreateCell(""));
                    inoiceTable.AddCell(CreateCell(""));
                    inoiceTable.AddCell(CreateCell(""));
                    inoiceTable.AddCell(CreateCell(""));
                }
            }
        }
    }
}
