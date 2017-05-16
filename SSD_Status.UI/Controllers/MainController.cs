using SSD_Status.Core.Api;
using SSD_Status.WPF.ViewModels;
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
            Entry entry = ServiceLocator.SmartEntryReader.ReadAttributes();
            foreach (var record in entry.Records)
            {
                _viewModel.RawValueInfo.RawValues.Add($"{record.Type.Name} {record.Value}");
            }
        }

        private void OpenFileCommand_Execute(object obj)
        {
            using (var openFileDialog = new OpenFileDialog())
            { 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _viewModel.UsageStatsInfo.SourceDataFile = openFileDialog.FileName;
                }
            }
        }
    }
}
