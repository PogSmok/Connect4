using System.ComponentModel;
using System.Runtime.CompilerServices;
using Connect4.Models;

namespace Connect4.ViewModels
{
    /// <summary>
    /// Represents a single cell on the Connect 4 game board.
    /// </summary>
    public class CellViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Row index of the cell (0 = top).
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Column index of the cell (0 = left).
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Gets or sets the current state of the cell.
        /// Null represents an empty cell.
        /// </summary>
        private PlayerColor? _state;
        public PlayerColor? State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;

                _state = value;
                OnPropertyChanged();
            }
        }

        public CellViewModel(int row, int column)
        {
            Row = row;
            Column = column;
            State = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}