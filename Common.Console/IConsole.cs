namespace Common.Console
{
    /// <summary>
    /// An interface of a Console writer.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Write the line given by <paramref name="line"/>.
        /// </summary>
        void WriteLine(string line = null);

        /// <summary>
        /// Write the error message <paramref name="error"/>.
        /// </summary>
        void WriteError(string error);
    }
}
