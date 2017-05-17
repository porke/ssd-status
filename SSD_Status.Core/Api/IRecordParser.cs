namespace SSD_Status.Core.Api
{
    public interface IRecordParser
    {
        string Description { get; }

        Record Parse(byte[] data, int offset);
        bool CanParse(byte id);
    }
}
