using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WerServer.Handlers;

public class RequestFiles
{
    public Uri Url { get; private set; }

    public RequestFiles(Uri url)
    {
        Url = url;
    }

    public RequestFiles(string url)
    {
        try
        {
            Url = new Uri(url);
        }
        catch (Exception ex)
        {
            throw new Exception("Url not valid", ex);
        }
    }

    public async Task<bool> PrintStream(string printerName)
    {
        bool result;
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage responseClient = await client.GetAsync(Url))
            {
                if (responseClient.IsSuccessStatusCode)
                {
                    Stream streamFile = await responseClient.Content.ReadAsStreamAsync();
                    PrintPdf(printerName, 1, streamFile);
                    Console.WriteLine($"printed stream data from {Url}...");
                    result = true;
                }
                else
                    result = false;
            }
        }
        return result;
    }

    public async Task<bool> PrintStream(string printerName, int pagecount)
    {
        bool result;
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage responseClient = await client.GetAsync(Url))
            {
                if (responseClient.IsSuccessStatusCode)
                {
                    Stream streamFile = await responseClient.Content.ReadAsStreamAsync();
                    result = PrintPdf(printerName, 1, streamFile);
                    Console.WriteLine($"printed stream data from {Url}...");
                }
                else
                    result = false;
            }
        }
        return result;
    }


    private bool PrintPdf(string printer, int copies, Stream stream)
    {
        var Print = new Helpers.PrintHelper();
        return Print.PrintPDF(printer, copies, stream);
    }

}
