using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Effanville.Common.UI.Wpf.Converters
{
    /// <summary>
    /// Converter to convert an array of objects into a single object (and not back again)
    /// </summary>
    public sealed class ArrayMultiValueConverter : MarkupExtension, IMultiValueConverter
    {
        private static ArrayMultiValueConverter sConverter;

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (sConverter == null)
            {
                sConverter = new ArrayMultiValueConverter();
            }

            return sConverter;
        }

        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
