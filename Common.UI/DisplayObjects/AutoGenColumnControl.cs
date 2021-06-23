using System.Windows.Controls;

namespace Common.UI.DisplayObjects
{
    public class AutoGenColumnControl : UserControl
    {
        public void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }
        }
    }
}
