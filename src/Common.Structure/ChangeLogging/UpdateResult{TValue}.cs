namespace Effanville.Common.Structure.ChangeLogging;

/// <summary>
/// Encompasses details about an update to an object of type <see typecref="TValue"/>
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class UpdateResult<TValue>
{
    /// <summary>
    /// 
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Is the update an addition
    /// </summary>
    public bool IsAdd { get; init; }

    /// <summary>
    /// Is the update a change
    /// </summary>
    public bool IsChange { get; init; }

    /// <summary>
    /// Is the update a deletion
    /// </summary>
    public bool IsDelete { get; init; }

    /// <summary>
    /// The previous value before the update
    /// </summary>
    public TValue OldValue { get; init; }

    /// <summary>
    /// The value after the update
    /// </summary>
    public TValue NewValue { get; init; }

    /// <summary>
    /// Any message about the update
    /// </summary>
    public string Message { get; set; }
}
