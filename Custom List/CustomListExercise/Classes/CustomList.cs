using System;
using System.Collections.Generic;
using System.Text;
using CustomListExercise.Contracts;

namespace CustomListExercise.Classes
{
    class CustomList<T> : ICustomList<T>
        where T: IComparable<T>
    {
        private const int startCapacity = 4;
        public int Count { get;private set; }
        private T[] array;
        private int capacity { get; set; }
        public CustomList()
        {
            
            array = new T[startCapacity];
            this.Count = 0;
            
        }

        public void Add(T item)
        {
            if (this.array.Length == Count)
            {
                this.Resize();
            }
            array[this.Count++] = item;
        }

        private void Resize()
        {
            T[] TempArray = new T[this.array.Length * 2];
            for (int i = 0; i < this.array.Length; i++)
            {
                TempArray[i] = array[i];
            }
            this.array = TempArray;
        }

        public void Clear()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this.array[i] = default(T);
            }
            this.Count = 0;
        }

        public bool Contains(T element)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.array[i].Equals(element))
                {
                    return true;
                }
            }
            return false;
        }

        public int CountGreaterThan(T element)
        {
            int result = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this.array[i].CompareTo(element) > 0)
                {
                    result++;
                }
            }
            return result;
        }

        public int IndexOf(T item)
        {
            int index = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this.array[i].Equals(item))
                {
                    index = i;
                }
            }
            return index;
        }

        public void Insert(T item, int index)
        {
            for (int i = index; i < this.Count+1; i++)
            {          
                this.array[i + 1] = this.array[i];   
            }
            this.array[index] = item;
            Count++;
        }

        public void Remove(T item)
        {
         
            if (this.Count == 0)
            {
                throw new InvalidOperationException();
            }
            for (int i = 0; i < this.Count; i++)
            {
                if (this.array[i].Equals(item))
                {
                    this.array[i] = default(T);
                    this.Count--;
                    for (int y = i; y < this.Count; y++)
                    {
                        this.array[i] = this.array[i + 1];

                    }                
                }
            }
          
        }

        public void Swap(int index1, int index2)
        {
            T tempVar = array[index1];
            array[index1] = array[index2];
            array[index2] = tempVar;
        }
    }
}
