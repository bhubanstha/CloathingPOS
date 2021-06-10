using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using POS.Model;
using POS.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.PDF
{
    public class CreatePDF
    {

        public async Task<string> CreateInvoice(BillModel bill, List<SalesModel> salesItem, Shop shop, string pdfPassword)
        {
            Document document;
            string pdfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bills", $"{bill.BillNo}.pdf");
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

                //List<SalesModel> mockSales = salesItem;// SalesMockData.GetMockSalesData(1);

                int salesCount = salesItem.Count;
                int totalPages = salesCount > 0 ? (int)Math.Ceiling(salesCount / 10.0) : 1;
                int skipCount = 0;


                PageHeaderEventHandler handler = new PageHeaderEventHandler(document, bill, shop);
                pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, handler);
                PageFooterEventHandler footerEventHandler = new PageFooterEventHandler(totalPages);
                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, footerEventHandler);

                
                for (int page = 1; page <= totalPages; page++)
                {
                    Table table = new Table(5);
                    table.SetWidth(UnitValue.CreatePercentValue(100));
                    PDFUtility.CreateInvoiceTableHeader(ref table);
                    int sn = 1;
                   
                    List<SalesModel> workingItems = salesItem.Skip(skipCount).Take(10).ToList();
                    foreach (SalesModel item in workingItems)
                    {
                        PDFUtility.CreateInoiceTableRecord(ref table, item, sn);
                        sn++;
                    }
                    PDFUtility.CreateEmptyRowInInoiceTable(ref table, 10 - workingItems.Count , sn);
                    PDFUtility.CreateInvoiceTotal(ref table, salesItem, shop);
                    
                    skipCount += 10;
                    document.Add(table);
                    document.Add(PDFUtility.CreateParagraph($"", TextAlignment.LEFT, 1.0f));
                }
                document.Close();
            }

            return pdfPath;


        }
    }

    class PageHeaderEventHandler : IEventHandler
    {
        private Document doc;
        private Shop _shop;
        private BillModel _bill;

        public PageHeaderEventHandler(Document doc, BillModel bill, Shop shop)
        {
            this.doc = doc;
            this._shop = shop;
            this._bill = bill;
        }

        public void HandleEvent(Event currentEvent)
        {

            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            Rectangle pageSize = docEvent.GetPage().GetPageSize();
            pageSize.ApplyMargins(14.4f, 10.0f, 14.4f, 10.0f,false);

            Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);

            string invoiceHeader = _shop.CalculateVATOnSales ? "TAX INVOICE" : "INVOICE";

            canvas.Add(PDFUtility.CreateParagraph($"{invoiceHeader}", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"{_shop.Name}", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"{_shop.Address}", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"PAN: {_shop.PANNumber}", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateLogoAtPoint(_shop.LogoPath, new Point(15, 390)));
            canvas.Add(PDFUtility.CreateParagraph("", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Bill No.: {_bill.BillNo}\t\t\t\t\t\t\tDate: {DateTime.Now.ToString("yyyy/mm/dd hh:mm tt")}", TextAlignment.LEFT, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Customer Name: {_bill.BillTo}", TextAlignment.LEFT, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Customer Address: {_bill.BillingAddress}", TextAlignment.LEFT, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"Customer PAN: {_bill.BillingPAN}", TextAlignment.LEFT, 0.5f));
            doc.SetTopMargin(145f);
            canvas.Close();
        }

        
    }

    class PageFooterEventHandler : IEventHandler
    {
        private Table table;
        private int _totalPage;

        public PageFooterEventHandler(int totalPage)
        {
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
