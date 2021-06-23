using System.Windows;
using Common.UI.Interfaces;

namespace Common.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// This is a container window for any dialogs that are opened via 
    /// the Dialog service.
    /// </summary>
    public sealed partial class DialogWindow : Window, ICloseable
    {
        /// <summary>
        /// Create an instance.
        /// </summary>
        public DialogWindow()
        {
            InitializeComponent();
        }
    }
}
