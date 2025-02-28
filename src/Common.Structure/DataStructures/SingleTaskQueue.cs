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
    public Task Enqueue(Task currentTask)
    {
        currentTask.RunSynchronously();
        return currentTask;
    }

    /// <inheritdoc/>
    public Task<TReturn> Enqueue<TData, TReturn>(Func<TData, TReturn> func, TData obj)
        => Task.FromResult(func(obj));
}