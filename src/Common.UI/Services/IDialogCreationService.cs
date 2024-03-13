namespace Effanville.Common.UI.Services
{
    /// <summary>
    /// Interface for creating dialog boxes in the UI. Note that this should live in the UI part, but is a service that can be used in
    /// the view model area of the code.
    /// </summary>
    public interface IBaseDialogCreationService
    {
        /// <summary>
        /// Shows a standard message box with the specified parameters.
        /// </summary>
        MessageBoxOutcome ShowMessageBox(string text, string title, BoxButton buttons, BoxImage imageType);

        /// <summary>
        /// Displays an arbitrary dialog window, populated from an object which is
        /// either a window itself, or is a viewModel.
        /// </summary>
        void DisplayCustomDialog(object obj);
    }
}
