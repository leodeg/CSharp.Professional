using System;
using System.Collections;
using System.Collections.Generic;

namespace WorkingWithText
{
    class Program
    {
        static void Main (string[] args)
        {
            String_Initialization ();
        }

        static private void String_Initialization ()
        {
            string str = "Hello";
            string str2 = new string ('-', 10);

            str += str2;

            string str3 = 5.ToString ();
            string str4 = String.Format ("{0} + {1} = {2}", 1, 2, 1 + 2);

            string path = "C\\windows\\system32";
            string path2 = @"C\windows\system32";

            Console.WriteLine (str);
            Console.WriteLine (str2);
            Console.WriteLine (str3);
            Console.WriteLine (str4);
            Console.WriteLine (path);
            Console.WriteLine (path2);
        }
    }
}
