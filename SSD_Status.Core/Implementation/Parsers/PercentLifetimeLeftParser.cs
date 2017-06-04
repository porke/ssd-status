using System;
using SSD_Status.Core.Api;

namespace SSD_Status.Core.Implementation.Parsers
{
    internal class PercentLifetimeLeftParser : IRecordParser
    {
        private const byte AttributeId = 0xCA;
        public string Description => "Percent lifetime left";

        public bool CanParse(byte id)
        {
            return id == AttributeId;
        }

        public Record Parse(byte[] data, int offset)
        {
            long vendordata = BitConverter.ToInt32(data, offset * 12 + 7);
            return new Record
            {
                Value = 100 - vendordata,
                Type = new RecordType(Description, UnitType.Percent),
            };
        }
    }
}
