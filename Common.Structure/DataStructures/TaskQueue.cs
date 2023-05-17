using System;
using System.Threading.Tasks;

namespace Common.Structure.DataStructures;

/// <summary>
/// Contains a process to queue a collection of tasks to enact them in the order added.
/// </summary>
public sealed class TaskQueue
{
    private readonly object lockRoot = new object();
    private Task fPreviousTask;

    /// <summary>
    /// Add an action to the queue.
    /// </summary>
    public void Enqueue(Action action)
    {
        lock (lockRoot)
        {
            fPreviousTask = fPreviousTask?.ContinueWith(tsk => action()) ?? Task.Factory.StartNew(action);
        }
    }

    /// <summary>
    /// Add an action to the queue.
    /// </summary>
    public void Enqueue<T>(Action<T> action, T obj)
    {
        lock (lockRoot)
        {
            fPreviousTask = fPreviousTask?.ContinueWith(tsk => action(obj)) ?? Task.Factory.StartNew(Convert(action), obj);
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
    public void Enqueue(Task currentTask)
    {
        lock (lockRoot)
        {
            fPreviousTask = fPreviousTask?.ContinueWith(tsk => currentTask) ?? currentTask;
        }
    }
}
