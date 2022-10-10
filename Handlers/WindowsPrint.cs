using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WerServer.Models;

namespace WerServer.Handlers;

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
                PrinterName = printerName;
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
            Url = null;
        }
        try
        {
            FilePath = requestData.Find(x => x.Variable.ToLower() == "file").Valor;
        }
        catch
        {
            FilePath = string.Empty;
        }
        try
        {
            Count = Convert.ToInt32(requestData.Find(x => x.Variable.ToLower() == "count").Valor);
        }
        catch
        {
            Count = 1;
        }
    }

    public void SetUrl(string url)
    {
        try
        {
            Url = new Uri(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Url not valid", ex);
            throw new Exception("Url not valid", ex);
        }
    }

    public async Task<bool> PrintUrl()
    {
        return await PrintUrl(Count);
    }

    public async Task<bool> PrintUrl(int pagecount)
    {
        if (!string.IsNullOrEmpty(PrinterName))
        {
            RequestFiles files = new RequestFiles(Url);
            return await files.PrintStream(PrinterName, pagecount);
        }
        else return false;
    }

    public void PrintStream(Stream streamData)
    {
        PrintStream(streamData, Count);
    }

    public void PrintStream(Stream streamData, int pagecount)
    {
        if (streamData != null)
        {
            Console.WriteLine($"printed stream data result: {PrintPdf(PrinterName, pagecount, streamData)}");
        }
        else Console.WriteLine("no stream data to print...");
    }

    private bool PrintPdf(string printer, int copies, Stream stream)
    {
        var Print = new Helpers.PrintHelper();
        return Print.PrintPDF(printer, copies, stream);
    }

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                PrinterName = null;
                Url = null;
                FilePath = null;
            }

            disposedValue = true;
        }
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
