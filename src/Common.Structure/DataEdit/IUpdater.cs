using System;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Contains methods for updating arbitrary objects.
/// </summary>
public interface IUpdater
{
    /// <summary>
    /// Update the portfolio with the given action.
    /// </summary>
    void PerformUpdateAction<T>(T data, Action<T> action) where T : class;

    /// <summary>
    /// Update the portfolio with the given action.
    /// </summary>
    void PerformUpdate<T>(T data, UpdateRequestArgs<T> requestArgs) where T : class;
}
