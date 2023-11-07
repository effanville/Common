using System.Collections.Generic;

namespace Common.Structure.Results;

/// <summary>
/// A <see cref="Result{TData}"/> indicating failure and detailing why the failure happened.
/// </summary>
public class ErrorResult<TData> : Result<TData>
{
    /// <summary>
    /// The message for the error.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// A list of ancillary errors.
    /// </summary>
    public IList<string> Errors { get; set; }
    
    /// <summary>
    /// Construct an instance of a <see cref="ErrorResult{TData}"/> from a single message.
    /// </summary>
    public ErrorResult(string message) 
        : this(message, new List<string>())
    {
    }
    
    /// <summary>
    /// Construct an instance of a <see cref="ErrorResult{TData}"/> from a single message.
    /// </summary>
    public ErrorResult(string message, IList<string> errors) 
        : base(default)
    {
        Message = message;
        Errors = errors;
        Success = false;
    }
}