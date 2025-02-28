using System;
using System.Threading.Tasks;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Contains methods for updating arbitrary objects.
/// </summary>
public interface IUpdater
{
    /// <summary>
    /// Update the portfolio with the given action.
    /// </summary>
    void PerformUpdateAction<TData>(TData data, Action<TData> action) where TData : class;

    /// <summary>
    /// Update the portfolio with the given action.
    /// </summary>
    Task PerformUpdate<TData>(TData data, UpdateRequestArgs<TData> requestArgs) where TData : class;

    /// <summary>
    /// Update the data using the args
    /// </summary>
    Task<UpdateResult<TReturn>> PerformUpdate<TData, TReturn>(TData data, UpdateRequestArgs<TData, TReturn> requestArgs) where TData : class;
}
