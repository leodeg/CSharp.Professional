using System;
using System.Collections;
using System.Collections.Generic;

namespace SystemCollections
{
    class Program
    {
        static void Main (string[] args)
        {
            HashTable_Test ();
        }

        static void ArrayList_Test ()
        {

        }

        static void HashTable_Test ()
        {
            Hashtable emails = new Hashtable () 
            {
                {"john@yandex.ru", "Brown, John"},
                {"chess@yandex.ru", "Kenny, Lukas"},
                {"superstar@yandex.ru", "Rondos, Lee"}
            };

            Console.WriteLine (new string ('-', 20));
            WriteText ("Type:", ConsoleColor.Yellow);
            foreach (object name in emails)
            {
                Console.WriteLine (name);
            }

            Console.WriteLine (new string ('-', 20));
            WriteText ("Values:", ConsoleColor.Yellow);
            foreach (DictionaryEntry name in emails)
            {
                Console.WriteLine (name.Value);
            }

            Console.WriteLine (new string ('-', 20));
            WriteText ("Values:", ConsoleColor.Yellow);
            foreach (object name in emails.Values)
            {
                Console.WriteLine (name);
            }

            Console.WriteLine (new string ('-', 20));
            WriteText ("Keys:", ConsoleColor.Yellow);
            foreach (object name in emails.Keys)
            {
                Console.WriteLine (name);
            }

            Console.WriteLine (new string ('-', 20));
            WriteText ("Hashtable Properties:", ConsoleColor.Yellow);
            Console.WriteLine ("Count: " + emails.Count.ToString ());
            Console.WriteLine ("IsFixedSize: " + emails.IsFixedSize);
            Console.WriteLine ("IsReadOnly: " + emails.IsReadOnly);
            Console.WriteLine ("IsSynchronized: " + emails.IsSynchronized);
            Console.WriteLine ("SyncRoot: " + emails.SyncRoot);

            Console.ReadKey ();
        }

        static void WriteText (string text, ConsoleColor color)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (text);
            Console.ForegroundColor = temp;
        }
    }
}
