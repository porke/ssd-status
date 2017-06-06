using SSD_Status.Core.Model;
using System.Globalization;
using System.IO;

namespace SSD_Status.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var entry = new SsdDrive().ReadSmartAttributes();

            if (args.Length == 1)
            {
                string path = args[0].Replace("\"", "").Split('=')[1];
                AppendToFile(entry, path);
            }
            else
            {
                PrintToConsole(entry); 
            }
        }

        private static void AppendToFile(DataEntry smartEntry, string path)
        {
            using (var file = new StreamWriter(File.Open(path, FileMode.Append)))
            {                
                string dateString = smartEntry.Timestamp.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                double gigabytesWritten = smartEntry.HostWrittenGb;
                double powerOnHours = smartEntry.PowerOnHours;
                double wearLevelling = smartEntry.WearLevellingCount;
                file.WriteLine($"{dateString};{powerOnHours};{wearLevelling};{(gigabytesWritten).ToString("0.##", CultureInfo.InvariantCulture)}");
            }
        }

        private static void PrintToConsole(DataEntry smartEntry)
        {
            System.Console.WriteLine(smartEntry.ToString());
            System.Console.WriteLine(smartEntry);            
        }
    }
}
