using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WerServer
{
    public class WindowsPrint : IDisposable
    {
        private bool disposedValue;
        private string PrinterName;
        private int Count;

        public Uri Url { get; set; }
        public string FilePath { get; set; }

        public WindowsPrint(List<DataFormat> requestData)
        {
            try
            {
                string printerName = requestData.Find(x => x.Variable.ToLower() == "printer").Valor;
                if (string.IsNullOrEmpty(printerName))
                {
                    Console.WriteLine("Printer Name is REQUIRED to print...");
                    throw new Exception("Printer Name is REQUIRED to print");
                }
                else
                {
                    this.PrinterName = printerName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Printer Name is REQUIRED to print...");
                throw new Exception("Printer Name is REQUIRED to print", ex);
            }
            try
            {
                string url = requestData.Find(x => x.Variable.ToLower() == "url").Valor;
                if (!string.IsNullOrEmpty(url)) SetUrl(url);
                else Console.WriteLine("no url to request a file...");
            }
            catch
            {
                Console.WriteLine("no url to request a file...");
                this.Url = null;
            }
            try
            {
                this.FilePath = requestData.Find(x => x.Variable.ToLower() == "file").Valor;
            }
            catch
            {
                this.FilePath = string.Empty;
            }
            try
            {
                this.Count = Convert.ToInt32(requestData.Find(x => x.Variable.ToLower() == "count").Valor);
            }
            catch
            {
                this.Count = 1;
            }
        }

        public void SetUrl(string url)
        {
            try
            {
                this.Url = new Uri(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Url not valid", ex);
                throw new Exception("Url not valid", ex);
            }
        }

        public async Task<bool> PrintUrl()
        {
            return await PrintUrl(this.Count);
        }

        public async Task<bool> PrintUrl(int pagecount)
        {
            if (!string.IsNullOrEmpty(this.PrinterName))
            {
                RequestFiles files = new RequestFiles(this.Url);
                return await files.PrintStream(this.PrinterName, pagecount);
            }
            else return false;
        }

        public void PrintStream(Stream streamData)
        {
            PrintStream(streamData, this.Count);
        }

        public void PrintStream(Stream streamData, int pagecount)
        {
            if (streamData != null)
            {
                PrintPdf(PrinterName, pagecount, streamData);
                Console.WriteLine("printed stream data...");
            }
            else Console.WriteLine("no stream data to print...");
        }


        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.PrinterName = null;
                    this.Url = null;
                    this.FilePath = null;
                }

                disposedValue = true;
            }
        }


        private bool PrintPdf(string printer,int copies,Stream stream)
        {
            var Print = new PDFium.PdfiumPrinterHelper();
            return Print.PrintPDF(printer, copies, stream);
        }


        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~WindowsPrint()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
