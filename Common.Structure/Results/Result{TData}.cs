namespace Common.Structure.Results;

/// <summary>
/// A class detailing the result of an operation and with output data.
/// </summary>
public abstract class Result<TData> : Result
{
    private TData _data;
    
    /// <summary>
    /// The data output of the operation.
    /// </summary>
    public TData Data
    {
        get => Success ? _data : default;
        set => _data = value;
    }
    
    /// <summary>
    /// Construct an instance of a <see cref="Result{TData}"/> from a data object.
    /// </summary>
    public Result(TData data)
    {
        Data = data;
    }
    
    /// <summary>
    /// Create a <see cref="Result{TData}"/> object implicitly from a value. This implies success.
    /// </summary>
    public static implicit operator Result<TData>(TData value)
        => new SuccessResult<TData>(value);
}