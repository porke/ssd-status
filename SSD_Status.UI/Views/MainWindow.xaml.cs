using SSD_Status.WPF.Controllers;
using SSD_Status.WPF.ViewModels;
using System.Windows;

namespace SSD_Status.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainController _controller;

        public MainWindow()
        {
            InitializeComponent();

            var mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
            _controller = new MainController(mainViewModel);
        }
    }
}
