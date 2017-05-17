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
                decimal bytesWritten = smartEntry.Records.First(x => x.Type.Unit == UnitType.Byte).Value;
                decimal powerOnHours = smartEntry.Records.First(x => x.Type.Unit == UnitType.Hour).Value;
                decimal wearLevelling = smartEntry.Records.First(x => x.Type.Unit == UnitType.None).Value;
                file.WriteLine($"{dateString};{powerOnHours};{wearLevelling};{BytesToGigabytes(bytesWritten).ToString("0.##", CultureInfo.InvariantCulture)}");
            }
        }

        private static void PrintToConsole(Entry smartEntry)
        {
            System.Console.WriteLine($"Time: {smartEntry.Timestamp}");
            foreach (var record in smartEntry.Records)
            {
                switch (record.Type.Unit)
                {
                    case UnitType.Byte:                        
                        System.Console.WriteLine($"{record.Type.Name} {BytesToGigabytes(record.Value).ToString("0.##", CultureInfo.InvariantCulture)} GB");
                        break;
                    case UnitType.Hour:
                        System.Console.WriteLine($"{record.Type.Name} {record.Value} {record.Type.Unit}s");
                        break;
                    default:
                        System.Console.WriteLine($"{record.Type.Name} {record.Value}");
                        break;

                }                
            }
        }

        private static decimal BytesToGigabytes(decimal bytes)
        {
            const decimal byteToGigabyteDivisor = 1024 * 1024 * 1024;
            return bytes / byteToGigabyteDivisor;
        }
    }
}
