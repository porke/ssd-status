using SSD_Status.Core.Api;
using SSD_Status.WPF.ViewModels;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand LoadRawValuesCommand { get; private set; }

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;

            OpenFileCommand = new RelayCommand(OpenFileCommand_Execute);
            _viewModel.UsageStatsInfo.OpenFileCommand = OpenFileCommand;

            LoadRawValuesCommand = new RelayCommand(LoadRawValuesCommand_Execute);
        }

        private void LoadRawValuesCommand_Execute(object obj)
        {
            _viewModel.RawValueInfo.RawValues.Clear();
            Entry entry = ServiceLocator.SmartEntryReader.ReadAttributes();
            foreach (var record in entry.Records)
            {
                switch (record.Type.Unit)
                {
                    case UnitType.Gigabyte:
                        _viewModel.RawValueInfo.RawValues.Add($"{record.Type.Name} {record.Value.ToString("0.##", CultureInfo.InvariantCulture)} GB");
                        break;
                    case UnitType.Hour:
                        _viewModel.RawValueInfo.RawValues.Add($"{record.Type.Name} {record.Value} {record.Type.Unit}s");
                        break;
                    default:
                        _viewModel.RawValueInfo.RawValues.Add($"{record.Type.Name} {record.Value}");
                        break;

                }
            }            
        }

        private void OpenFileCommand_Execute(object obj)
        {
            using (var openFileDialog = new OpenFileDialog())
            { 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _viewModel.UsageStatsInfo.SourceDataFile = openFileDialog.FileName;

                    foreach (var line in File.ReadAllLines(openFileDialog.FileName).Skip(1))
                    {
                        var entries = line.Split(';');
                        DateTime timestamp = DateTime.Parse(entries[0]);
                        int powerOnHours = int.Parse(entries[1]);
                        int wearLevelling = int.Parse(entries[2]);
                        double writtenGb = double.Parse(entries[3], CultureInfo.InvariantCulture);
                    }
                }
            }
        }
    }
}
