using System;
using System.Collections.Generic;
using System.Text;

namespace CustomListExercise.Contracts
{
    interface ICustomList<T>
    {
        void Add(T item);
        void Remove(T item);
        int Count { get; }
        void Insert(T item, int index);
        int IndexOf(T item);
        bool Contains(T element);
        void Swap(int index1, int index2);
        int CountGreaterThan(T element);
        void Clear();
    }
}
