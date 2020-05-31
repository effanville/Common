using System;
using System.Globalization;
using System.Windows.Data;
using StructureCommon.NamingStructures;

namespace UICommon.Converters
{
    public class NameToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(Name))
            {
                return value.ToString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string[] splitted = value.ToString().Split(' ');
                if (splitted.Length == 2)
                {
                    return new Name(splitted[1], splitted[0]);
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
