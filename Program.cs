using System;
using System.Collections.Generic;
using System.Linq;
using WerServer.Handlers;

namespace WerServer;

class Program
{
    static void Main(string[] args)
    {
        string help = @"Copyright (c) Frogmore Computer Services Ltd
Copyright (c) 2020 DrUalcman Programación

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ""AS IS"" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
            LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
            ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
            (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.

From JAVASCRIPT or HTML FORM can send request to this server. Can use post, get or mix the 2 options on the request call like.
ARGUMENTS (all optional)
PROTOCOL: HTTTP or HTTPS
SERVER: LOCALHOST
PORT NUMBER: port number to use, default 8888, can send more than one

PROPERTIES
PRINTER = Printer Name to use, this is REQUIRED
URL = Url to request a file to print, PDF, or any other format, but not HTML page.
FILE = Full path about some LOCAL FILE on the machine. This file must be exist on the local computer request to print, not in a server
COUNT = Number of copies for the document. Default always 1

FUNCTIONS
printerlist = Get the printers installed on the computer

//post data
var data = new FormData();
data.append(""printer"", ""[printer name]"");
data.append(""url"", ""[url with a document to print]"");           //if url not send path
data.append(""file"", ""[exact path with the file to print]"");     //if path not send url
data.append(""count"", ""[number of copioes]"");                    //default 1 if it´s not send

Get request with http://localhost:8888?printer=[printer name]&url[url file to print]
Get request with http://localhost:8888?printer=[printer name]&url[url file to print]&count=3
Get request with http://localhost:8888?printer=[printer name]&file[full path file to print]
Get request with http://localhost:8888?printer=[printer name]&file[full path file to print]&count=3

Post request with url http://localhost:8888 and the form data

//post data
var data = new FormData();
data.append(""printer"", ""[printer name]"");
data.append(""url"", ""[url with a document to print]"");           //if url not send path

You can combine post data and get data. Property only can send once or in get variables or in post variables.
";
        Console.WriteLine(help);

        WebServer web;
        if(args.Length > 0)
        {
            string http = "http";
            string server = "*";
            List<int> ports = new List<int>();
            List<string> prefixes = new List<string>();
            foreach(string item in args)
            {
                Uri test;
                if(Uri.TryCreate(item, UriKind.Absolute, out test))
                {
                    if(item.IndexOf(":", item.IndexOf("://") + 1) < 0)
                    {
                        prefixes.Add(item + ":8888/");
                    }
                    else
                    {
                        if(item.IndexOf("http") < 0)
                        {
                            if(item.Substring(item.Length - 1, 1) == "/") prefixes.Add("http://" + item);
                            else prefixes.Add("http://" + item + "/");
                        }
                        else
                        {
                            if(item.Substring(item.Length - 1, 1) == "/") prefixes.Add(item);
                            else prefixes.Add(item + "/");
                        }
                    }
                }
                else
                {
                    int port;
                    if(int.TryParse(item, out port)) ports.Add(port);
                    else
                    {
                        if(item.IndexOf("http") < 0) server = item;
                        else http = item.Replace("://", "") + "://";
                    }
                }
            }
            if(ports.Count() > 0)
            {
                foreach(int p in ports)
                {
                    prefixes.Add(http + server.Replace(":", "") + ":" + p.ToString() + "/");
                }
            }
            else prefixes.Add(http + server.Replace(":", "") + ":8888/");
            web = new WebServer(prefixes);
        }
        else
            web = new WebServer();

        Console.WriteLine("Press CTRL+C to exit...");
    }
}
