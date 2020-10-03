# webserver-printing
Local Web Server for can print in a local printer from Javascript to a preselected local printer without user interact. This tool is good when you need to print from your web app directly to a preselected printer.

Servidor Web Local para poder imprimir desde Javascript a una impresora local sin seleccion por parte del ususario. Esta utilidad es buena para cuando necesitas imprimir desde su app web a una impresora predefinida.

# How to use
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

You can combine post data and get data. Printer property only can send once or in get variables or in post variables.

EXAMPLE TO USE
        var dat = new FormData();
        dat.append("url", "http://localhost/dat/Error.pdf");

        function POST(url, success, error, data) {
            let xhr;            //control compatibilities
            if (window.XMLHttpRequest) {
                xhr = new XMLHttpRequest();
            }
            else {
                xhr = new ActiveXObject("Microsoft.XMLHTTP");
            }
            let type = "POST";
            let content = "application/json; charset=utf-8";
            xhr.open(type, url, true);
            xhr.setRequestHeader("Content-Type", content);
            xhr.onload = function () {
                if (xhr.status >= 200 && xhr.status <= 299) {
                    if (success !== undefined && success !== null) ExternalFunc(success, dat);
                }
                else {
                    if (error !== undefined && error !== null) ExternalFunc(error, xhr.statusText); 
                }
            };
            xhr.onerror = function (e) {
                console.error(e);
                if (error !== undefined && error !== null) ExternalFunc(error, e);
            };
            xhr.ontimeout = function (t) {
                console.error(t);
                if (error !== undefined && error !== null) ExternalFunc(error, t);
            };
            if (data === null || data === undefined || data === '') xhr.send();
            else xhr.send(data);
        }

        POST('http://localhost:8888/?printer=Microsoft Print to PDF', function (data) {
            console.log(data);
        }, function (error) {
                console.log(error);
        }, dat);

# Contributions from
RAW PRINT are used on this project from https://github.com/frogmorecs/RawPrint but with a small changes. Thanks to the owner

En este projecto se ha utilizado RAW PRINT desde https://github.com/frogmorecs/RawPrint pero con algunas pequeñas modificaciones. Gracias al creador.

