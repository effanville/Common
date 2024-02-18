using System.Collections.Generic;

namespace Effanville.Common.Structure.Results;

/// <summary>
/// A <see cref="Result"/> indicating failure and detailing why the failure happened.
/// </summary>
public class ErrorResult : Result
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
    /// Construct an instance of a <see cref="ErrorResult"/> from a single message.
    /// </summary>
    public ErrorResult(string message) 
        : this(message, new List<string>())
    {
    }
    
    /// <summary>
    /// Construct an instance of a <see cref="ErrorResult"/> from a single message, and an extra
    /// list of errors.
    /// </summary>
    public ErrorResult(string message, IList<string> errors) 
        : base()
    {
        Message = message;
        Errors = errors;
        Success = false;
    }
}