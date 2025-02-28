using System;
using System.Threading.Tasks;

namespace Effanville.Common.Structure.DataStructures;

/// <summary>
/// Contains a process to queue a collection of tasks to enact them in the order added.
/// </summary>
public sealed class TaskQueue : ITaskQueue
{
    private readonly object _previousTaskLock = new object();
    private Task _previousTask;

    /// <summary>
    /// Add an action to the queue.
    /// </summary>
    public void Enqueue(Action action)
    {
        lock (_previousTaskLock)
        {
            _previousTask = _previousTask?.ContinueWith(tsk => action()) ?? Task.Factory.StartNew(action);
        }
    }

    /// <summary>
    /// Add an action to the queue.
    /// </summary>
    public void Enqueue<T>(Action<T> action, T obj)
    {
        lock (_previousTaskLock)
        {
            _previousTask = _previousTask?.ContinueWith(tsk => action(obj)) ?? Task.Factory.StartNew(Convert(action), obj);
        }
    }

    private static Action<object> Convert<T>(Action<T> myActionT)
    {
        return myActionT == null
            ? null
            : new Action<object>(o => myActionT((T)o));
    }

    /// <summary>
    /// Add a task to the queue
    /// </summary>
    public Task Enqueue(Task currentTask)
    {
        lock (_previousTaskLock)
        {
            _previousTask = _previousTask?.ContinueWith(tsk => currentTask) ?? currentTask;
            return _previousTask;
        }
    }

    /// <inheritdoc/>
    public Task<TReturn> Enqueue<TData, TReturn>(Func<TData, TReturn> func, TData obj)
    {
        lock (_previousTaskLock)
        {
            _previousTask = _previousTask?.ContinueWith(tsk => func(obj)) ?? Task.Factory.StartNew(Convert(func), obj);
            return _previousTask as Task<TReturn>;
        }
    }

    private static Func<object, S> Convert<T, S>(Func<T, S> myActionT)
    {
        return myActionT == null
            ? null
            : new Func<object, S>(o => myActionT((T)o));
    }
}
