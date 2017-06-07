using System.Linq;
using SSD_Status.Core.Model;
using SSD_Status.WPF.Controllers.Chart;
using SSD_Status.WPF.ViewModels;
using SSD_Status.WPF.ViewModels.Sources;
using System.Collections.Generic;
using System.Globalization;

namespace SSD_Status.WPF.Controllers
{
    public class HistoricalUsageStatsController
    {
        private ChartViewModel _viewModel;

        private Dictionary<ChartType, IChartDataSelector> _dataSelectors = new Dictionary<ChartType, IChartDataSelector>();

        internal HistoricalUsageStatsController(ChartViewModel chartViewModel)
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
            _viewModel.SeriesValues.Clear();
            _viewModel.Timestamps.Clear();

            IChartDataSelector selector = _dataSelectors[chartTypeVm.Type];
            _viewModel.YAxisTitle = selector.YAxisDescription;
            _viewModel.SeriesTitle = chartTypeVm.Description;

            var selectedData = selector.SelectData(records);
            _viewModel.Minimum = selectedData.Any() ? selectedData.Select(x => x.Value).Min() : 0;
            _viewModel.Maximum = selectedData.Any() ? selectedData.Select(x => x.Value).Max() : 1;
            _viewModel.Timestamps.AddRange(selectedData.Select(x => x.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            _viewModel.SeriesValues.AddRange(selectedData.Select(x => x.Value));
        }
    }
}
