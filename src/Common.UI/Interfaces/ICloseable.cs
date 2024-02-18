namespace Effanville.Common.UI.Interfaces
{
    /// <summary>
    /// Wrapper for an object that enables the closing of itself (e.g. a window)
    /// </summary>
    public interface ICloseable
    {
        /// <summary>
        /// A routine to close.
        /// </summary>
        void Close();
    }
}
