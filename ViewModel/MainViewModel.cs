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
        #endregion

        #region Właściwości
        /// <summary>
        /// Przedstawia czas do wyłączenia komputera
        /// </summary>
        public TimeSpan ShutdownTimeLeft {
            get { return _shutdownTime - DateTime.Now; }
            set {
                _shutdownTime = DateTime.Now.Add(value);
                OnPropertyChanged(nameof(ShutdownTimeLeft));
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
                OnPropertyChanged(nameof(ShutdownTime));
            }
        }

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
            set
            {
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
        /// Wydarzenie obsługujące zmianę właściwości
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            //MessageBox.Show($"szhutdowntime: {_shutdownTime};  is time span {_isShutdownTimeSpanMode}");
        }
        #endregion
    }
}
