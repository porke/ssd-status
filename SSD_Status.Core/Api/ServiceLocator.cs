using SSD_Status.Core.Implementation;

namespace SSD_Status.Core.Api
{
    public static class ServiceLocator
    {
        public static ISmartReader RecordReader
        {
            get
            {
                return new SmartReader();
            }
        }
    }
}
