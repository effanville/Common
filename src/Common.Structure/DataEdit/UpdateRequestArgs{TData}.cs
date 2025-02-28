using System;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Contains information on how to update an object of type <typeparamref name="TData"/>
/// </summary>
public class UpdateRequestArgs<TData> where TData : class
{
    /// <summary>
    /// Was the change initiated from a user action?
    /// </summary>
    public bool UserInitiated { get; }

    /// <summary>
    /// The action to run to update the portfolio.
    /// </summary>
    public Action<TData> UpdateAction { get; }

    /// <summary>
    /// Has this request already been handled.
    /// </summary>
    public bool IsHandled { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public UpdateRequestArgs(bool userInitiated, Action<TData> updateAction)
    {
        UserInitiated = userInitiated;
        UpdateAction = updateAction;
    }
}
