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
