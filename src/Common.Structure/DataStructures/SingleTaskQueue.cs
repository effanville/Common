using System;
using System.Threading.Tasks;

namespace Effanville.Common.Structure.DataStructures;

/// <summary>
/// Task queue implementation that runs synchronously.
/// </summary>
public sealed class SingleTaskQueue : ITaskQueue
{
    /// <inheritdoc/>
    public Task Enqueue(Action action)
    {
        action();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task Enqueue<T>(Action<T> action, T obj)
    {
        action(obj);
        return Task.CompletedTask;
    }

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