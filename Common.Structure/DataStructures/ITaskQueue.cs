using System;
using System.Threading.Tasks;

namespace Common.Structure.DataStructures;

public interface ITaskQueue
{
    void Enqueue(Action action);

    void Enqueue<T>(Action<T> action, T obj);
    
    void Enqueue(Task currentTask);
}