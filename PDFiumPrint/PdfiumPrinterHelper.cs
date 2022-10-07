using System.Drawing.Printing;
using System.IO;

namespace PDFium
{
    public class PdfiumPrinterHelper
    {
        public bool PrintPDF(
           string printer, int copies, Stream stream)
        {
            bool Result;
            try
            {
                // Create the printer settings for our printer
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printer,
                    Copies = (short)copies,
                };
                // Now print the PDF document
                using (var document = PdfiumViewer.PdfDocument.Load(stream))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings =  new PageSettings(printerSettings)
                        {
                            Margins = new Margins(0, 0, 0, 0),
                        };
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
                Result =  true;
            }
            catch (System.Exception)
            {
                Result = false;
            }
            return Result;
        }
    }
}
