using SSD_Status.Core.Model;
using SSD_Status.WPF.Controllers.Chart;
using SSD_Status.WPF.ViewModels;
using SSD_Status.WPF.ViewModels.Sources;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers
{
    public class ChartController
    {
        private ChartViewModel _viewModel;

        private Dictionary<ChartType, IChartDataSelector> _dataSelectors = new Dictionary<ChartType, IChartDataSelector>();

        internal ChartController(ChartViewModel chartViewModel)
        {
            _viewModel = chartViewModel;

            _dataSelectors.Add(ChartType.None, new NoneSelector());
            _dataSelectors.Add(ChartType.HostWrittenGbInTime, new HostWritesSelector());
            _dataSelectors.Add(ChartType.HostWrittenGbPerPowerOnHoursInTime, new HostWritesPerHoursOnSelector());
            _dataSelectors.Add(ChartType.WearLevellingInTime, new WearLevellingSelector());
            _dataSelectors.Add(ChartType.PowerOnHoursInTime, new PowerOnHoursSelector());
        }

        internal void SelectChartData(IReadOnlyList<SmartDataEntry> records, ChartTypeViewModel chartTypeVm)
        {
            _viewModel.UsageValues.Clear();

            IChartDataSelector selector = _dataSelectors[chartTypeVm.Type];
            _viewModel.YAxisTitle = selector.YAxisDescription;
            _viewModel.Title = chartTypeVm.Description;
            _viewModel.UsageValues.AddRange(selector.SelectData(records));
        }
    }
}
