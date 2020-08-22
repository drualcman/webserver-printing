using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using RawPrint;

namespace WerServer
{
    public class RequestFiles
    {
        public Uri Url { get; private set; }

        public RequestFiles(Uri url)
        {
            this.Url = url;
        }

        public RequestFiles(string url)
        {
            try
            {
                this.Url = new Uri(url);
            }
            catch (Exception ex)
            {
                throw new Exception("Url not valid", ex);
            }
        }

        public async Task<bool> PrintStream(string printerName, IPrinter printer)
        {
            bool result;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseClient = await client.GetAsync(this.Url))
                {
                    if (responseClient.IsSuccessStatusCode)
                    {
                        Stream streamFile = await responseClient.Content.ReadAsStreamAsync();
                        printer.PrintRawStream(printerName, streamFile, "Web Server Raw Print");
                        Console.WriteLine($"printed stream data from {this.Url}...");
                        result = true;
                    }
                    else
                        result = false;
                }
            }
            return result;
        }
    }
}
