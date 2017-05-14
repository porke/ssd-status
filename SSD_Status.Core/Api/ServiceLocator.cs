using SSD_Status.Core.Implementation;

namespace SSD_Status.Core.Api
{
    public static class ServiceLocator
    {
        public static IRecordReader RecordReader
        {
            get
            {
                return new RecordReader();
            }
        }
    }
}
