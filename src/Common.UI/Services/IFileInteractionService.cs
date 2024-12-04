using System.Threading.Tasks;

namespace Effanville.Common.UI.Services
{
    /// <summary>
    /// Interface for interacting with the file structure.
    /// </summary>
    public interface IFileInteractionService
    {
        /// <summary>
        /// Interaction with a saving dialog.
        /// </summary>
        Task<FileInteractionResult> SaveFile(string defaultExt, string fileName, string initialDirectory = null, string filter = null);

        /// <summary>
        /// Interaction with an opening file dialog.
        /// </summary>
        Task<FileInteractionResult> OpenFile(string defaultExt, string initialDirectory = null, string filter = null);
    }
}
