using System;
using System.IO;

namespace RawPrint
{
    public class JobCreatedEventArgs : EventArgs
    {
        public uint Id { get; set; }
        public string PrinterName { get; set; }
    }

    public delegate void JobCreatedHandler(object sender, JobCreatedEventArgs e);

    public interface IPrinter
    {
        void PrintRawFile(string printer, string path);
        void PrintRawFile(string printer, string path, int pagecount);
        void PrintRawFile(string printer, string path, bool paused);
        void PrintRawFile(string printer, string path, bool paused, int pagecount);
        void PrintRawFile(string printer, string path, string documentName);
        void PrintRawFile(string printer, string path, string documentName, bool paused);
        void PrintRawStream(string printer, Stream stream, string documentName);
        void PrintRawStream(string printer, Stream stream, string documentName, int pagecount);
        void PrintRawStream(string printer, Stream stream, string documentName, bool paused);
        void PrintRawStream(string printer, Stream stream, string documentName, bool paused, int pagecount);

        event JobCreatedHandler OnJobCreated;
    }
}