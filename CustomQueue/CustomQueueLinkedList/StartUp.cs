using System;
using System.Collections.Generic;
using System.Diagnostics;
using CustomQueueLinkedList.Classes;

namespace CustomQueueLinkedList
{
    class StartUp
    {
        static void Main(string[] args)
        {
            LinkedQueue<int> linkedQ = new LinkedQueue<int>();
            linkedQ.Enqueue(10);
            linkedQ.Dequeue();
            linkedQ.Enqueue(1);
            foreach (var item in linkedQ)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(linkedQ.Count);
        }
    }
}
