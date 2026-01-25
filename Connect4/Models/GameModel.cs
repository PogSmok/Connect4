using Connect4.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Connect4.Models
{
    public class GameModel : INotifyPropertyChanged
    {
        public const int Rows = 6;
        public const int Columns = 7;

        private PlayerColor _currentPlayer;
        public PlayerColor CurrentPlayer
        {
            get => _currentPlayer;
            private set
            {
                if (_currentPlayer == value) return;
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        private GameResult _gameResult;
        public GameResult GameResult
        {
            get => _gameResult;
            set
            {
                if (_gameResult == value) return;
                _gameResult = value;
                OnPropertyChanged(nameof(GameResult));
            }
        }

        private readonly List<int> _moves = new();
        public IReadOnlyList<int> Moves => _moves;

        /// <summary>
        /// Collection of cells on the game board.
        /// </summary>
        public ObservableCollection<CellViewModel> Cells { get; }

        public GameModel(PlayerColor startingPlayer)
        {
            CurrentPlayer = startingPlayer;
            GameResult = GameResult.Unknown;
            Cells = new ObservableCollection<CellViewModel>();
            InitializeBoard();
        }

        /// <summary>
        /// Initializes the Connect 4 board with empty cells.
        /// </summary>
        private void InitializeBoard()
        {
            Cells.Clear();
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    Cells.Add(new CellViewModel(row, col));
                }
            }
        }

        /// <summary>
        /// Attempts to drop a chip using the clicked cell (top-row only).
        /// </summary>
        public void DropChip(CellViewModel cell)
        {
            if (GameResult != GameResult.Unknown) return;

            // Only allow dropping from top row
            if (cell.Row != 0) return;

            // Verify the column is not full
            if (IsColumnFull(cell.Column)) return;

            var targetCell = GetLowestEmptyCell(cell.Column);
            if (targetCell == null) return;

            _moves.Add(cell.Column);
            targetCell.State = CurrentPlayer;

            // Check for win
            if (IsWin(targetCell.Row, targetCell.Column))
            {
                GameResult = CurrentPlayer == PlayerColor.Red
                    ? GameResult.RedWonConnect
                    : GameResult.YellowWonConnect;
            }
            // Check for draw
            else if (IsDraw())
            {
                GameResult = GameResult.Draw;
            }
            // Continue game
            else
            {
                SwitchPlayer();
            }
        }

        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == PlayerColor.Red
                ? PlayerColor.Yellow
                : PlayerColor.Red;
        }

        private CellViewModel? GetLowestEmptyCell(int column)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                var cell = Cells[row * Columns + column];
                if (cell.State == null)
                    return cell;
            }
            return null;
        }

        private bool IsColumnFull(int column)
        {
            return Cells[column].State != null;
        }

        public bool IsDraw()
        {
            // Draw occurs only when all cells are filled and there is no win
            return _moves.Count == Rows * Columns;
        }

        /// <summary>
        /// Checks if after placing a chip at [row, column] current player wins.
        /// </summary>
        public bool IsWin(int row, int column)
        {
            return Count(row, column, 1, 0) + Count(row, column, -1, 0) >= 3 ||  // vertical
                   Count(row, column, 0, 1) + Count(row, column, 0, -1) >= 3 ||  // horizontal
                   Count(row, column, 1, 1) + Count(row, column, -1, -1) >= 3 || // diagonal /
                   Count(row, column, 1, -1) + Count(row, column, -1, 1) >= 3;   // diagonal \
        }

        private int Count(int row, int col, int dRow, int dCol)
        {
            int count = 0;
            for (int i = 1; i < 4; i++)
            {
                int r = row + dRow * i;
                int c = col + dCol * i;
                if (r < 0 || r >= Rows || c < 0 || c >= Columns)
                    break;

                var cell = Cells[r * Columns + c];
                if (cell.State == CurrentPlayer) count++;
                else break;
            }
            return count;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
