using System.Collections;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace Effanville.Common.UI.Wpf.Controls
{
    /// <summary>
    /// A chart that can have its source bound to.
    /// </summary>
    public class BindableChart : Chart
    {
        /// <summary>
        /// The source for the series in this bindable chart.
        /// </summary>
        public IEnumerable SeriesSource
        {
            get => (IEnumerable)GetValue(SeriesSourceProperty);

            set => SetValue(SeriesSourceProperty, value);
        }

        /// <summary>
        /// The series source for the series in this bindable chart.
        /// </summary>
        public static readonly DependencyProperty SeriesSourceProperty = DependencyProperty.Register(
            name: "SeriesSource",
            propertyType: typeof(IEnumerable),
            ownerType: typeof(BindableChart),
            typeMetadata: new PropertyMetadata(
                defaultValue: default(IEnumerable),
                propertyChangedCallback: new PropertyChangedCallback(OnSeriesSourceChanged)
            )
        );

        private static void OnSeriesSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IEnumerable newValue = (IEnumerable)e.NewValue;
            BindableChart source = (BindableChart)d;

            source.Series.Clear();
            if (newValue != null)
            {
                foreach (ISeries item in newValue)
                {
                    source.Series.Add(item);
                }
            }
        }
    }
}
