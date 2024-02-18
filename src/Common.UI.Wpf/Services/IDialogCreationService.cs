using System.Windows;

using Effanville.Common.UI.Services;

namespace Effanville.Common.UI.Wpf.Services
{
    /// <summary>
    /// Interface for creating dialog boxes in the UI. Note that this should live in the UI part, but is a service that can be used in
    /// the view model area of the code.
    /// </summary>
    public interface IDialogCreationService : IBaseDialogCreationService
    {
        /// <summary>
        /// Shows a standard message box with the specified parameters with non-default owner.
        /// </summary>
        MessageBoxOutcome ShowMessageBox(Window owner, string text, string title, BoxButton buttons, BoxImage imageType);
    }
}
