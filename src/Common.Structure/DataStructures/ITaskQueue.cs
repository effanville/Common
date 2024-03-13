using System;
using System.Threading.Tasks;

namespace Effanville.Common.Structure.DataStructures;

/// <summary>
/// Contains a process to queue a collection of tasks to enact them in the order added.
/// </summary>
public interface ITaskQueue
{
    /// <summary>
    /// Add an action to the queue.
    /// </summary>
    void Enqueue(Action action);

    /// <summary>
    /// Add an action to the queue.
    /// </summary>
    void Enqueue<T>(Action<T> action, T obj);

    /// <summary>
    /// Add a task to the queue
    /// </summary>
    void Enqueue(Task currentTask);
}