using SSD_Status.Core.Api;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SSD_Status.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var entry = ServiceLocator.SmartEntryReader.ReadAttributes();

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

        private static void AppendToFile(Entry smartEntry, string path)
        {
            using (var file = new StreamWriter(File.Open(path, FileMode.Append)))
            {                
                string dateString = smartEntry.Timestamp.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                double gigabytesWritten = smartEntry.Records.First(x => x.Type.Unit == UnitType.Gigabyte).Value;
                double powerOnHours = smartEntry.Records.First(x => x.Type.Unit == UnitType.Hour).Value;
                double wearLevelling = smartEntry.Records.First(x => x.Type.Unit == UnitType.None).Value;
                file.WriteLine($"{dateString};{powerOnHours};{wearLevelling};{(gigabytesWritten).ToString("0.##", CultureInfo.InvariantCulture)}");
            }
        }

        private static void PrintToConsole(Entry smartEntry)
        {
            System.Console.WriteLine(smartEntry.Description);
            System.Console.WriteLine(smartEntry);            
        }
    }
}
