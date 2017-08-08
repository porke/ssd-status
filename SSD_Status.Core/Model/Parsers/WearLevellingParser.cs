using System;

namespace SSD_Status.Core.Model.Parsers
{
    internal class WearLevellingParser : IRecordParser
    {
        public byte AttributeId => 0xAD;

        public bool CanParse(byte id)
        {
            return id == AttributeId;
        }

        public double Parse(byte[] data, int offset)
        {
            return BitConverter.ToInt32(data, offset * 12 + 7);
        }
    }
}
