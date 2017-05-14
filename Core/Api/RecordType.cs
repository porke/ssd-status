namespace SSD_Status.Core.Api
{
    public class RecordType
    {
        public RecordType(byte code, string name, UnitType unit)
        {
            SmartCode = code;
            Name = name;
            Unit = unit;
        }

        public byte SmartCode { get; }
        public string Name { get; }
        public UnitType Unit { get; }
    }
}
