using System;
using System.Collections.Generic;
using System.Linq;

namespace comparatorsAndIterators
{
    class StartUp
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            PersonNameComparator nameComparer = new PersonNameComparator();
            PersonAgeComparator ageComparer = new PersonAgeComparator();
            SortedSet<Person> nameSorted = new SortedSet<Person>(nameComparer);
            SortedSet<Person> ageSorted = new SortedSet<Person>(ageComparer);
            for (int i = 0; i < n; i++)                     
            {
                string input = Console.ReadLine();
                string[] personArgs = input.Split().ToArray();
                string name = personArgs[0];
                int age = int.Parse(personArgs[1]);
       
                Person person = new Person(name, age);

                nameSorted.Add(person);
                ageSorted.Add(person);
            }
            Console.WriteLine(string.Join(Environment.NewLine,nameSorted));
            Console.WriteLine(string.Join(Environment.NewLine, ageSorted));
        }
    }
}
