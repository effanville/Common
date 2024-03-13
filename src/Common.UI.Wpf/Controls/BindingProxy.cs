using System.Windows;

namespace Effanville.Common.UI.Wpf.Controls
{
    /// <summary>
    /// A proxy to use to enable binding when it otherwise doesnt have it.
    /// </summary>
    public class BindingProxy : Freezable
    {
        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        /// <summary>
        /// The data to use in binding.
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The property for the Data above.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}