using System;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Implementation of a <see cref="IUpdater"/> that performs the action syncronously.
/// </summary>
public sealed class SynchronousUpdater : IUpdater
{
    /// <inheritdoc/>
    public void PerformUpdateAction<T>(T data, Action<T> action) where T : class
        => action(data);

    /// <inheritdoc/>
    public void PerformUpdate<T>(T data, UpdateRequestArgs<T> requestArgs) where T : class
    {
        if (!requestArgs.IsHandled)
        {
            requestArgs.UpdateAction(data);
            requestArgs.IsHandled = true;
        }
    }
}