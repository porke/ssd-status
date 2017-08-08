namespace SSD_Status.Core.Model
{
    interface IRecordParser
    {
        byte AttributeId { get; }

        bool CanParse(byte id);
        double Parse(byte[] data, int offset);
    }
}
