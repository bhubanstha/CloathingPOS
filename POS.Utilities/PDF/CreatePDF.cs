using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using iText.Layout;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Layout.Renderer;
using iText.Layout.Layout;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace POS.Utilities.PDF
{
    public class CreatePDF
    {

        public string CreatePdfTable(Int64 billNo, string pdfPassword, string logoPath)
        {
            Document document;
            string pdfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bills", $"{billNo}.pdf");
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(pdfPath)))
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pdfPath));
            }
            using (FileStream stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Rectangle rectangle = new Rectangle(323.63f, 459.36f); //C6 paper size
                byte[] password = Encoding.ASCII.GetBytes(pdfPassword);
                WriterProperties props = new WriterProperties()
                    .SetStandardEncryption(password, password, EncryptionConstants.ALLOW_PRINTING,
                            EncryptionConstants.ENCRYPTION_AES_256 | EncryptionConstants.DO_NOT_ENCRYPT_METADATA);

                //PdfWriter writer = new PdfWriter(stream, props);
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                document = new Document(pdf, new PageSize(rectangle));
                document.SetMargins(14.4f, 10.0f, 14.4f, 10.0f);



                /*
                document.Add(CreateParagraph("TAX INVOICE", TextAlignment.CENTER, 0.5f));
                document.Add(CreateParagraph("MERO COMPANY PVT LTD", TextAlignment.CENTER, 0.5f));
                document.Add(CreateParagraph("Gongabu, Kathmandu", TextAlignment.CENTER, 0.5f));
                document.Add(CreateParagraph("PAN: 125436", TextAlignment.CENTER, 0.5f));

                document.Add(CreateLogoAtPoint(logoPath, new Point(15, 390)));

                document.Add(CreateParagraph("", TextAlignment.CENTER, 0.5f));
                document.Add(CreateParagraph($"Bill No.: {billNo}\t\t\t\t\t\t\tDate: {DateTime.Now.ToString("yyyy/mm/dd hh:mm tt")}", TextAlignment.LEFT, 0.5f));
                document.Add(CreateParagraph($"Customer Name: Bhuban Shrestha", TextAlignment.LEFT, 0.5f));
                document.Add(CreateParagraph($"Customer Address: Balaju, Kathmandu", TextAlignment.LEFT, 0.5f));
                document.Add(CreateParagraph($"Customer PAN: ", TextAlignment.LEFT, 1.0f));
                */

                List<MockSales> mockSales = SalesMockData.GetMockSalesData(1);

                int salesCount = mockSales.Count;
                int totalPages = (int)Math.Ceiling(salesCount / 10.0);
                int skipCount = 0;


                PageHeaderEventHandler handler = new PageHeaderEventHandler(document, logoPath, billNo);
                pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, handler);
                PageFooterEventHandler footerEventHandler = new PageFooterEventHandler(document, totalPages);
                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, footerEventHandler);

                
                for (int page = 1; page <= totalPages; page++)
                {
                    Table table = new Table(5);
                    table.SetWidth(UnitValue.CreatePercentValue(100));
                    PDFUtility.CreateInvoiceTableHeader(ref table);
                    int sn = 1;
                   
                    List<MockSales> workingItems = mockSales.Skip(skipCount).Take(10).ToList();
                    foreach (MockSales item in workingItems)
                    {
                        PDFUtility.CreateInoiceTableRecord(ref table, item, sn);
                        sn++;
                    }
                    PDFUtility.CreateEmptyRowInInoiceTable(ref table, 10 - workingItems.Count , sn);
                    PDFUtility.CreateInvoiceTotal(ref table, mockSales);
                    
                    skipCount += 10;
                    document.Add(table);
                    document.Add(PDFUtility.CreateParagraph($"", TextAlignment.LEFT, 1.0f));
                }
               

               



                //document.Add(CreateParagraph($"", TextAlignment.LEFT, 0.2f));
                //Table footerTable = new Table(1);
                //footerTable.SetWidth(UnitValue.CreatePercentValue(100));
                //cell = CreateCell("THANK YOU FOR YOUR BUSINESS!", TextAlignment.CENTER);
                //cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                //cell.SetBorder(Border.NO_BORDER);
                //footerTable.AddCell(cell);

                //document.Add(footerTable);
                //Paragraph p = PDFUtility.CreateParagraph($"Bill 1 of 2", TextAlignment.RIGHT, 0.2f);
                //p.SetFixedPosition(10, 10, UnitValue.CreatePercentValue(100));
                //document.Add(p);
                document.Close();
            }

            return pdfPath;


        }



        //public byte[] CreatePdfTableInMemory()
        //{

        //    byte[] pdfBytes;
        //    //string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.pdf");
        //    //FileStream stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Document document = new Document(PageSize.B6);
        //        PdfWriter writer = PdfWriter.GetInstance(document, ms);
        //        document.Open();

        //        PdfPTable table = new PdfPTable(2);
        //        PdfPCell cell = new PdfPCell(new Phrase("This is some text from memory"));
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Phrase("This is some another text from meory"));
        //        table.AddCell(cell);

        //        document.Add(table);

        //        cell = new PdfPCell(new Phrase("row 2 This is some text"));
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Phrase("row 2 This is some another text"));
        //        table.AddCell(cell);

        //        document.Add(table);

        //        document.Close();

        //        //path = pdfPath;

        //        pdfBytes = ms.ToArray();
        //        using (MemoryStream input = new MemoryStream(pdfBytes))
        //        {
        //            using (MemoryStream output = new MemoryStream())
        //            {
        //                PdfReader reader = new PdfReader(input);
        //                PdfEncryptor.Encrypt(reader, output, true, "123", "12ab", PdfWriter.ALLOW_SCREENREADERS);
        //                pdfBytes = output.ToArray();
        //            }
        //        }
        //        //return document;
        //    }
        //    return pdfBytes;
        //}
    }

    class PageHeaderEventHandler : IEventHandler
    {
        private Document doc;
        private string logoPath;
        private Int64 billno;

        public PageHeaderEventHandler(Document doc, string logoPath, Int64 billNo)
        {
            this.doc = doc;
            this.logoPath = logoPath;
            this.billno = billNo;
        }

        public void HandleEvent(Event currentEvent)
        {

            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            Rectangle pageSize = docEvent.GetPage().GetPageSize();
            pageSize.ApplyMargins(14.4f, 10.0f, 14.4f, 10.0f,false);

            Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);

            canvas.Add(PDFUtility.CreateParagraph("TAX INVOICE", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph("MERO COMPANY PVT LTD", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph("Gongabu, Kathmandu", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph("PAN: 125436", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateLogoAtPoint(logoPath, new Point(15, 390)));
            canvas.Add(PDFUtility.CreateParagraph("", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Bill No.: {this.billno}\t\t\t\t\t\t\tDate: {DateTime.Now.ToString("yyyy/mm/dd hh:mm tt")}", TextAlignment.LEFT, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Customer Name: Bhuban Shrestha", TextAlignment.LEFT, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Customer Address: Balaju, Kathmandu", TextAlignment.LEFT, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Customer PAN: ", TextAlignment.LEFT, 0.5f));
            doc.SetTopMargin(145f);
            canvas.Close();
        }

        
    }

    class PageFooterEventHandler : IEventHandler
    {
        private Document doc;
        private Table table;
        private int _totalPage;

        public PageFooterEventHandler(Document doc, int totalPage)
        {
            this.doc = doc;
            this._totalPage = totalPage;
            table = CreateFooterTable();
        }

        public void HandleEvent(Event currentEvent)
        {

            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            PdfPage page = docEvent.GetPage();
            Rectangle pageSize = page.GetPageSize();
            pageSize.ApplyMargins(pageSize.GetHeight()-35, 10.0f, 14.4f, 10.0f, false);

            Canvas canvas = new Canvas(page, pageSize);
            //canvas.ShowTextAligned(String.Format("Page %d of", pageNum));
            
            canvas.Add(table);

            if(this._totalPage>1)
            {
               
                int pageNum = docEvent.GetDocument().GetPageNumber(page);

                Rectangle bilCountRectangle = new Rectangle(page.GetPageSize().GetWidth(), 200);// page.GetPageSize();
                bilCountRectangle.ApplyMargins(pageSize.GetHeight() - 250, 10.0f, 14.4f, 250.0f, false);
                Canvas billingCanvas = new Canvas(page, bilCountRectangle);
                Paragraph p = new Paragraph($"Bill {pageNum} of {_totalPage}");
                p.SetRotationAngle(145);
                billingCanvas.Add(p);
            }
           
        }

       
        private Table CreateFooterTable()
        {
            Table footerTable = new Table(1);
            footerTable.SetWidth(UnitValue.CreatePercentValue(100));
            Cell cell = PDFUtility.CreateCell("THANK YOU FOR YOUR BUSINESS!", TextAlignment.CENTER);
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            cell.SetBorder(Border.NO_BORDER);
            footerTable.AddCell(cell);

            return footerTable;
            //document.Add(footerTable);
        }
    }
}
