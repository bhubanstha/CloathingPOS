using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using POS.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.PDF
{
    public class CreateProfitLossReport
    {
        public async Task<string> CreateReport(List<ProfitLossReport> record, ShopVM shop, decimal profit, int year, int month)
        {
            return await Task.Run(() =>
            {
                Document document;
                string pdfPath = FileUtility.GetReportPath(year, month);
                if (string.IsNullOrEmpty(pdfPath)) return "";

                if (!Directory.Exists(System.IO.Path.GetDirectoryName(pdfPath)))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pdfPath));
                }
                using (FileStream stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {

                    //Rectangle rectangle = new Rectangle(323.63f, 459.36f); //C6 paper size
                    byte[] password = Encoding.ASCII.GetBytes(shop.PdfPassword);
                    WriterProperties props = new WriterProperties()
                        .SetStandardEncryption(password, password, EncryptionConstants.ALLOW_PRINTING,
                                EncryptionConstants.ENCRYPTION_AES_256 | EncryptionConstants.DO_NOT_ENCRYPT_METADATA);

                    PdfWriter writer = new PdfWriter(stream, props);
                    PdfDocument pdf = new PdfDocument(writer);
                    document = new Document(pdf, StandardPaperSize.A4.Rotate());
                    document.SetMargins(14.4f, 10.0f, 14.4f, 10.0f);

                    ReportPageHeaderEventHandler handler = new ReportPageHeaderEventHandler(document, shop);
                    pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, handler);
                    Table table = new Table(11);
                    PDFUtility.CreateReportTableHeader(ref table);
                    int i = 1;
                    foreach (var item in record)
                    {
                        Cell c = PDFUtility.CreateCell(i.ToString(), TextAlignment.CENTER);
                        table.AddCell(c);
                        table.AddCell(PDFUtility.CreateCell($"{item.BillingDate.ToString("yyyy/MM/dd")}", TextAlignment.LEFT));
                        table.AddCell(PDFUtility.CreateCell($"{item.Brand}", TextAlignment.LEFT));
                        table.AddCell(PDFUtility.CreateCell($"{item.Category}", TextAlignment.LEFT));
                        table.AddCell(PDFUtility.CreateCell($"{item.ItemName}", TextAlignment.LEFT));
                        table.AddCell(PDFUtility.CreateCell($"{item.ItemCode}", TextAlignment.LEFT));
                        table.AddCell(PDFUtility.CreateCell($"{item.Size}", TextAlignment.LEFT));
                        table.AddCell(PDFUtility.CreateCell($"{item.SalesQty}", TextAlignment.CENTER));
                        table.AddCell(PDFUtility.CreateCell($"{item.PurchaseTotal}", TextAlignment.RIGHT));
                        table.AddCell(PDFUtility.CreateCell($"{item.SalesTotal}", TextAlignment.RIGHT));
                        table.AddCell(PDFUtility.CreateCell($"{item.Profit}", TextAlignment.RIGHT));
                        i++;
                    }
                    Cell c1 = PDFUtility.CreateCell("Total Profit", TextAlignment.CENTER, colSpan: 10);
                    c1.SetBold();
                    table.AddCell(c1);
                    table.AddCell(PDFUtility.CreateCell($"{profit}", TextAlignment.RIGHT).SetBold());

                    document.Add(table);
                    document.Close();
                }

                return pdfPath;
            });
        }
    }

    class ReportPageHeaderEventHandler : IEventHandler
    {
        private Document doc;
        private ShopVM _shop;

        public ReportPageHeaderEventHandler(Document doc,  ShopVM shop)
        {
            this.doc = doc;
            this._shop = shop;
        }
 
        public void HandleEvent(Event currentEvent)
        {

            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            Rectangle pageSize = docEvent.GetPage().GetPageSize();
            pageSize.ApplyMargins(14.4f, 10.0f, 14.4f, 10.0f, false);

            Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);

            string invoiceHeader = "Profit and Loss Report";

            canvas.Add(PDFUtility.CreateParagraph($"{invoiceHeader}", TextAlignment.CENTER, 0.5f, 12));
            canvas.Add(PDFUtility.CreateParagraph($"{_shop.Name}", TextAlignment.CENTER, 0.5f, 12));
            canvas.Add(PDFUtility.CreateParagraph($"{_shop.Address}", TextAlignment.CENTER, 0.5f));
            canvas.Add(PDFUtility.CreateParagraph($"PAN: {_shop.PANNumber}", TextAlignment.CENTER, 0.5f, 12));
            canvas.Add(PDFUtility.CreateLogoAtPoint(_shop.LogoPath, new Point(15, 530)));
            canvas.Add(PDFUtility.CreateParagraph("", TextAlignment.CENTER, 0.5f));
            doc.SetTopMargin(75f);
            canvas.Close();
        }


    }
}
