using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UICommon.Converters
{
    /// <summary>
    /// Converter to convert from a DateTime into the UK representation of that DateTime.
    /// </summary>
    public sealed class StringToUKDateTimeConverter : MarkupExtension, IValueConverter
    {
        private static StringToUKDateTimeConverter sConverter;

        /// <summary>
        /// Called to construct the value provider when required.
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (sConverter == null)
            {
                sConverter = new StringToUKDateTimeConverter();
            }

            return sConverter;
        }

        /// <summary>
        /// Converts into the UK representation of that date.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (culture.IetfLanguageTag == "en-US")
                {
                    CultureInfo UKEnglishCulture = new CultureInfo("en-GB");
                    return date.ToString(UKEnglishCulture.DateTimeFormat);
                }
                else
                {
                    return date.ToString(culture.DateTimeFormat);
                }
            }

            return null;
        }

        /// <summary>
        /// Converts back from a string into the relevant datetime
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (culture.IetfLanguageTag == "en-US")
                {
                    CultureInfo UKEnglishCulture = new CultureInfo("en-GB");
                    if (DateTime.TryParse(stringValue, UKEnglishCulture.DateTimeFormat, DateTimeStyles.None, out DateTime date))
                    {
                        return date;
                    }
                }
                else
                {
                    if (DateTime.TryParse(stringValue, culture.DateTimeFormat, DateTimeStyles.None, out DateTime date))
                    {
                        return date;
                    }
                }
            }
            return null;
        }
    }
}
