namespace Common.Console
{
    /// <summary>
    /// The exitCode of the application.
    /// </summary>
    public enum ExitCode
    {
        /// <summary>
        /// The execution was a success.
        /// </summary>
        Success = 0,

        /// <summary>
        /// There was an error in a command.
        /// </summary>
        CommandError = 1,

        /// <summary>
        /// There was an error in an option.
        /// </summary>
        OptionError = 2
    }
}
