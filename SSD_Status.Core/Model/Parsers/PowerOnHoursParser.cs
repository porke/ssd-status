using System;

namespace SSD_Status.Core.Model.Parsers
{
    internal class PowerOnHoursParser : IRecordParser
    {
        public const byte AttributeId = 0x09;

        public string Description => "Power on hours";

        public bool CanParse(byte id)
        {
            return AttributeId == id;
        }

        public double Parse(byte[] data, int offset)
        {
            return BitConverter.ToInt32(data, offset * 12 + 7);            
        }
    }
}
