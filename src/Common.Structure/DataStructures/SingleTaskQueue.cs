using System;
using System.Threading.Tasks;

namespace Effanville.Common.Structure.DataStructures;

/// <summary>
/// Task queue implementation that runs synchronously.
/// </summary>
public sealed class SingleTaskQueue : ITaskQueue
{
    /// <inheritdoc/>
    public void Enqueue(Action action) => action();

    /// <inheritdoc/>
    public void Enqueue<T>(Action<T> action, T obj) => action(obj);

    /// <inheritdoc/>
    public void Enqueue(Task currentTask){ }
}