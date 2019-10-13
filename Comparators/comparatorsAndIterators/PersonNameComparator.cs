using System;
using System.Collections.Generic;
using System.Text;

namespace comparatorsAndIterators
{
    public class PersonNameComparator : IComparer<Person>
    {
        public int Compare(Person x, Person y)
        {
            int result = x.Name.Length.CompareTo(y.Name.Length);
            if(result == 0)
            {
                char xPersonLetter = Char.ToLower(x.Name[0]);
                char yPersonLetter = Char.ToLower(y.Name[0]);
                result = xPersonLetter.CompareTo(yPersonLetter);
            }
            return result;
        }
    }
}
