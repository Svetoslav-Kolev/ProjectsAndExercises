using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CustomListExercise.Contracts;

namespace CustomListExercise.Classes
{
    class CustomList<T> : ICustomList<T>, IEnumerable<T>
        where T: IComparable<T>
    {
        private const int startCapacity = 4;
        public int Count { get;private set; }
        private T[] array;
        public int capacity { get;private set; }

        public CustomList()
        {
            array = new T[startCapacity];
            capacity = startCapacity;
            this.Count = 0;
        }

        public void Add(T item)
        {
            if (this.array.Length == Count)
            {
                this.IncreaseSize();
            }
            array[this.Count++] = item;
        }

        //Extract array copy -- DONE
        private void DecreaseSize()
        {
            capacity /= 2;
            MakeArrayCopy();
        }

        private void IncreaseSize()
        {
            capacity *= 2;
            MakeArrayCopy();

        }

        public void Clear()
        {
            //Potentially new array[capacity] -- DONE
            this.array = new T[startCapacity];
            this.Count = 0;
            this.capacity = startCapacity;
        }

        public bool Contains(T element)
        {
            //Call IndexOf, if -1 false, else true -- DONE
            if (IndexOf(element) == -1)
            {
                return false;
            }
            return true;
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
            //Usually -1 when not contained -- DONE
            int index = -1;
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
            if (index >= this.Count)
            {
                throw new IndexOutOfRangeException();
            }

            if(this.Count == capacity)
            {
                this.IncreaseSize();
            }
           
            T[] TempArray = new T[this.Count];
            for (int i = index; i < this.Count; i++)
            {
                TempArray[i] = this.array[i];
            }
            for (int i = index; i < this.Count; i++)
            {
                this.array[i + 1] = TempArray[i];
            }
            this.array[index] = item;
            Count++;
        }

        public bool Remove(T item)
        {
         
            if (this.Count == 0)
            {
                throw new InvalidOperationException();
            }
            int index = IndexOf(item);
            if (index != -1)
            {
               
                this.Count--;
                for (int y = index; y < this.Count; y++)
                {
                    this.array[y] = this.array[y + 1];

                }
                if (this.Count <= capacity / 2)
                {
                    this.DecreaseSize();
                }
                
                return true;
            }
                   
            return false;
        }

        public void Swap(int index1, int index2)
        {
            if(index1>=this.Count || index2 >= this.Count)
            {
                throw new InvalidOperationException();
            }
            else
            {
                T tempVar = array[index1];
                array[index1] = array[index2];
                array[index2] = tempVar;
            }
           
        }
        private void MakeArrayCopy()
        {
            T[] TempArray = new T[capacity];
            for (int i = 0; i < this.Count; i++)
            {
                TempArray[i] = array[i];
            }
            this.array = TempArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T this[int i]
        {
            get
            {
                if(i >= this.Count || i <0)
                {
                    throw new IndexOutOfRangeException();
                }
                return array[i];
            }
            set
            {
                if (i >= this.Count || i < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                array[i] = value;
            }
        }

    }
}
