using System;
using SSD_Status.Core.Api;

namespace SSD_Status.Core.Implementation.Parsers
{
    internal class WearLevellingParser : IRecordParser
    {
        private const byte AttributeId = 0xAD;

        public string Description => "Wear levelling";

        public bool CanParse(byte id)
        {
            return id == AttributeId;
        }

        public Record Parse(byte[] data, int offset)
        {
            long vendordata = BitConverter.ToInt32(data, offset * 12 + 7);
            return new Record
            {
                Value = vendordata,
                Type = new RecordType(AttributeId, Description, UnitType.None),
            };
        }
    }
}
