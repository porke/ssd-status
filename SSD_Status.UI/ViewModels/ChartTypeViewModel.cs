using SSD_Status.WPF.ViewModels.Sources;

namespace SSD_Status.WPF.ViewModels
{
    internal class ChartTypeViewModel
    {
        internal ChartTypeViewModel(ChartType type, string description)
        {
            Type = type;
            Description = description;
        }

        public ChartType Type { get; private set; }
        public string Description { get; private set; }
    }
}
