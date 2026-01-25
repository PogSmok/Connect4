using Connect4.Models;
using Connect4.Services;
using Connect4.Data;
using Connect4.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;

namespace Connect4.ViewModels
{
    public class DbViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;

        public ObservableCollection<GameDbRecord> Games { get; }

        /// <summary>
        /// Opens selected game in a replay mode
        /// </summary>
        public ICommand ViewGameCommand { get; }

        /// <summary>
        /// Removes selected game from database
        /// </summary>
        public ICommand DeleteGameCommand { get; }

        /// <summary>
        /// Goes back to main menu
        /// </summary>
        public ICommand BackCommand { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DbViewModel"/>.
        /// Shows stored games in database
        /// </summary>
        public DbViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            Games = new ObservableCollection<GameDbRecord>(GetGames());

            ViewGameCommand = new RelayCommand(game => {
                if (game is GameDbRecord record)
                {
                    _navigationService.Navigate(new ReplayView(_navigationService, record));
                }
            });
            DeleteGameCommand = new RelayCommand(game =>
            {
                if (game is GameDbRecord record)
                {
                    RemoveGame(record);
                }
            });
            BackCommand = new RelayCommand(_ => _navigationService.GoBack());
        }

        /// <summary>
        /// Extracts all games from database
        /// </summary>
        private List<GameDbRecord> GetGames()
        {
            using var db = new GameDbContext();
            return db.Games.OrderByDescending(g => g.PlayedAt).ToList();
        }


        /// <summary>
        /// Removes record from the database
        /// </summary>
        private void RemoveGame(GameDbRecord record)
        {
            using var db = new GameDbContext();
            db.Games.Remove(record);
            db.SaveChanges();

            // Remove from UI
            Games.Remove(record);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
