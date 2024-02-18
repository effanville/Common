using System.Windows.Controls;

namespace Common.UI.Wpf.Controls
{
    /// <summary>
    /// A control that auto generates the DateTime format in UK date format.
    /// </summary>
    public class AutoGenColumnControl : UserControl
    {
        /// <summary>
        /// Event to use when Column of DataGrid are auto generating.
        /// </summary>
        public void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }
        }
    }
}
