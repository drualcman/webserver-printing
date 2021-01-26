# webserver-printing
Local Web Server for can print in a local printer from Javascript to a preselected local printer without user interact. This tool is good when you need to print from your web app directly to a preselected printer.

## How to use
### From JAVASCRIPT or HTML FORM can send request to this server. Can use post, get or mix the 2 options on the request call like.
* ARGUMENTS (all optional)
* PROTOCOL: HTTTP or HTTPS
* SERVER: LOCALHOST
* PORT NUMBER: port number to use, default 8888, can send more than one

### PROPERTIES
* PRINTER = Printer Name to use, this is REQUIRED
* URL = Url to request a file to print, PDF, or any other format, but not HTML page.
* FILE = Full path about some LOCAL FILE on the machine. This file must be exist on the local computer request to print, not in a server
* COUNT = Number of copies for the document. Default always 1

### FUNCTIONS
printerlist = Get the printers installed on the computer

## Examples
```
//post data
var data = new FormData();
data.append("printer", "[printer name]");
data.append("url", "[url with a document to print]");           //if not url, send path
data.append("file", "[exact path with the file to print]");     //if not path, send url
data.append("count", "[number of copioes]");                    //optional. Default 1
```

### get data
* Get request with ```http://localhost:8888/printerlist```
* Get request with ``` http://localhost:8888?printer=[printer name]&url[url file to print]```
* Get request with ```http://localhost:8888?printer=[printer name]&url[url file to print]&count=3```
* Get request with ```http://localhost:8888?printer=[printer name]&file[full path file to print]```
* Get request with ```http://localhost:8888?printer=[printer name]&file[full path file to print]&count=3```

### Post request with url http://localhost:8888 and the form data

```
//post data
var data = new FormData();
data.append("printer", "[printer name]");
data.append("url", "[url with a document to print]");           //if url not send path
```

You can combine post data and get data. Property only can send once or in get variables or in post variables.

### Contributions from
RAW PRINT are used on this project from https://github.com/frogmorecs/RawPrint but with a small changes. Thanks to the owner

