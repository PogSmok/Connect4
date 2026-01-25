using Connect4.Services;
using Connect4.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace Connect4.ViewModels
{
    /// <summary>
    /// ViewModel for the main menu of the Connect 4 application.
    /// </summary>
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;

        /// <summary>
        /// Navigates to the game settings screen.
        /// </summary>
        public ICommand PlayCommand { get; }

        /// <summary>
        /// Navigates to db view screen
        /// </summary>
        public ICommand MyGamesCommand { get; }


        private bool _isSoundOff = false;
        public bool IsSoundOff
        {
            get => _isSoundOff;
            set
            {
                if (_isSoundOff != value)
                {
                    _isSoundOff = value;
                    OnPropertyChanged(nameof(IsSoundOff));
                    if (!_isSoundOff) SoundService.PlayBackgroundMusic();
                    else SoundService.StopBackgroundMusic();
                }
            }
        }

        /// <summary>
        /// Toggles music on and off
        /// </summary> 
        public ICommand ToggleSoundCommand { get; }

        /// <summary>
        /// Quits the application when executed.
        /// </summary>
        public ICommand QuitCommand { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="MainMenuViewModel"/>.
        /// Sets up the commands used in the main menu.
        /// </summary>
        public MainMenuViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            PlayCommand = new RelayCommand(_ => _navigationService.Navigate(new GameSettingsView(_navigationService)));
            MyGamesCommand = new RelayCommand(_ => _navigationService.Navigate(new DbView(_navigationService)));
            ToggleSoundCommand = new RelayCommand(_ => IsSoundOff = !IsSoundOff);
            QuitCommand = new RelayCommand(_ => QuitApplication());
        }

        /// <summary>
        /// Shuts down the current WPF application.
        /// </summary>
        private void QuitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
