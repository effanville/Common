using System.Windows;

using Microsoft.Win32;

namespace Common.UI.Services
{
    /// <summary>
    /// Interaction service for the file system. Note this lives in the UI, but is a service for the view models.
    /// </summary>
    public class FileInteractionService : IFileInteractionService
    {
        /// <summary>
        /// The default parent to use if non are selected.
        /// </summary>
        private readonly Window fDefaultParent;

        /// <summary>
        /// The standard constructor to use.
        /// </summary>
        public FileInteractionService(Window defaultParent)
        {
            fDefaultParent = defaultParent;
        }

        /// <summary>
        /// Interaction with a saving dialog.
        /// </summary>
        /// <inheritdoc/>
        public FileInteractionResult SaveFile(string defaultExt, string fileName, string initialDirectory = null, string filter = null)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = defaultExt, FileName = fileName };

            if (!string.IsNullOrEmpty(initialDirectory))
            {
                saving.InitialDirectory = initialDirectory;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                saving.Filter = filter;
            }
            bool? saved = saving.ShowDialog(fDefaultParent);
            if (saved.HasValue)
            {
                return new FileInteractionResult(saved.Value, saving.FileName);
            }

            return new FileInteractionResult(false, null);
        }

        /// <summary>
        /// Interaction with an opening file dialog.
        /// </summary>
        /// <inheritdoc/>
        public FileInteractionResult OpenFile(string defaultExt, string initialDirectory = null, string filter = null)
        {
            OpenFileDialog openFile = new OpenFileDialog() { DefaultExt = defaultExt };

            if (!string.IsNullOrEmpty(initialDirectory))
            {
                openFile.InitialDirectory = initialDirectory;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                openFile.Filter = filter;
            }

            bool? showed = openFile.ShowDialog(fDefaultParent);
            if (showed.HasValue)
            {
                return new FileInteractionResult(showed.Value, openFile.FileName);
            }

            return new FileInteractionResult(false, null);
        }
    }
}
