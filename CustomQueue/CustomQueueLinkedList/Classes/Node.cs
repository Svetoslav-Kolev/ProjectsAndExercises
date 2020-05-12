using System;
using System.Collections.Generic;
using System.Text;

namespace CustomQueueLinkedList.Classes
{
    public class Node<T>
    {
        public T Item { get; set; }
        public Node<T> Next;
        public Node(T item , Node<T> node)
        {
            this.Item = item;
            this.Next = node;
        }
    }
}
