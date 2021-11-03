using System;
using System.Globalization;
using System.Windows.Data;
using Common.Structure.NamingStructures;

namespace Common.UI.Converters
{
    /// <summary>
    /// Converts an object of type <see cref="Name"/> into a string.
    /// </summary>
    public class NameToStringConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(Name))
            {
                return value.ToString();
            }

            return value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string[] splitted = value.ToString().Split('-');
                if (splitted.Length == 2)
                {
                    return new Name(splitted[0], splitted[1]);
                }
                if (splitted.Length == 1)
                {
                    return new Name(splitted[0], "");
                }

                return new Name();
            }

            return value;
        }
    }
}
