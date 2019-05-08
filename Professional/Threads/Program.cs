using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            //UseThread ();
            //StartThreadJoining ();
            //RecursiveThreadCreator ();
            //ThreadAborting ();
            //StartMutex ();
            //ThreadPoolCallAsync ();
            ThreadPoolExecutionContext ();
        }

        #region Use Thread

        public static void UseThread ()
        {
            Thread thread = new Thread (new ThreadStart (ThreadFunction));

            Console.WriteLine ("Id of he current thread: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine ("Start the first thread...");

            thread.Start ();
            Console.WriteLine (Thread.CurrentThread.GetHashCode ());

            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < 160; i++)
            {
                Thread.Sleep (20);
                Console.Write ('-');
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine ("\nThe first thread was completed.");
        }

        public static void ThreadFunction ()
        {
            Console.WriteLine ("ID of a new created thread: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < 160; i++)
            {
                Thread.Sleep (20);
                Console.Write (".");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine ("\nEnd of the created thread.");
        }

        #endregion

        #region Use Thread Join

        public static void StartThreadJoining ()
        {
            Console.WriteLine (" - The first thread: {0}", Thread.CurrentThread.ManagedThreadId);
            WriteChar ('1', 80, ConsoleColor.Green);

            Thread thread = new Thread (FirstMethodToJoin);
            thread.Start ();
            thread.Join ();

            WriteChar ('1', 80, ConsoleColor.DarkGreen);
            Console.WriteLine (" - The first thread was completed.");
        }

        private static void FirstMethodToJoin ()
        {
            Console.WriteLine (" - The second thread: {0}", Thread.CurrentThread.ManagedThreadId);
            WriteChar ('2', 80, ConsoleColor.Cyan);

            Thread thread = new Thread (SecondMethodToJoin);
            thread.Start ();
            thread.Join ();

            WriteChar ('2', 80, ConsoleColor.DarkCyan);
            Console.WriteLine (" - The second thread was completed.");
        }

        private static void SecondMethodToJoin ()
        {
            Console.WriteLine (" - The third thread: {0}", Thread.CurrentThread.ManagedThreadId);
            WriteChar ('3', 80, ConsoleColor.Yellow);
            Console.WriteLine (" - The third thread was completed.");
        }

        private static void WriteChar (char character, int count, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep (10);
                Console.Write (character);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        #endregion

        #region Thread Creator

        private static int Counter;

        public static void RecursiveThreadCreator ()
        {
            if (Counter < 100)
            {
                ++Counter;
                Console.WriteLine ("Start: {0} thread", Thread.CurrentThread.GetHashCode ());

                Thread thread = new Thread (RecursiveThreadCreator);
                thread.Start ();
                thread.Join ();
            }

            Console.WriteLine ("Thread {0} was completed.", Thread.CurrentThread.GetHashCode ());
        }

        #endregion

        #region Thread Abort

        private static void ThreadAborting ()
        {
            Thread thread = new Thread (new ThreadStart (FunctionForAbort));
            thread.Start ();

            Thread.Sleep (2000);
            thread.Abort ();
            thread.Join ();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine ("\n" + new string ('-', 80));
        }

        private static void FunctionForAbort ()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            while (true)
            {
                try
                {
                    Thread.Sleep (10);
                    Console.Write ('.');
                }
                catch (ThreadAbortException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine ("\nThreadAbortException");

                    for (int i = 0; i < 160; i++)
                    {
                        Thread.Sleep (10);
                        Console.Write ('.');
                    }

                    Console.ForegroundColor = ConsoleColor.Green;

                    Thread.ResetAbort ();
                }
            }
        }

        #endregion

        #region Mutex

        private static readonly Mutex mutex = new Mutex (false, "MutexSample");

        public static void StartMutex ()
        {
            Thread[] threads = new Thread[5];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread (new ThreadStart (MutexFunction)) {
                    Name = i.ToString ()
                };
                threads[i].Start ();
            }
        }

        public static void MutexFunction ()
        {
            bool currentMutex = mutex.WaitOne ();

            Console.WriteLine ("Thread {0} came in to a private field.", Thread.CurrentThread.Name);
            Thread.Sleep (100);

            Console.WriteLine ("Thread {0} came out from the private field.", Thread.CurrentThread.Name);
            mutex.ReleaseMutex ();
        }

        #endregion

        #region ThreadPool

        public static void ThreadPoolCallAsync ()
        {
            Console.WriteLine ("Main thread: quiring an aync operation");
            ThreadPool.QueueUserWorkItem (ComputeBoundOp, 5);

            Console.WriteLine ("Main thread: Doing other work...");
            Thread.Sleep (500);

            Console.WriteLine ("Hit <Any Key> to end this program...");
        }

        private static void ComputeBoundOp (object state)
        {
            Console.WriteLine ("Is ComputeBoundOp: state={0}", state);
        }

        public static void ThreadPoolExecutionContext ()
        {
            CallContext.LogicalSetData ("Name", "LeoDeg");
            ThreadPool.QueueUserWorkItem (state => Console.WriteLine ("Name= {0}", CallContext.LogicalGetData ("Name")));

            ExecutionContext.SuppressFlow ();
            ThreadPool.QueueUserWorkItem (state => Console.WriteLine ("Name= {0}", CallContext.LogicalGetData ("Name")));

            ExecutionContext.RestoreFlow ();
        }

        #endregion
    }
}
