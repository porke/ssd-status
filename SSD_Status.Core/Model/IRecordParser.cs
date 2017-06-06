namespace SSD_Status.Core.Model
{
    interface IRecordParser
    {
        bool CanParse(byte id);
        double Parse(byte[] data, int offset);
    }
}
