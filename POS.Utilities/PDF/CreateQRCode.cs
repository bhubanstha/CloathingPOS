using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Net.Codecrete.QrCodeGenerator;
using POS.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace POS.Utilities.PDF
{
    public class CreateQRCode
    {
        private string pdfPassword;

        public CreateQRCode(string pdfPassword)
        {
            this.pdfPassword = pdfPassword;
        }
        public async Task<string> CreateLabel(List<Inventory> inventoryItems, int leaveLabels = 0)
        {
            return await Task.Run(()=>
            {
                string pdfpath = FileUtility.GetLabelPdfPath(true);
                using (FileStream stream = new FileStream(pdfpath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Margin margin = new Margin(10);
                    Document doc = stream.CreateDocument(PageSize.A4, margin, "");
                    /*
                     Label printer paper can have many label in sigle paper. Most often it there can be
                    1, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 21, 24, 30, 33, 40, 48, 56, 65, 84 labels in single A4 paper 

                    Below consideres (3 columns x 7 rows) =  21 label in single A4 paper 
                    Each label is 63.5 mm x 38.1mm OR 2.5 inch X 1.5 inch
                    */

                    Table table = new Table(5);//3 columns contains label and 2 will be a column seperator
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    int colCount = 1;
                    for (int i = 0; i < leaveLabels; i++)
                    {
                        Cell cell = CreateLabelCell();
                        table.AddCell(cell);

                        if (colCount < 5)
                        {
                            cell = CreateLabelCell(5);
                            table.AddCell(cell);
                        }
                        colCount += 2;
                        if (colCount > 5)
                            colCount = 1;
                    }


                    foreach (Inventory inventory in inventoryItems)
                    {
                        string code = $"{inventory.BarCode}";
                        System.Drawing.Image qrImage = GetQrCode(code);
                        ImageData imageData = ImageDataFactory.Create(qrImage, System.Drawing.Color.Transparent);
                        Image img = new Image(imageData);

                        for (int qty = 0; qty < inventory.Quantity; qty++)
                        {
                            Cell cell = CreateLabelCell();
                            cell.Add(img.SetHorizontalAlignment(HorizontalAlignment.CENTER));
                            Paragraph p = new Paragraph($"Code: {inventory.Code}");
                            p.SetTextAlignment(TextAlignment.CENTER);
                            cell.Add(p).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                            cell.SetPadding(5);
                            table.AddCell(cell);

                            if (colCount < 5)
                            {
                                cell = CreateLabelCell(5);
                                table.AddCell(cell);
                            }
                            colCount += 2;
                            if (colCount > 5)
                                colCount = 1;
                        }
                    }
                    doc.Add(table);
                    doc.Close();
                }
                return pdfpath;
            });

            //t.Wait();

            // return await Task.FromResult(t.Result);
            //return t;
        }


        private System.Drawing.Bitmap GetQrCode(string code)
        {
            var qr = QrCode.EncodeText(code, QrCode.Ecc.Medium);
            var bitmap = qr.ToBitmap(3, 0);
            return bitmap;
        }

        private Cell CreateLabelCell(float width = 180f, float height = 105f)
        {
            Cell c = new Cell();
            //1 inch = 72 points
            c.SetWidth(width); // 2.5 inch = 180 points
            c.SetHeight(height);// 1.5 inch = 108f points
            c.SetBorder(Border.NO_BORDER);
            return c;
        }
    }
}
