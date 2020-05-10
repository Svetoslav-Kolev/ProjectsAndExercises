using System;
using CustomListExercise.Classes;
using CustomListExercise.Contracts;
using System.Collections.Generic;

namespace CustomListExercise
{
   public class CustomList
    {
        static void Main(string[] args)
        {
            ICustomList<int> myList = new CustomList<int>();
            Console.WriteLine("Adding elements 10 , 20 , 30 ,40 , 50 - Inserting value 3 on index 1 - then printing");
            myList.Add(10);
            myList.Add(20);
            myList.Add(30);
            myList.Add(40);
            myList.Add(50);
            myList.Insert(3, 1);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("capacity: "+myList.capacity);
            Console.WriteLine("count: " + myList.Count);
            myList.Remove(30);
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            myList.Remove(40);
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            myList.Swap(0, 3);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            myList.Insert(3, 1);
            myList.Insert(4, 1);
            myList.Insert(5, 1);
            myList.Insert(6, 1);
            myList.Insert(7, 1);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);
            myList.Remove(3);
            myList.Remove(4);
            myList.Remove(5);
            myList.Remove(6);
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(myList.IndexOf(3));
            Console.WriteLine(myList.Contains(3));
            Console.WriteLine(myList.Contains(4));
            Console.WriteLine(myList.IndexOf(5));
            myList.Swap(4,0);
            myList.Swap(2, 1);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            myList.Clear();
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);


        }
    }
}
