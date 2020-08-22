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

//post data
var data = new FormData();
data.append("printer", "[printer name]");

Post request with url http://localhost:8888 and the form data
Get request with http://localhost:8888?printer=[printer name]

Printer property only can send once or in get variables or in post variables.

EXAMPLE

var data = new FormData();
data.append("url", "[url to request a file]");

$p.post('http://localhost:8888/?printer=[printer name]', function(data) {
                console.log(data);
            }, function(error) {
                console.log(error);
            }, data, ""); 

# Contributions from
RAW PRINT are used on this project from https://github.com/frogmorecs/RawPrint but with a small changes. Thanks to the owner

En este projecto se ha utilizado RAW PRINT desde https://github.com/frogmorecs/RawPrint pero con algunas pequeñas modificaciones. Gracias al creador.

