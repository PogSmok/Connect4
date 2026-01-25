using Connect4.Models;
using Connect4.Services;
using Connect4.Views;
using System.Windows.Input;

namespace Connect4.ViewModels
{
    /// <summary>
    /// ViewModel for the game settings of the Connect 4 application.
    /// </summary>
    public class GameSettingsViewModel
    {
        private readonly NavigationService _navigationService;
        public GameSettingsModel GameSettings { get; set; }

        /// <summary>
        /// Starts the game with chosen game settings
        /// </summary>
        public ICommand PlayCommand { get; }

        /// <summary>
        /// Goes back to main menu
        /// </summary>
        public ICommand BackCommand { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="MainMenuViewModel"/>.
        /// Sets up the commands used in the game settings
        /// </summary>
        public GameSettingsViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            GameSettings = new GameSettingsModel();

            PlayCommand = new RelayCommand(_ => _navigationService.Navigate(new GameView(_navigationService, GameSettings)));
            BackCommand = new RelayCommand(_ => _navigationService.GoBack());
        }
    }
}
