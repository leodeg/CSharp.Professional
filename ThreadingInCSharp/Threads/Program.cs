using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            //Thread_CreateThread ();
            //ThreadIsDoneTest.Start ();
            //Thread_Name ();
            //ThreadPool_Entering ();
            //AsyncDelegate ();
            Thread_Wait ();
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

            new Thread (delegate ()
            {
                Console.WriteLine ("Create another thread");
                Console.WriteLine ("Easy-peasy!");
            }).Start ();
        }

        private static void Thread_Wait ()
        {
            string text = "text1";
            Thread first = new Thread (() => Console.WriteLine (text));
            first.Start ();

            text = "text2";
            Thread second = new Thread (() => Console.WriteLine (text));
            second.Start ();

            // output: 
            // text2
            // text2
        }

        public static void Thread_Name ()
        {
            Thread.CurrentThread.Name = "Main";
            Thread worker = new Thread (() => Console.WriteLine ("Do something, thread Name: {0}", Thread.CurrentThread.Name));
            worker.Name = "Worker";
            worker.Start ();
            Console.WriteLine ("Do Something, thread Name: {0}", Thread.CurrentThread.Name);
        }

        public static void ThreadPool_Entering ()
        {
            Task newTask = Task.Factory.StartNew (() => Console.WriteLine ("Start from task pool"));
            newTask.Wait ();
        }

        public static void AsyncDelegate ()
        {
            Func<string, int> method = (string s) => s.Length;
            IAsyncResult cookie = method.BeginInvoke ("test", null, null);

            // Do something in parallel...

            int result = method.EndInvoke (cookie);
            Console.WriteLine ("String length is: {0}", result);
        }
    }

    internal class ThreadIsDoneTest
    {
        private static bool isDone;
        private static readonly object locker = new object ();

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
