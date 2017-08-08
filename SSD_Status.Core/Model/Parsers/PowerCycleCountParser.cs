using System;

namespace SSD_Status.Core.Model.Parsers
{
    internal class PowerCycleCountParser : IRecordParser
    {
        public byte AttributeId => 0x0C;

        public bool CanParse(byte id)
        {
            return id == AttributeId;
        }

        public double Parse(byte[] data, int offset)
        {
            long vendordata = BitConverter.ToInt32(data, offset * 12 + 7);
            return vendordata;
        }
    }
}
