using System;

namespace SSD_Status.Core.Model.Parsers
{
    internal class PercentLifetimeLeftParser : IRecordParser
    {
        public const byte AttributeId = 0xCA;

        public bool CanParse(byte id)
        {
            return id == AttributeId;
        }

        public double Parse(byte[] data, int offset)
        {
            long vendordata = BitConverter.ToInt32(data, offset * 12 + 7);
            return 100 - vendordata;
        }
    }
}
