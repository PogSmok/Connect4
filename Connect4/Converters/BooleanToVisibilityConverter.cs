using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Connect4.Converters
{
    /// <summary>
    /// Converts between a Bool value and a Visibility enum to control UI popups.
    /// Visible if true, collapsed if false.
    /// Supports two-way binding.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to visibilty enum
        /// </summary>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolVal) return boolVal ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts an enum back to boolean
        /// </summary>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Visibility vis) return vis == Visibility.Visible;

            return false;
        }
    }
}
