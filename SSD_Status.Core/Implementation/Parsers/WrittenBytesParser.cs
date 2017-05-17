using System;
using System.Linq;
using SSD_Status.Core.Api;


namespace SSD_Status.Core.Implementation.Parsers
{
    internal class WrittenBytesParser : IRecordParser
    {
        private readonly byte AttributeId = 0xF6;

        public string Description => "Written bytes";

        public bool CanParse(byte id)
        {
            return AttributeId == id;
        }

        public Record Parse(byte[] data, int offset)
        {
            var fieldBytes = data.Skip(offset * 12 + 7)
                                 .Take(6)
                                 .ToList();
            fieldBytes.Add(0);
            fieldBytes.Add(0);
            
            long writtenSectors = BitConverter.ToInt64(fieldBytes.ToArray(), 0);
            const int sectorSizeInBytes = 512;
            return new Record
            {
                Value = writtenSectors * sectorSizeInBytes,
                Type = new RecordType(0, Description, UnitType.Byte),
            };
        }
    }
}
