using System.Linq;

namespace SSD_Status.WPF.Utilities
{
    internal static class SimpleMovingAverageCalculator
    {
        public static double CalculateMovingAverage(double[] input, uint periods)
        {
            return input.Take((int)periods).Average();
        }
    }
}
