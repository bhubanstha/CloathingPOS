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

namespace POS.Utilities.PDF
{
    public class CreatePDF
    {


        public string CreatePdfTable(Int64 billNo, string pdfPassword)
        {
            Document document;
            string pdfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bills", $"{billNo}.pdf");
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(pdfPath)))
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pdfPath));
            }
            using (FileStream stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {

                byte[] password = Encoding.ASCII.GetBytes(pdfPassword);
                WriterProperties props = new WriterProperties()
                    .SetStandardEncryption(password, password, EncryptionConstants.ALLOW_PRINTING,
                            EncryptionConstants.ENCRYPTION_AES_256 | EncryptionConstants.DO_NOT_ENCRYPT_METADATA);

                PdfWriter writer = new PdfWriter(stream, props);
                PdfDocument pdf = new PdfDocument(writer);
                document = new Document(pdf, PageSize.B6);



                Table table = new Table(2);

                //row 1
                Cell cell = new Cell();
                cell.Add(new Paragraph("This is row1 column1"));
                table.AddCell(cell);
                cell = new Cell();
                cell.Add(new Paragraph("This is row1 column2"));
                table.AddCell(cell);

                //row2
                cell = new Cell();
                cell.Add(new Paragraph("This is row2 column1"));
                table.AddCell(cell);
                cell = new Cell();
                cell.Add(new Paragraph("This is row2 column2"));
                table.AddCell(cell);

                document.Add(table);
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
}
