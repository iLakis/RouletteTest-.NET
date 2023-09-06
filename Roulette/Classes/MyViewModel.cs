using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace Roulette.Models {   
    public class MyViewModel : INotifyPropertyChanged {
        private ObservableCollection<string> _winningBets;
        private SolidColorBrush _winningBetColor = new SolidColorBrush(Colors.MediumAquamarine);
        private SolidColorBrush _winningNumberColor;
        public ObservableCollection<string> WinningBets {
            get => _winningBets;
            set {
                _winningBets = value;
                OnPropertyChanged(nameof(WinningBets));
            }
        }
        public SolidColorBrush WinningBetColor {
            get => _winningBetColor;
            set {
                _winningBetColor = value;
                OnPropertyChanged(nameof(WinningBetColor));
            }
        }
        public SolidColorBrush WinningNumberColor {
            get => _winningNumberColor;
            set {
                _winningNumberColor = value;
                OnPropertyChanged(nameof(WinningNumberColor));
            }
        }

        public MyViewModel() {
            WinningBets = new ObservableCollection<string>();
        }

        public void UpdateWinningBets(
            ObservableCollection<string> newData,
            SolidColorBrush newNumberColor,
            SolidColorBrush newBetColor) {
            WinningBets = new ObservableCollection<string>(newData);
            WinningNumberColor = newNumberColor;
            WinningBetColor = newBetColor;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
