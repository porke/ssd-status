namespace SSD_Status.Core.Api
{
    public class RecordType
    {
        public RecordType(string name, UnitType unit)
        {
            Name = name;
            Unit = unit;
        }
        
        public string Name { get; }
        public UnitType Unit { get; }
    }
}
