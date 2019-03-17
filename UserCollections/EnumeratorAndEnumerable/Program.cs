using System;
using System.Collections;
using System.Collections.Generic;
using EnumeratorAndEnumerable.Collection;

namespace EnumeratorAndEnumerable
{
    class Program
    {
        static void Main (string[] args)
        {
            UserCollection collection = new UserCollection (5);
            collection[0] = new Element (1, 2);
            collection[1] = new Element (2, 3);
            collection[2] = new Element (3, 4);
            collection[3] = new Element (4, 5);
            collection[4] = new Element (5, 6);

            Console.WriteLine ("Foreach iteration");
            foreach (Element element in collection)
            {
                Console.WriteLine ("{0} {1}", element.FieldA, element.FieldB);
            }

            Console.WriteLine (new string ('-', 5));
            Console.WriteLine ("While iteration");
            IEnumerator enumerator = collection.GetEnumerator ();
            while (enumerator.MoveNext ())
            {
                var element = enumerator.Current as Element;
                Console.WriteLine ("{0} {1}", element.FieldA, element.FieldB);
            }
        }
    }
}
