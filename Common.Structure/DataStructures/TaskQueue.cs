using System;
using System.Threading.Tasks;

namespace Common.Structure.DataStructures;

public sealed class TaskQueue
{
    private object lockRoot = new object();
    private Task fPreviousTask;

    public void Enqueue(Action action)
    {
        lock (lockRoot)
        {
            fPreviousTask = fPreviousTask?.ContinueWith(tsk => action()) ?? Task.Factory.StartNew(action);
        }
    }

    public void Enqueue(Task currentTask)
    {
        lock (lockRoot)
        {
            fPreviousTask = fPreviousTask?.ContinueWith(tsk => currentTask) ?? currentTask;
        }
    }
}
