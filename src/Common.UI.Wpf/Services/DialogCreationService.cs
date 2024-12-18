﻿using System.Windows;

using Effanville.Common.UI.Services;
using Effanville.Common.UI.Wpf.Dialogs;

namespace Effanville.Common.UI.Wpf.Services
{
    /// <summary>
    /// Created dialog boxes in the UI. Note that this should live in the UI part, but is a service that can be used in
    /// the view model area of the code.
    /// </summary>
    public class DialogCreationService : IDialogCreationService
    {
        /// <summary>
        /// The default parent to use if non are selected.
        /// </summary>
        private readonly Window fDefaultParent;

        /// <summary>
        /// The standard constructor to use.
        /// </summary>
        public DialogCreationService(Window defaultParent)
        {
            fDefaultParent = defaultParent;
        }

        /// <summary>
        /// Shows a standard message box with the specified parameters.
        /// </summary>       
        /// <inheritdoc/>
        public MessageBoxOutcome ShowMessageBox(string text, string title, BoxButton buttons, BoxImage imageType)
        {
            MessageBoxImage messageImageType = imageType.ToMessageBoxImage();
            MessageBoxButton messageButtons = buttons.ToMessageBoxButton();
            return MessageBox.Show(fDefaultParent, text, title, messageButtons, messageImageType).ToResult();
        }

        /// <summary>
        /// Shows a standard message box with the specified parameters with non-default owner.
        /// </summary>
        /// <inheritdoc/>
        public MessageBoxOutcome ShowMessageBox(Window owner, string text, string title, BoxButton buttons, BoxImage imageType)
        {
            MessageBoxImage messageImageType = imageType.ToMessageBoxImage();
            MessageBoxButton messageButtons = buttons.ToMessageBoxButton();
            return MessageBox.Show(owner, text, title, messageButtons, messageImageType).ToResult();
        }

        /// <summary>
        /// Displays an arbitrary dialog window, populated from an object which is
        /// either a window itself, or is a viewModel.
        /// In the latter case one should add a template into the DialogTemplate.xaml file.
        /// </summary>
        /// <inheritdoc/>
        public void DisplayCustomDialog(object obj)
        {
            // If obj is a window, then display that window.
            if (obj is Window window)
            {
                window.Owner = fDefaultParent;
                _ = window.ShowDialog();
            }
            else
            {
                // if obj isnt a window, guess it is a view model, so try to display as such.
                DialogWindow dialog = new DialogWindow() { DataContext = obj };
                dialog.ShowInTaskbar = true;
                _ = dialog.ShowDialog();
            }
        }
    }
}
