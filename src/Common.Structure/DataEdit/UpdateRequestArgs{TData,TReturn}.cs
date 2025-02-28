using System;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Contains information on how to update an object of type <typeparamref name="TData"/> and return an object of type <typeparamref name="TReturn"/>
/// </summary>
public class UpdateRequestArgs<TData, TReturn> where TData : class
{
    /// <summary>
    /// Was the change initiated from a user action?
    /// </summary>
    public bool UserInitiated { get; }

    /// <summary>
    /// The function to update using.
    /// </summary>
    public Func<TData, UpdateResult<TReturn>> UpdateFunction { get; }

    /// <summary>
    /// Has this request already been handled.
    /// </summary>
    public bool IsHandled { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public UpdateRequestArgs(bool userInitiated, Func<TData, UpdateResult<TReturn>> updateFunction)
    {
        UserInitiated = userInitiated;
        UpdateFunction = updateFunction;
    }
}
