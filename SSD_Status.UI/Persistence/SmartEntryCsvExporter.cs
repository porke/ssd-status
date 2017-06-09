using SSD_Status.Core.Model;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SSD_Status.WPF.Persistence
{
    internal class SmartEntryCsvExporter
    {
        internal void ExportSmartEntries(string filename, IReadOnlyList<SmartDataEntry> entries)
        {
            using (var file = new StreamWriter(File.OpenWrite(filename)))
            {
                file.WriteLine("Timestamp;HostWrittenGb");
                foreach (var entry in entries)
                {
                    file.WriteLine($"{entry.Timestamp.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}" +
                                   $";{entry.HostWrittenGb.ToString("0.##", CultureInfo.InvariantCulture)}");
                }
            }
        }
    }
}
