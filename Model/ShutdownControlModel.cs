using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace SystemShutdown.Model {
    public class ShutdownControlModel {

        /// <summary>
        /// Kontekst do odświeżania GUI
        /// </summary>
        private readonly Window _context;

        /// <summary>
        /// Licznik czasu
        /// </summary>
        private DispatcherTimer _timer;

        /// <summary>
        /// Przechowuje informacje czy program jest w procesie wyłączania komputera
        /// </summary>
        public bool IsShuttingDown => _timer != null && _timer.IsEnabled;

        /// <summary>
        /// Tworzy nowy obiekt wyłączania systemu
        /// </summary>
        /// <param name="context"></param>
        public ShutdownControlModel(Window context) {
            _context = context;
        }

        /// <summary>
        /// Wyłącza komputer po określonym czasie, poprzez uruchomienie procesu shutdown.exe
        /// </summary>
        /// <param name="shutdownDate">Moment wyłączenia komputera</param>
        /// <param name="guiUpdateAction">Akcja wykonywana przy odliczaniu do wyłączenia</param>
        public void StartSystemShutdown(DateTime shutdownDate, Action guiUpdateAction = null) {
            _timer = new DispatcherTimer();
            _timer.Tick += (sender, e) => {
                // Wykonaj akcję odświeżającą GUI
                _context.Dispatcher.Invoke(guiUpdateAction);

                // Sprawdź czy czas do wyłączenia już upłynął
                if (DateTime.Compare(DateTime.Now, shutdownDate) < 0)
                    return;

                // Wyłącz timer i komputer
                //MessageBox.Show("Shutdown");
                _timer.Stop();
                Process.Start("shutdown", "/s /t 0");
            };
            _timer.Interval = TimeSpan.FromMilliseconds(250);
            _timer.Start();
        }

        /// <summary>
        /// Zatrzymuje wątek wyłączania systemu i wysyła rozkaz GUI stopShutdown()
        /// </summary>
        public void StopShutdown() {
            _timer.Stop();
        }
    }
}
