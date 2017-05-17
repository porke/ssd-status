using System;
using SSD_Status.Core.Api;

namespace SSD_Status.Core.Implementation.Parsers
{
    internal class PowerOnHoursParser : IRecordParser
    {
        private const byte AttributeId = 0x09;

        public string Description => "Power on hours";

        public bool CanParse(byte id)
        {
            return AttributeId == id;
        }

        public Record Parse(byte[] data, int offset)
        {
            long vendordata = BitConverter.ToInt32(data, offset * 12 + 7);
            return new Record
            {
                Value = vendordata,
                Type = new RecordType(AttributeId, Description, UnitType.Hour),
            };
        }
    }
}
