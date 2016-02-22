using System;
using System.Windows;
using System.Windows.Forms;
using SystemShutdown.Model;
using SystemShutdown.ViewModel;
using static System.Drawing.Icon;
using static System.Reflection.Assembly;

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
        /// Ikona powiadomień
        /// </summary>
        private readonly NotifyIcon _notifyIcon;

        /// <summary>
        /// Tworzy nowy obiekt głównego okna
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            _viewModel = (MainViewModel) DataContext;
            _shutdownControl = new ShutdownControlModel(this);
            _notifyIcon = new NotifyIcon {
                Visible = true,
                Text = Title,
                Icon = ExtractAssociatedIcon(GetEntryAssembly().ManifestModule.Name),
                BalloonTipIcon = ToolTipIcon.Info
            };
            _notifyIcon.Click += (s, a) => WindowState = WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        }

        /// <summary>
        /// Wyświetla powiadomienie
        /// </summary>
        /// <param name="text">Treść powiadomienia</param>
        /// <param name="title">Tytuł powiadomienia</param>
        /// <param name="timeout">Długość wyświetlania powiadomienia</param>
        private void ShowNotification(string title = null, string text = null, int timeout = 1500) {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = text;
            _notifyIcon.ShowBalloonTip(timeout);
        }

        /// <summary>
        /// Kliknięcie w przycisk wyłączania komputera
        /// </summary>
        private void ShutdownComputerClick(object sender, RoutedEventArgs e) {
            if (_viewModel.IsShuttingDown)
                _shutdownControl.StopShutdown();
            else
                _shutdownControl.StartSystemShutdown(_viewModel.ShutdownTime, ShuttingDownUpdate);

            _viewModel.IsShuttingDown = !_viewModel.IsShuttingDown;
        }

        /// <summary>
        /// Aktualizacja interfejsu przy uruchomionym odliczaniu do wyłączenia systemu
        /// </summary>
        private void ShuttingDownUpdate() {
            _viewModel.UpdateTimeLeft();

            // Czas pozostały do wyłączenia komputera
            var t = _viewModel.ShutdownTimeLeft;

            // Komunikat co pół godziny, co 5 minut jeśli zostało mniej niż 15 minut, co minutę jeśli jest mniej niż 5 minut
            if (t.Seconds == 0 && (t.Minutes % 30 == 0 || t.Hours == 0 && (t.Minutes <= 15 && t.Minutes % 5 == 0 || t.Minutes <= 5))) {

                // Jeśli komunikat był w przeciągu ostatnich 2 sekund - nie pokazuj go
                if ((_timeLeftOnLastShutdownNotification - t).Duration() <= TimeSpan.FromSeconds(2)) return;

                // Wyświetl powiadomienie
                ShowNotification("Czas do wyłączenia", _viewModel.FormattedShutdownTimeLeft);

                // Zapamiętaj czas do wyłączenia
                _timeLeftOnLastShutdownNotification = t;
            }
        }

        /// <summary>
        /// Czas jaki pozostał do wyłączenia przy ostanim powiadomieniu
        /// </summary>
        private TimeSpan _timeLeftOnLastShutdownNotification;

        /// <summary>
        /// Zamknięcie aplikacji
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            _notifyIcon.Dispose();
        }
    }
}
