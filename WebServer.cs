using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace WerServer
{
    public class WebServer
    {
        static HttpListener listener;
        private Thread listenThread1;

        public WebServer()
        {
            List<string> prefixes = new List<string>() { "http://*:8888/" };
            Start(prefixes);
        }

        public WebServer(List<string> prefixes)
        {
            Start(prefixes);
        }

        public void Start(List<string> prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            }

            // Create a listener.
            listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
                Console.WriteLine($"server on {s}...");
            }
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            listener.Start();
            listenThread1 = new Thread(new ParameterizedThreadStart(startlistener));
            listenThread1.Start();
            Console.WriteLine("Listening...");
        }

        private  void startlistener(object s)
        {
            while (true)
            {
                ////blocks until a client has connected to the server
                ProcessRequest();
            }
        }

        private  void ProcessRequest()
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }

        private async void ListenerCallback(IAsyncResult result)
        {
            HttpListenerContext context = listener.EndGetContext(result);
            Uri uri = context.Request.Url;
            Console.WriteLine($"Revived request for {uri}");

            List<DataFormat> dataValues = new List<DataFormat>();
            //get data from body
            string cleaned_data;
            if (context.Request.HasEntityBody)
            {                
                string data_text = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
                dataValues.AddRange(RequestData.GetValues(System.Web.HttpUtility.UrlDecode(data_text)));
            }
            else cleaned_data = string.Empty;            

            //get data form url
            dataValues.AddRange(RequestData.GetValues(context.Request.QueryString));
            cleaned_data = Helpers.ToJson(dataValues);

            // Get a response stream and write the response to it.
            HttpListenerResponse response = context.Response;
            response.StatusCode = 200;
            response.StatusDescription = "OK";
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            response.AddHeader("Access-Control-Max-Age", "1000");
            response.AddHeader("Access-Control-Allow-Header", "Content-Type");
            response.ContentType = "application/json; charset=utf-8";
            //append the data response
            byte[] buffer;
            Stream output = new MemoryStream();
            Console.WriteLine("setup completed...");
            try
            {
                switch (uri.LocalPath.ToLower())
                {
                    case "/printerlist":
                    case "printerlist":
                        //get the printer list to show
                        Console.WriteLine("Get printer List...");
                        buffer = Encoding.ASCII.GetBytes(Helpers.ToJson(WindowsManagement.PopulateInstalledPrinters()));
                        response.ContentLength64 = buffer.Length;
                        output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        break;
                    default:
                        Console.WriteLine("Print document...");
                        WindowsPrint winPrint = new WindowsPrint(dataValues);
                        bool print = await winPrint.PrintUrl();
                        if (print)
                        {
                            Console.WriteLine("printed...");
                            buffer = Encoding.ASCII.GetBytes(Helpers.ToJson(print));
                        }
                        else
                        {
                            Console.WriteLine("error on print a document...");
                            buffer = Encoding.ASCII.GetBytes(Helpers.ToJson(print, "Error on print a document"));
                        }
                        winPrint.Dispose();                        
                        response.ContentLength64 = buffer.Length;
                        output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        // must close the output stream.
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("we can't print, no printer defined...", ex);
                buffer = Encoding.ASCII.GetBytes(Helpers.ToJson(false, ex.Message));
                response.ContentLength64 = buffer.Length;
                output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                // must close the output stream.
                output.Close();
            }
            context.Response.Close();
        }
    }
}
