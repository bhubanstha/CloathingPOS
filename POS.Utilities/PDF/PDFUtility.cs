using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using POS.Model;
using POS.Model.ViewModel;
using POS.Utilities.Extension;
using POS.Utilities.NumToWord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POS.Utilities.PDF
{
    public class Margin
    {
        public float TopMargin { get; private set; }
        public float RightMargin { get; private set; }
        public float BottomMargin { get; private set; }
        public float LeftMargin { get; private set; }
        public Margin(float top, float right, float bottom, float left)
        {
            this.TopMargin = top;
            this.RightMargin = right;
            this.BottomMargin = bottom;
            this.LeftMargin = left;
        }

        public Margin(float topAndBottom, float leftAndRight)
        {
            this.TopMargin = topAndBottom;
            this.BottomMargin = topAndBottom;

            this.LeftMargin = leftAndRight;
            this.RightMargin = leftAndRight;
        }

        public Margin(float all)
        {
            this.TopMargin = all;
            this.RightMargin = all;
            this.BottomMargin = all;
            this.LeftMargin = all;
        }
    }
    public static class PDFUtility
    {

        public static Document CreateDocument(this FileStream stream, StandardPaperSize pageSize, Margin pageMargin, string pdfPassword)
        {
            PdfDocument pdfDoc = stream.CreatePdfDocument(pdfPassword);

            Document document = new Document(pdfDoc, pageSize);
            document.SetMargins(pageMargin.TopMargin, pageMargin.RightMargin, pageMargin.BottomMargin, pageMargin.LeftMargin);

            return document;
        }

        public static Document CreateDocument(this FileStream stream, PageSize pageSize, Margin pageMargin, string pdfPassword)
        {

            PdfDocument pdfDoc = stream.CreatePdfDocument(pdfPassword);
            Document document = new Document(pdfDoc, pageSize);
            document.SetMargins(pageMargin.TopMargin, pageMargin.RightMargin, pageMargin.BottomMargin, pageMargin.LeftMargin);

            return document;
        }

        private static PdfDocument CreatePdfDocument(this FileStream stream, string pdfPassword)
        {
            PdfWriter writer;
            if (!string.IsNullOrEmpty(pdfPassword))
            {
                byte[] password = Encoding.ASCII.GetBytes(pdfPassword);
                WriterProperties props = new WriterProperties()
                    .SetStandardEncryption(password, password, EncryptionConstants.ALLOW_PRINTING,
                            EncryptionConstants.ENCRYPTION_AES_256 | EncryptionConstants.DO_NOT_ENCRYPT_METADATA);
                writer = new PdfWriter(stream, props);
            }
            else
            {
                writer = new PdfWriter(stream);
            }
            PdfDocument pdfDoc = new PdfDocument(writer);
            return pdfDoc;
        }

        public static Paragraph CreateParagraph(string text, TextAlignment textAlignment = TextAlignment.JUSTIFIED, float lineSpacing = 1.0f, float fontSize = 10.0f)
        {
            Paragraph p = new Paragraph(text).SetFontSize(fontSize);
            p.SetTextAlignment(textAlignment);
            p.SetMultipliedLeading(lineSpacing);
            return p;
        }

        public static Cell CreateCell(string text, TextAlignment textAlignment = TextAlignment.CENTER, int rowSpan = 1, int colSpan = 1, float width = 10)
        {
            Cell c = new Cell(rowSpan, colSpan);
            c.SetWidth(width);
            Paragraph p = CreateParagraph(text, textAlignment);
            c.Add(p);
            c.SetTextAlignment(textAlignment);
            //Border b = new SolidBorder(ColorConstants.LIGHT_GRAY, 1);
            // c.SetBorderBottom(b);
            c.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            return c;
        }
        public static Image CreateLogoAtPoint(string logoName, Point point)
        {
            string logoFullPath = FilePath.GetLogoFullPath(logoName);
            ImageData imageData = ImageDataFactory.Create(logoFullPath);
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

        public static void CreateReportTableHeader(ref Table reportTable)
        {
            List<TableHeader> headerItems = new List<TableHeader>
            {
                new TableHeader{Text = "S.N", TextAlignment= TextAlignment.CENTER, CellWidth=5},
                new TableHeader{Text = "Date", TextAlignment= TextAlignment.CENTER, CellWidth=60},
                new TableHeader{Text = "Brand", TextAlignment= TextAlignment.CENTER, CellWidth=60},
                new TableHeader{Text = "Category", TextAlignment= TextAlignment.CENTER, CellWidth=60},
                new TableHeader{Text = "Item", TextAlignment= TextAlignment.CENTER, CellWidth=140},
                new TableHeader{Text = "Code", TextAlignment= TextAlignment.CENTER, CellWidth=60},
                new TableHeader{Text = "Size", TextAlignment= TextAlignment.CENTER, CellWidth=40},
                new TableHeader{Text = "Qty", TextAlignment= TextAlignment.CENTER, CellWidth=40},
                new TableHeader{Text = "Purchase Total", TextAlignment= TextAlignment.CENTER, CellWidth=100},
                new TableHeader{Text = "Sales Total", TextAlignment= TextAlignment.CENTER, CellWidth=100},
                new TableHeader{Text = "Profit", TextAlignment= TextAlignment.CENTER, CellWidth=100}
            };

            CreateTableHeader(ref reportTable, headerItems);
        }

        public static void CreateInoiceTableRecord(ref Table invoiceTable, SalesModel mockSales, int sn)
        {
            Cell c = CreateCell(sn.ToString(), TextAlignment.CENTER);
            invoiceTable.AddCell(c);
            invoiceTable.AddCell(CreateCell($"{mockSales.ProductName} - {mockSales.Size}({mockSales.ColorName})", TextAlignment.LEFT));
            invoiceTable.AddCell(CreateCell(mockSales.SalesQuantity.ToString(), TextAlignment.CENTER));
            invoiceTable.AddCell(CreateCell(mockSales.RetailRate.ToString(), TextAlignment.CENTER));
            invoiceTable.AddCell(CreateCell($"{mockSales.SalesQuantity * mockSales.RetailRate}", TextAlignment.RIGHT));

        }

        public static void CreateInoiceTableRecord(ref Table invoiceTable, Sales item, int sn)
        {
            Cell c = CreateCell(sn.ToString(), TextAlignment.CENTER);
            invoiceTable.AddCell(c);
            invoiceTable.AddCell(CreateCell($"{item.Inventory.Name} - {item.Inventory.Size}({item.Inventory.ColorName})", TextAlignment.LEFT));
            invoiceTable.AddCell(CreateCell(item.SalesQuantity.ToString(), TextAlignment.CENTER));
            invoiceTable.AddCell(CreateCell(item.Inventory.RetailRate.ToString(), TextAlignment.CENTER));
            invoiceTable.AddCell(CreateCell($"{item.SalesQuantity * item.Inventory.RetailRate}", TextAlignment.RIGHT));

        }

        public static void CreateInvoiceTotal(ref Table inoiceTable, List<SalesModel> sales, ShopVM shop)
        {

            decimal total = sales.Sum(x => x.SalesQuantity * x.RetailRate);
            decimal totalDiscount = sales.Sum(x => x.Discount);
            decimal vatAmount = shop.CalculateVATOnSales ? Math.Ceiling((13 * total) / 100) : 0;
            decimal grandTotal = total + vatAmount - totalDiscount;

            string amountInWord = grandTotal > 0 ? NumberToWord.Parse($"{grandTotal}") : "";
            inoiceTable.AddCell(CreateCell($"{amountInWord}.", TextAlignment.CENTER, 4, 2).SetVerticalAlignment(VerticalAlignment.MIDDLE));

            inoiceTable.AddCell(CreateCell("Taxable Total", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(total > 0 ? total.ToString() : "")}", TextAlignment.RIGHT));

            inoiceTable.AddCell(CreateCell("Discount", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(totalDiscount > 0 ? totalDiscount.ToString() : "-")}", totalDiscount > 0 ? TextAlignment.RIGHT : TextAlignment.CENTER));

            inoiceTable.AddCell(CreateCell("VAT(13%)", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(vatAmount > 0 ? vatAmount.ToString() : "-")}", vatAmount > 0 ? TextAlignment.RIGHT : TextAlignment.CENTER));

            inoiceTable.AddCell(CreateCell("Grand Total", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(grandTotal > 0 ? grandTotal.ToString() : "") }", TextAlignment.RIGHT));
        }

        public static void CreateInvoiceTotal(ref Table inoiceTable, List<Sales> sales, ShopVM shop)
        {

            decimal total = sales.Sum(x => x.SalesQuantity * x.Inventory.RetailRate);
            decimal totalDiscount = sales.Sum(x => x.Discount);
            decimal vatAmount = shop.CalculateVATOnSales ? Math.Ceiling((13 * total) / 100) : 0;
            decimal grandTotal = total + vatAmount - totalDiscount;

            string amountInWord = grandTotal > 0 ? NumberToWord.Parse($"{grandTotal}") : "";
            inoiceTable.AddCell(CreateCell($"{amountInWord}.", TextAlignment.CENTER, 4, 2).SetVerticalAlignment(VerticalAlignment.MIDDLE));

            inoiceTable.AddCell(CreateCell("Taxable Total", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(total > 0 ? total.ToString() : "")}", TextAlignment.RIGHT));

            inoiceTable.AddCell(CreateCell("Discount", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(totalDiscount > 0 ? totalDiscount.ToString() : "-")}", totalDiscount > 0 ? TextAlignment.RIGHT : TextAlignment.CENTER));

            inoiceTable.AddCell(CreateCell("VAT(13%)", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(vatAmount > 0 ? vatAmount.ToString() : "-")}", vatAmount > 0 ? TextAlignment.RIGHT : TextAlignment.CENTER));

            inoiceTable.AddCell(CreateCell("Grand Total", TextAlignment.LEFT, 1, 2));
            inoiceTable.AddCell(CreateCell($"{(grandTotal > 0 ? grandTotal.ToString() : "") }", TextAlignment.RIGHT));
        }


        public static void CreateEmptyRowInInoiceTable(ref Table inoiceTable, int rowsToCreate, int snToStart)
        {
            if (rowsToCreate > 0)
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
