using SSD_Status.Core.Api;
using System.Collections.Generic;

namespace SSD_Status.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var entry = ServiceLocator.RecordReader.ReadAttributes();

            if (args.Length == 1)
            {
                AppendToFile(entry, "test.csv");
            }
            else
            {
                PrintToConsole(entry); 
            }
        }

        private static void AppendToFile(Entry smartEntry, string filename)
        {
            // TODO: implement output
        }

        private static void PrintToConsole(Entry smartEntry)
        {
            System.Console.WriteLine($"Time: {smartEntry.Timestamp}");
            foreach (var record in smartEntry.Records)
            {
                switch (record.Type.Unit)
                {
                    case UnitType.Byte:
                        const decimal byteToGigabyteDivisor = 1024 * 1024 * 1024;
                        System.Console.WriteLine($"{record.Type.Name} {(record.Value / byteToGigabyteDivisor).ToString("0.##")} GB");
                        break;
                    case UnitType.Hour:
                        System.Console.WriteLine($"{record.Type.Name} {record.Value} {record.Type.Unit}s");
                        break;
                    default:
                        System.Console.WriteLine($"{record.Type.Name} {record.Value}");
                        break;

                }                
            }

            System.Console.ReadKey();
        }
    }
}
