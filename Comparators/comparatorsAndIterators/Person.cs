using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace comparatorsAndIterators
{
    public class Person : IComparable<Person>
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public Person(string name , int age )
        {
            this.Age = age;
            this.Name = name;
 
        }
        public int CompareTo(Person other)
        {
            int result =  this.Name.CompareTo(other.Name);
            if(result == 0)
            {
                result = this.Age.CompareTo(other.Age);

            }


            return result;
            
        }
        public override string ToString()
        {
            return this.Name + " " + this.Age;
        }
    }
}
