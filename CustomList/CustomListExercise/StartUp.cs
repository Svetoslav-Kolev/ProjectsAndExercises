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
            Console.WriteLine("removing element '30'");
            myList.Remove(30);
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            myList.Remove(40);
            Console.WriteLine("removing element '40'");
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            myList.Swap(0, 3);
            Console.WriteLine("Swapping elements at index 0 and 3");
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Inserting elements 3 ,4 ,5 ,6 ,7 all at index 1");
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
            Console.WriteLine("removing elements 3 ,4 ,5 ,6");
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
            Console.WriteLine("print index of element '3'");
            Console.WriteLine(myList.IndexOf(3));
            Console.WriteLine("check if list contains element '3'");
            Console.WriteLine(myList.Contains(3));
            Console.WriteLine("check if list contains element '4'");
            Console.WriteLine(myList.Contains(4));
            Console.WriteLine("print index of element '5' , if -1 = list does not contain 5");
            Console.WriteLine(myList.IndexOf(5));
            Console.WriteLine("swap index 4 with index 0");
            myList.Swap(4,0);
            Console.WriteLine("swap index 2 with index 1");
            myList.Swap(2, 1);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("insert element 3 at index 0");
            myList.Insert(3, 0);
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("printing elements at index 3 and 1 with indexing");
            Console.WriteLine(myList[3]);
            Console.WriteLine(myList[1]);
            Console.WriteLine("print number of elements bigger than 15");
            Console.WriteLine(myList.CountGreaterThan(15));
            Console.WriteLine("print number of elements bigger than 5");
            Console.WriteLine(myList.CountGreaterThan(5));
            Console.WriteLine("Clear List");
            myList.Clear();
            Console.WriteLine("capacity: " + myList.capacity);
            Console.WriteLine("count: " + myList.Count);


        }
    }
}
