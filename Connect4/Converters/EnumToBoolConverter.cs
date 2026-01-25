using System.Globalization;
using System.Windows.Data;

namespace Connect4.Converters
{
    /// <summary>
    /// Converts between an Enum value and a Boolean for use with RadioButtons.
    /// True if the bound value equals the ConverterParameter; otherwise false.
    /// Supports two-way binding.
    /// </summary>
    public sealed class EnumToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts an enum value to a boolean for a RadioButton.
        /// </summary>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;

            return value.ToString() == parameter.ToString();
        }

        /// <summary>
        /// Converts a boolean back to the enum value.
        /// </summary>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter != null && targetType.IsEnum)
            {
                return Enum.Parse(targetType, parameter.ToString()!);
            }

            return Binding.DoNothing;
        }
    }
}