using System.Windows;
using SystemShutdown.Model;
using SystemShutdown.ViewModel;

namespace SystemShutdown.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// ViewModel okna
        /// </summary>
        private readonly MainViewModel _viewModel;

        /// <summary>
        /// Obiekt do obsługi wyłączania komputera
        /// </summary>
        private readonly ShutdownControlModel _shutdownControl;

        /// <summary>
        /// Tworzy nowy obiekt głównego okna
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            _viewModel = (MainViewModel) DataContext;
            _shutdownControl = new ShutdownControlModel(this);
        }

        /// <summary>
        /// Kliknięcie w przycisk wyłączania komputera
        /// </summary>
        private void ShutdownComputerClick(object sender, RoutedEventArgs e) {
            if (_viewModel.IsShuttingDown)
                _shutdownControl.StopShutdown();
            else
                _shutdownControl.StartSystemShutdown(_viewModel.ShutdownTime, () => _viewModel.UpdateTimeLeft());

            _viewModel.IsShuttingDown = !_viewModel.IsShuttingDown;
        }
    }
}
