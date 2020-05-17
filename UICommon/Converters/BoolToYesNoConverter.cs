using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace UICommon.Converters
{
    /// <summary>
    /// Converts a boolean to the strings "Yes" and "No" and back again.
    /// </summary>
    public sealed class BoolToYesNoConverter : MarkupExtension, IValueConverter
    {
        private static BoolToYesNoConverter sConverter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (sConverter == null)
            {
                sConverter = new BoolToYesNoConverter();
            }

            return sConverter;
        }

        /// <summary>
        /// Converts from boolean object into yes/no string object.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (booleanValue)
                {
                    return "Yes";
                }

                return "No";
            }

            return null;
        }

        /// <summary>
        /// Converts back from yes/no string object into boolean object.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string name)
            {
                if (name.Equals("Yes"))
                {
                    return true;
                }
                else if (name.Equals("No"))
                {
                    return false;
                }
            }

            return null;
        }
    }
}
