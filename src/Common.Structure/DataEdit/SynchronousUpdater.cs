using System;
using System.Threading.Tasks;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Implementation of a <see cref="IUpdater"/> that performs the action syncronously.
/// </summary>
public sealed class SynchronousUpdater : IUpdater
{
    /// <inheritdoc/>
    public void PerformUpdateAction<TData>(TData data, Action<TData> action) where TData : class
        => action(data);

    /// <inheritdoc/>
    public Task PerformUpdate<TData>(TData data, UpdateRequestArgs<TData> requestArgs) where TData : class
    {
        if (!requestArgs.IsHandled)
        {
            requestArgs.UpdateAction(data);
            requestArgs.IsHandled = true;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<UpdateResult<TReturn>> PerformUpdate<TData, TReturn>(TData data, UpdateRequestArgs<TData, TReturn> requestArgs) where TData : class
    {
        if (!requestArgs.IsHandled)
        {

            requestArgs.IsHandled = true;
            return Task.FromResult(requestArgs.UpdateFunction(data));
        }

        return null;
    }
}