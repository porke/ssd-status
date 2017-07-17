using SSD_Status.WPF.ViewModels.Enums;

namespace SSD_Status.WPF.Controllers.Converters
{
    internal sealed class RealTimeIntervalToSecondCountConverter
    {
        public static int Convert(RealTimeIntervalType value)
        {
            switch (value)
            {
                case RealTimeIntervalType.Seconds_15:
                    return 15;
                case RealTimeIntervalType.Minutes_1:
                    return 60;
                case RealTimeIntervalType.Minutes_5:
                    return 5 * 60;
                case RealTimeIntervalType.Minutes_15:
                    return 15 * 16;
                case RealTimeIntervalType.Hours_1:
                    return 3600;
                case RealTimeIntervalType.Hours_4:
                    return 4 * 3600;
                default:
                    return 60;
            }
        }
    }
}
