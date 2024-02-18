namespace Common.Structure.Results;

/// <summary>
/// A class detailing the result of an operation.
/// </summary>
public abstract class Result
{
    /// <summary>
    /// Is the resultant output successful.
    /// </summary>
    public bool Success { get; protected set; }
    
    /// <summary>
    /// Is the resultant output failure.
    /// </summary>
    public bool Failure => !Success;
}