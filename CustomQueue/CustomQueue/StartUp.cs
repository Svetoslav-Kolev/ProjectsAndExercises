using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CustomQueue
{
    class StartUp
    {
        static void Main(string[] args)
        {
            ICustomQueue<int> queue = new CustomQueue<int>();
            queue.Enqueue(1);
            queue.Dequeue();
            queue.Enqueue(1);
            foreach (var item in queue)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(queue.Count);
        }
    }
}
