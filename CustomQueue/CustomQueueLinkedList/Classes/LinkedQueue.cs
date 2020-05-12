using CustomQueueLinkedList.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CustomQueueLinkedList.Classes
{
    public class LinkedQueue<T>:ILinkedQueue<T>,IEnumerable<T>
    {
        public LinkedQueue()
        {
            this.Count = 0;
        }

        private Node<T> First { get; set; }
        private Node<T> Last { get; set; }
        public int Count { get; private set; }

        public void Enqueue(T item)
        {
            Node<T> temp = new Node<T>(item, null);
            if (this.Count == 0)
            {
                this.First = temp;
                this.Last = this.First;
            }
            else
            {
                this.Last.Next = temp;
                this.Last = temp;
            }
            this.Count++;
        }

        public T Dequeue()
        {
            if (this.Count == 0)
            {
                return default(T);
            }
            T itemtoDequeue = this.First.Item;
            this.First = this.First.Next;
            this.Count--;
            return itemtoDequeue;
           
        }

        public T Peek()
        {
            return this.First.Item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = this.First;
            for (int i = 0; i < this.Count; i++)
            {
                yield return current.Item;
                current = current.Next;

            }
                   
                

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool isEmpty
        {
            get
            {
                if (this.Count == 0)
                {
                    return true;
                }
                return false;
            }

        }
    }
}
