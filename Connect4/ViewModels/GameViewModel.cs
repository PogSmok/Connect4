using Connect4.Data;
using Connect4.Models;
using Connect4.Services;
using Connect4.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;

namespace Connect4.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing the state of the Connect 4 game board.
    /// </summary>
    public sealed class GameViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly GameSettingsModel _gameSettings;
        private readonly GameModel _gameModel;

        /// <summary>
        /// Collection of cells on the game board.
        /// </summary>
        public ObservableCollection<CellViewModel> Cells => _gameModel.Cells;

        public PlayerColor CurrentPlayer => _gameModel.CurrentPlayer;

        /// <summary>
        /// Returns the text to display the current player's turn.
        /// </summary>
        public string CurrentPlayerText => $"{(CurrentPlayer == PlayerColor.Red ? _gameSettings.RedPlayerName : _gameSettings.YellowPlayerName)}'s turn";

        private TimeSpan _redTime;
        private TimeSpan _yellowTime;

        /// <summary>
        /// Null = no time limit game
        /// </summary>
        private PlayerColor? _currentPlayerTime;
        public PlayerColor? CurrentPlayerTime
        {
            get => _currentPlayerTime;
            private set
            {
                if (_currentPlayerTime == value) return;
                _currentPlayerTime = value;
                OnPropertyChanged();
            }
        }

        private readonly DispatcherTimer? _dispatcherTimer;

        /// <summary>
        /// Displays the remaining time for the Red and Yellow player in MM:SS format.
        /// </summary>
        public string RedTimerText => _redTime.ToString(@"mm\:ss");
        public string YellowTimerText => _yellowTime.ToString(@"mm\:ss");

        private bool _isGameOver;
        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                if(_isGameOver == value) return;
                _isGameOver = value;
                OnPropertyChanged();
            }
        }

        private string _gameOverText = string.Empty;
        public string GameOverText
        {
            get => _gameOverText;
            set
            {
                if (_gameOverText == value) return;
                _gameOverText = value;
                OnPropertyChanged();
            }
        }

        public ICommand ForfeitCommand { get; }
        public ICommand DropChipCommand { get; }
        public ICommand ExitCommand { get; }

        public GameViewModel(NavigationService navigationService, GameSettingsModel gameSettings)
        {
            _navigationService = navigationService;
            _gameSettings = gameSettings;
            _gameModel = new GameModel(gameSettings.StartingPlayer);
            _gameModel.PropertyChanged += GameModel_PropertyChanged;

            _redTime = TimeSpan.FromMinutes(gameSettings.BaseTimeMinutes);
            _yellowTime = TimeSpan.FromMinutes(gameSettings.BaseTimeMinutes);

            if (gameSettings.BaseTimeMinutes > 0)
            {
                CurrentPlayerTime = _gameModel.CurrentPlayer;

                _dispatcherTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                _dispatcherTimer.Tick += OnTimerTick;
                _dispatcherTimer.Start();
            }
            else
            {
                CurrentPlayerTime = null;
            }

            DropChipCommand = new RelayCommand(param =>
            {
                if (param is CellViewModel cell)
                    _gameModel.DropChip(cell);
            });

            ForfeitCommand = new RelayCommand(_ => OnForfeit());

            ExitCommand = new RelayCommand(_ => _navigationService.Navigate(new MainMenuView(_navigationService)));
        }

        private void GameModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameModel.CurrentPlayer))
            {
                OnPropertyChanged(nameof(CurrentPlayer));
                OnPropertyChanged(nameof(CurrentPlayerText));

                if (CurrentPlayerTime != null)
                {
                    if (CurrentPlayerTime == PlayerColor.Red)
                    {
                        _redTime += TimeSpan.FromSeconds(_gameSettings.IncrementSeconds);
                        OnPropertyChanged(nameof(RedTimerText));
                    }
                    else
                    {
                        _yellowTime += TimeSpan.FromSeconds(_gameSettings.IncrementSeconds);
                        OnPropertyChanged(nameof(YellowTimerText));
                    }
                    CurrentPlayerTime = _gameModel.CurrentPlayer;
                }
            }

            if (e.PropertyName == nameof(GameModel.GameResult))
            {
                _dispatcherTimer?.Stop();
                CurrentPlayerTime = null;
                GameEnd();
            }
        }

        /// <summary>
        /// Called every second to decrease the current player's timer.
        /// </summary>
        private void OnTimerTick(object? sender, EventArgs e)
        {
            if (CurrentPlayerTime == null) return;
            if (_gameModel.GameResult != GameResult.Unknown) return;

            if (CurrentPlayerTime == PlayerColor.Red)
            {
                _redTime -= TimeSpan.FromSeconds(1);
                OnPropertyChanged(nameof(RedTimerText));

                if (_redTime.TotalSeconds <= 0)
                    _gameModel.GameResult = GameResult.YellowWonOnTime;
            }
            else
            {
                _yellowTime -= TimeSpan.FromSeconds(1);
                OnPropertyChanged(nameof(YellowTimerText));

                if (_yellowTime.TotalSeconds <= 0)
                    _gameModel.GameResult = GameResult.RedWonOnTime;
            }
        }

        /// <summary>
        /// Handles the forfeit action.
        /// </summary>
        private void OnForfeit()
        {
            _dispatcherTimer?.Stop();
            _gameModel.GameResult =
                CurrentPlayer == PlayerColor.Red
                ? GameResult.YellowWonByForfeit
                : GameResult.RedWonByForfeit;
        }

        /// <summary>
        /// Adds an entry to the database
        /// Before calling this function esnure the game has finished
        /// </summary>
        private void SaveGameToDatabase()
        {
            using var db = new GameDbContext();

            var record = new GameDbRecord
            {
                PlayedAt = DateTime.Now,
                Settings = _gameSettings,
                Moves = _gameModel.Moves.ToList(),
                Result = _gameModel.GameResult
            };

            db.Games.Add(record);
            db.SaveChanges();
        }

        /// <summary>
        /// Called upon game end
        /// </summary>
        private void GameEnd()
        {
            switch(_gameModel.GameResult)
            {
                case GameResult.RedWonConnect:
                    GameOverText = _gameSettings.RedPlayerName + " Won!";
                    break;
                case GameResult.YellowWonConnect:
                    GameOverText = _gameSettings.YellowPlayerName + " Won!";
                    break;
                case GameResult.RedWonOnTime:
                    GameOverText = _gameSettings.RedPlayerName + " Won on time!";
                    break;
                case GameResult.YellowWonOnTime:
                    GameOverText = _gameSettings.YellowPlayerName + " Won on time!";
                    break;
                case GameResult.RedWonByForfeit:
                    GameOverText = _gameSettings.RedPlayerName + " Won by forfeit!";
                    break;
                case GameResult.YellowWonByForfeit:
                    GameOverText = _gameSettings.YellowPlayerName + " Won by forfeit!";
                    break;
                case GameResult.Draw:
                    GameOverText = "Game ended in a draw!";
                    break;
                default:
                    GameOverText = "Game ended unexpectedly";
                    break;
            }

            SaveGameToDatabase();

            IsGameOver = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
