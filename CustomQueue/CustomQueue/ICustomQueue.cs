using System;
using System.Collections.Generic;
using System.Text;

namespace CustomQueue
{
   public interface ICustomQueue<T>:IEnumerable<T>
    {
        int Count { get; }
        int Capacity { get; }
        bool IsFull { get; }
        void Enqueue(T item);
        T Dequeue();
        bool IsEmpty { get; }
        T Peek();
    }
}
