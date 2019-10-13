using System;
using CustomListExercise.Classes;
using CustomListExercise.Contracts;

namespace CustomListExercise
{
   public class CustomList
    {
        static void Main(string[] args)
        {
            ICustomList<string> myList = new CustomList<string>();

            myList.Add("test");
            myList.Add("test2");
            myList.Insert("test1", 1);
            Console.WriteLine(myList.IndexOf("test1"));
            
        }
    }
}
