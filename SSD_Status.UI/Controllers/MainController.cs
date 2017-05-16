using SSD_Status.WPF.ViewModels;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
