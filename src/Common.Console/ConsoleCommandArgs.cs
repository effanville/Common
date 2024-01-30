namespace Effanville.Common.Console;

/// <summary>
/// Wrapper for arguments passed into console.
/// </summary>
public sealed class ConsoleCommandArgs
{
    /// <summary>
    /// The arguments stored.
    /// </summary>
    public string[] Args { get; set; }

    /// <summary>
    /// Construct an instance
    /// </summary>
    /// <param name="args">The arguments to use.</param>
    public ConsoleCommandArgs(string[] args)
    {
        Args = args;
    }
}