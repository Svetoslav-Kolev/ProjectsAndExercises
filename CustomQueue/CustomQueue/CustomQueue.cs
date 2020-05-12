using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CustomQueue
{
    class CustomQueue<T> : ICustomQueue<T>, IEnumerable<T>
    {
        private const int StartCapacity = 4;

        private T[] innerArray;
        private int front;
        private int rear;

        public CustomQueue()
        {
            this.Count = 0;
            this.front = -1;
            this.rear = -1;
            this.Capacity = StartCapacity;
            this.innerArray = new T[this.Capacity];
        }

        public int Count { get; private set; }

        public int Capacity { get; private set; }

        public bool IsEmpty
        {
            get
            {
                if (this.Count <= 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsFull
        {
            get
            {
                if (this.Count >= Capacity)
                {
                    return true;
                }
                return false;
            }
        }

        public void Enqueue(T item)
        {
            if (this.IsEmpty)
            {
                this.front = 0;
            }
            if (this.IsFull)
            {
                //int oldCapacity = this.Capacity;
                //IncreaseSize(this.Capacity);
                IncreaseSize();
            }
            AddRearElement(item);

        }

        public T Dequeue()
        {
            T element;
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            else
            {
                //if(this.front == -1)
                //{
                //    this.front = 0;
                //}
                element = this.innerArray[front];
                if (this.front == this.rear)
                {
                    this.front = -1;
                    this.rear = -1;
                }
                else
                {
                    this.front = (this.front + 1) % this.Capacity;
                }
             
            }
            this.Count--;
            return element;
        }

        public T Peek()
        {
            return this[this.front];
        }

        private void AddRearElement(T item)
        {
            this.rear = (this.rear + 1) % this.Capacity;
            innerArray[this.rear] = item;
            this.Count++;
        }

        private void IncreaseSize()
        {
           this.Capacity *= 2;
           MakeArrayCopy(this.Capacity / 2);
        }

        private void MakeArrayCopy(int oldCapacity)
        {
            T[] tempArray = new T[this.Capacity];
            int tempCount = 0;
            if (rear < front)
            {
                for (int i = this.front; i < oldCapacity; i++)
                {
                    tempArray[tempCount] = innerArray[i];
                    tempCount ++;
                }
                for (int i = 0; i <= this.rear; i++)
                {
                    tempArray[tempCount] = innerArray[i];
                    tempCount++;
                } 
            }
            else
            {
                for (int i = this.front; i <= this.rear; i++)
                {
                    tempArray[tempCount] = innerArray[i];
                    tempCount++;
                }
            }
            this.front = 0;
            this.rear = tempCount - 1;
            this.innerArray = tempArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.Count != 0)
            {
                if (this.front > this.rear)
                {
                    for (int i = this.front; i < this.Capacity; i++)
                    {
                        yield return this.innerArray[i];
                    }
                    for (int i = 0; i <= rear; i++)
                    {
                        yield return this.innerArray[i];
                    }
                }
                else if (this.front <= this.rear)
                {
                    for (int i = front; i <= rear; i++)
                    {
                        yield return this.innerArray[i];
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private T this[int i]
        {
            get
            {
                return innerArray[i];
            }
            set
            {
                innerArray[i] = value;
            }
        }
    }
}
