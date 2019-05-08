using System;
using System.Collections.Generic;
using System.Threading;

namespace Threads
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            //Thread_CreateThread ();
            //ThreadIsDoneTest.Start ();
            Thread_Name ();
        }

        private static void Thread_CreateThread ()
        {
            Thread newThread = new Thread (delegate ()
            {
                for (int i = 0; i < 100; i++)
                    Console.WriteLine ("New thread do something");
            });

            newThread.Start ();

            for (int i = 0; i < 100; i++)
                Console.WriteLine ("Main thread do something");

            new Thread (() =>
            {
                Console.WriteLine ("Create another thread");
                Console.WriteLine ("Easy-peasy!");
            }).Start ();

            new Thread (delegate()
            {
                Console.WriteLine ("Create another thread");
                Console.WriteLine ("Easy-peasy!");
            }).Start ();
        }

        public static void Thread_Name ()
        {
            Thread.CurrentThread.Name = "Main";
            Thread worker = new Thread (() => Console.WriteLine ("Do something, thread Name: {0}", Thread.CurrentThread.Name));
            worker.Name = "Worker";
            worker.Start ();
            Console.WriteLine ("Do Something, thread Name: {0}", Thread.CurrentThread.Name );
        }

        public static void Thread_BackgroundAndForegroundThreads ()
        {

        }
    }

    class ThreadIsDoneTest
    {
        static bool isDone;
        static readonly object locker = new object ();

        public static void Start ()
        {
            new Thread (GoThread).Start ();
            GoThread ();
        }

        private static void GoThread ()
        {
            lock (locker)
            {
                if (!isDone)
                {
                    //isDone = true; // Print Done one time
                    Console.WriteLine ("Done");
                    isDone = true; // Print Done twice
                }
            }
        }
    }
}
