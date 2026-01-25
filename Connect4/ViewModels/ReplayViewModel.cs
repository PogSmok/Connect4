using Connect4.Models;
using Connect4.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;

namespace Connect4.ViewModels
{
    /// <summary>
    /// ViewModel responsible for replaying a stored game move-by-move.
    /// </summary>
    public class ReplayViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly GameDbRecord _game;

        /// <summary>
        /// Collection of board cells displayed in the UI.
        /// </summary>
        public ObservableCollection<CellViewModel> Cells { get; }

        /// <summary>
        /// Apply next move
        /// </summary>
        public ICommand NextMoveCommand { get; }

        /// <summary>
        /// Go back a move
        /// </summary>
        public ICommand PreviousMoveCommand { get; }

        /// <summary>
        /// Return to db view
        /// </summary>
        public ICommand BackCommand { get; }

        /// <summary>
        /// Current move index being displayed.
        /// </summary>
        private int _currentMoveIndex;

        public int CurrentMoveIndex
        {
            get => _currentMoveIndex;
            private set
            {
                if (_currentMoveIndex == value) return;
                _currentMoveIndex = value;
                OnPropertyChanged(nameof(MoveCounterText));
            }
        }

        public int TotalMoves => _game.Moves.Count;

        public string PlayersText => _game.PlayersText;
        public string ResultText => _game.ResultText;

        /// <summary>
        /// Text shown in UI representing replay progress.
        /// </summary>
        public string MoveCounterText => $"Move {CurrentMoveIndex} / {TotalMoves}";

        /// <summary>
        /// Initializes a new instance of <see cref="ReplayViewModel"/>.
        /// Allows browsing a game from db.
        /// </summary>
        public ReplayViewModel(NavigationService navigationService, GameDbRecord game)
        {
            _navigationService = navigationService;
            _game = game;

            Cells = CreateEmptyBoard();

            NextMoveCommand = new RelayCommand(
                _ => ApplyNextMove(),
                _ => CurrentMoveIndex < TotalMoves
            );

            PreviousMoveCommand = new RelayCommand(
                _ => UndoLastMove(),
                _ => CurrentMoveIndex > 0
            );

            BackCommand = new RelayCommand(_ => _navigationService.GoBack());
        }

        /// <summary>
        /// Applies the next move from the stored game.
        /// </summary>
        private void ApplyNextMove()
        {
            int column = _game.Moves[CurrentMoveIndex];

            var player = (CurrentMoveIndex % 2 == 0)
                ? _game.Settings.StartingPlayer
                : Opponent(_game.Settings.StartingPlayer);

            DropChip(column, player);
            CurrentMoveIndex++;

            RaiseCommandState();
        }

        /// <summary>
        /// Removes the previously applied move.
        /// </summary>
        private void UndoLastMove()
        {
            int column = _game.Moves[CurrentMoveIndex - 1];

            RemoveTopChip(column);
            CurrentMoveIndex--;

            RaiseCommandState();
        }

        /// <summary>
        /// Updates CanExecute state of navigation commands.
        /// </summary>
        private void RaiseCommandState()
        {
            (NextMoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (PreviousMoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Creates an empty Connect4 board.
        /// </summary>
        private ObservableCollection<CellViewModel> CreateEmptyBoard()
        {
            var cells = new ObservableCollection<CellViewModel>();

            for (int row = 0; row < GameModel.Rows; row++)
            {
                for (int col = 0; col < GameModel.Columns; col++)
                {
                    cells.Add(new CellViewModel(row, col));
                }
            }

            return cells;
        }

        /// <summary>
        /// Drops a chip into a column for the given player.
        /// </summary>
        private void DropChip(int column, PlayerColor player)
        {
            for (int row = GameModel.Rows - 1; row >= 0; row--)
            {
                var cell = Cells[row * GameModel.Columns + column];
                if (cell.State == null)
                {
                    cell.State = player;
                    return;
                }
            }
        }

        /// <summary>
        /// Removes the topmost chip from a column.
        /// </summary>
        private void RemoveTopChip(int column)
        {
            for (int row = 0; row < GameModel.Rows; row++)
            {
                var cell = Cells[row * GameModel.Columns + column];
                if (cell.State != null)
                {
                    cell.State = null;
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the opponent of the given player.
        /// </summary>
        private static PlayerColor Opponent(PlayerColor player)
            => player == PlayerColor.Red ? PlayerColor.Yellow : PlayerColor.Red;


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
