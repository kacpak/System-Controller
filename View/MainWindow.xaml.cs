using System.Windows;
using SystemShutdown.ViewModel;

namespace SystemShutdown.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly MainViewModel _viewModel;

        public MainWindow() {
            InitializeComponent();
            _viewModel = (MainViewModel) DataContext;
        }

        private void ShutdownModeClicked(object sender, RoutedEventArgs e) {
            //if (TimeSpanRadioButton.IsChecked != null && (bool) TimeSpanRadioButton.IsChecked)
            //    _viewModel.CurrentShutdownMode = MainViewModel.ShutdownMode.TimeSpan;
            //else
            //    _viewModel.CurrentShutdownMode = MainViewModel.ShutdownMode.Time;
        }
    }
}
