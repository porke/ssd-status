using System;
using System.Linq;

namespace SSD_Status.Core.Model.Parsers
{
    internal class WrittenGigabytesParser : IRecordParser
    {
        public const byte AttributeId = 0xF6;

        public string Description => "Written gigabytes";

        public bool CanParse(byte id)
        {
            return AttributeId == id;
        }

        public double Parse(byte[] data, int offset)
        {
            var fieldBytes = data.Skip(offset * 12 + 7)
                                 .Take(6)
                                 .ToList();
            fieldBytes.Add(0);
            fieldBytes.Add(0);
            
            long writtenSectors = BitConverter.ToInt64(fieldBytes.ToArray(), 0);
            const int sectorSizeInBytes = 512;            
            return BytesToGigabytes(writtenSectors * sectorSizeInBytes);            
        }

        private static double BytesToGigabytes(double bytes)
        {
            const double byteToGigabyteDivisor = 1024 * 1024 * 1024;
            return bytes / byteToGigabyteDivisor;
        }
    }
}
