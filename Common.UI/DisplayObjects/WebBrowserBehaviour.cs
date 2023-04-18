using System;
using System.Windows;
using System.Windows.Controls;

namespace Common.UI.DisplayObjects
{
    /// <summary>
    /// Class to assist with web browser to set a binding source.
    /// </summary>
    public static class WebBrowserBehaviours
    {
        /// <summary>
        /// The bindable source property.
        /// </summary>
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(object), typeof(WebBrowserBehaviours), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        /// <summary>
        /// Returns the bound source. 
        /// </summary>
        public static object GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        /// <summary>
        /// Sets the source to the value specified.
        /// </summary>
        public static void SetBindableSource(DependencyObject obj, object value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        /// <summary>
        /// The property for binding the web browser source with.
        /// </summary>
        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is not WebBrowser browser)
            {
                return;
            }

            Uri uri = null;

            if (e.NewValue is string)
            {
                string uriString = e.NewValue as string;
                uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
            }
            else if (e.NewValue is Uri)
            {
                uri = e.NewValue as Uri;
            }

            browser.Source = uri;
        }
    }
}
