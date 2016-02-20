using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SystemShutdown.Annotations;

namespace SystemShutdown.ViewModel {
    public class MainViewModel : INotifyPropertyChanged {

        public MainViewModel() {
            _isShutdownTimeSpanMode = true;
            _shutdownTime = DateTime.Now.AddHours(1).AddSeconds(1);
        }

        #region Zmienne
        /// <summary>
        /// Wybrany czas wyłączenia komputera
        /// </summary>
        private DateTime _shutdownTime;

        /// <summary>
        /// Przechowuje informację czy wybieramy godzinę podając ile czasu zostało do wyłączania komputera
        /// </summary>
        private bool _isShutdownTimeSpanMode;

        /// <summary>
        /// Przechowuje informacje czy system czeka do wyłączenia
        /// </summary>
        private bool _isShuttingDown;
        #endregion

        #region Właściwości
        /// <summary>
        /// Przedstawia czy system czeka do wyłączenia
        /// </summary>
        public bool IsShuttingDown {
            get { return _isShuttingDown; }
            set {
                _isShuttingDown = value;
                OnPropertyChanged(nameof(IsShuttingDown));
                OnPropertyChanged(nameof(IsEnabledWhileShuttingDown));
                OnPropertyChanged(nameof(ShutdownButtonText));
            }
        }

        /// <summary>
        /// Przedstawia informację czy kontrolka powinna być dezaktywowana przy wyłączaniu
        /// </summary>
        public bool IsEnabledWhileShuttingDown => !_isShuttingDown;

        /// <summary>
        /// Tekst przycisku zamykania/przerywania wyłączania komputera
        /// </summary>
        public string ShutdownButtonText => IsShuttingDown ? "Przerwij zamykanie komputera" : "Wyłącz komputer";

        /// <summary>
        /// Przedstawia czas do wyłączenia komputera
        /// </summary>
        public TimeSpan ShutdownTimeLeft {
            get { return _shutdownTime - DateTime.Now; }
            set {
                _shutdownTime = DateTime.Now.Add(value);
                OnPropertyChanged(nameof(ShutdownTimeLeft));
                OnPropertyChanged(nameof(FormattedShutdownTimeLeft));
                OnPropertyChanged(nameof(ShutdownTime));
            }
        }

        /// <summary>
        /// Przedstawia czas wyłączenia komputera
        /// </summary>
        public DateTime ShutdownTime {
            get { return _shutdownTime; }
            set {
                _shutdownTime = value;
                OnPropertyChanged(nameof(ShutdownTimeLeft));
                OnPropertyChanged(nameof(FormattedShutdownTimeLeft));
                OnPropertyChanged(nameof(ShutdownTime));
            }
        }

        /// <summary>
        /// Minimalna data wyłączenia komputera
        /// </summary>
        public DateTime MinimumShutdownDate => DateTime.Now.AddMinutes(1);

        /// <summary>
        /// Sformatowany czas do wyłączenia komputera
        /// </summary>
        public string FormattedShutdownTimeLeft => ShutdownTimeLeft.ToString(ShutdownTimeLeft.Days > 0 ? @"dd\.hh\:mm\:ss" : @"hh\:mm\:ss");

        /// <summary>
        /// Czy wybieramy godzinę podając ile czasu zostało do wyłączenia
        /// </summary>
        public bool IsShutdownTimeSpanMode {
            get { return _isShutdownTimeSpanMode; }
            set {
                _isShutdownTimeSpanMode = value;
                OnPropertyChanged(nameof(IsShutdownTimeSpanMode));
                OnPropertyChanged(nameof(IsShutdownTimeSpanVisible));
                OnPropertyChanged(nameof(IsShutdownTimePickerVisible));
            }
        }

        /// <summary>
        /// Czy wybieramy godzinę podając ją bezpośrednio
        /// </summary>
        public bool IsShutdownTimeMode {
            get { return !_isShutdownTimeSpanMode; }
            set {
                _isShutdownTimeSpanMode = !value;
                OnPropertyChanged(nameof(IsShutdownTimeMode));
                OnPropertyChanged(nameof(IsShutdownTimeSpanVisible));
                OnPropertyChanged(nameof(IsShutdownTimePickerVisible));
            }
        }

        /// <summary>
        /// Czy wybór godziny przez podanie czasu do wyłączenia ma być widoczny
        /// </summary>
        public Visibility IsShutdownTimeSpanVisible => IsShutdownTimeSpanMode ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Czy wybór godziny bezpośrednio ma być widoczny
        /// </summary>
        public Visibility IsShutdownTimePickerVisible => IsShutdownTimeMode ? Visibility.Visible : Visibility.Collapsed;
        #endregion

        #region Metody obsługujące ogólną zmianę właściwości
        /// <summary>
        /// Aktualizuje pozostały do wyłączenia czas
        /// </summary>
        public void UpdateTimeLeft() {
            OnPropertyChanged(nameof(ShutdownTimeLeft));
            OnPropertyChanged(nameof(FormattedShutdownTimeLeft));
        }

        /// <summary>
        /// Wydarzenie obsługujące zmianę właściwości
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
