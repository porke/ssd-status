using SSD_Status.Core.Api;
using System.Collections.Generic;

namespace SSD_Status.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var records = ServiceLocator.RecordReader.GetRecords();

            if (args.Length == 1)
            {
                AppendToFile(records, "test.csv");
            }
            else
            {
                PrintToConsole(records); 
            }
        }

        private static void AppendToFile(IReadOnlyList<Record> records, string filename)
        {
            // TODO: implement output
        }

        private static void PrintToConsole(IReadOnlyList<Record> records)
        {
            foreach (var record in records)
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
