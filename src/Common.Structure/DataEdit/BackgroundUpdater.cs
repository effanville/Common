using System;

using Effanville.Common.Structure.DataStructures;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Implementation of a <see cref="IUpdater"/> that performs the action asyncronously.
/// </summary>
public sealed class BackgroundUpdater : IUpdater
{
    private readonly TaskQueue _taskQueue = new TaskQueue();

    /// <inheritdoc/>
    public void PerformUpdateAction<T>(T data, Action<T> action) where T : class
        => _taskQueue.Enqueue(action, data);

    /// <inheritdoc/>
    public void PerformUpdate<T>(T data, UpdateRequestArgs<T> requestArgs) where T : class
    {
        if (!requestArgs.IsHandled)
        {
            _taskQueue.Enqueue(requestArgs.UpdateAction, data);
            requestArgs.IsHandled = true;
        }
    }
}
