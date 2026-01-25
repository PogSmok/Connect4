using Connect4.Models;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Connect4.Converters
{
    /// <summary>
    /// Converts a <see cref="PlayerColor"/> value to a corresponding <see cref="Brush"/>
    /// for rendering Connect 4 board cells.
    /// </summary>
    public sealed class PlayerColorToBrushConverter : IValueConverter
    {
        private readonly Brush _emptyBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xEE, 0xFF));
        private readonly Brush _redBrush = Brushes.Red;
        private readonly Brush _yellowBrush = Brushes.Gold;

        /// <summary>
        /// Converts a <see cref="PlayerColor"/> to a <see cref="Brush"/>.
        /// </summary>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not PlayerColor playerColor)
                return _emptyBrush;

            return playerColor switch
            {
                PlayerColor.Red => _redBrush,
                PlayerColor.Yellow => _yellowBrush,
                _ => _emptyBrush
            };
        }

        /// <summary>
        /// ConvertBack should not be used
        /// </summary>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
