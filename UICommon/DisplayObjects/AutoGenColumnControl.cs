﻿using System.Windows.Controls;

namespace UICommon.DisplayObjects
{
    public class AutoGenColumnControl : UserControl
    {
        protected void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }
        }
    }
}
