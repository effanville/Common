namespace Effanville.Common.Console;

/// <summary>
/// Contains the context for a console application, as well as the
/// validation and execution routines.
/// </summary>
public interface IConsoleContext
{        
    /// <summary>
    /// Validates the chosen command.
    /// </summary>
    bool Validate();

    /// <summary>
    /// Executes the chosen command.
    /// </summary>
    int Execute();
        
    /// <summary>
    /// Validate and then execute the chosen command.
    /// </summary>
    int ValidateAndExecute();
}