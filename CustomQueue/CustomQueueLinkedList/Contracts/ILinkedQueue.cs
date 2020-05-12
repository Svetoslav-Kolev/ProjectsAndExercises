using System;
using System.Collections.Generic;
using System.Text;

namespace CustomQueueLinkedList.Contracts
{
   public interface ILinkedQueue<T>
    {
        int Count { get; }
        void Enqueue(T item);
        T Dequeue();
        bool isEmpty { get; }
        T Peek();
    }
}
