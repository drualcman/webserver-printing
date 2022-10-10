using System.Collections.Generic;
using System.Drawing.Printing;

namespace WerServer.Handlers;

public class WindowsManagement
{
    public static List<string> PopulateInstalledPrinters()
    {
        // Add list of installed printers found to the combo box.
        // The pkInstalledPrinters string will be used to provide the display string.
        List<string> printers = new List<string>();
        foreach (string item in PrinterSettings.InstalledPrinters)
        {
            printers.Add(item);
        }
        return printers;
    }
}
