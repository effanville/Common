namespace Effanville.Common.Structure.Results;

/// <summary>
/// A class detailing that the result of an operation was successful
/// and with output data.
/// </summary>
public class SuccessResult<TData> : Result<TData>
{
    /// <summary>
    /// Construct an instance of a <see cref="SuccessResult{TData}"/> from a data object.
    /// </summary>
    public SuccessResult(TData data)
        : base(data)
    {
        Success = true;
    }        
    
    /// <summary>
    /// Create a <see cref="SuccessResult{TData}"/> object implicitly from a value.
    /// </summary>
    public static implicit operator SuccessResult<TData>(TData value)
        => new(value);
}