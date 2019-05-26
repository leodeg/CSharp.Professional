using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;

namespace BasicSynchronization
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            //ThreadSafe.Start ();
            //TwoWaySignaling.Start ();
            //ProducerConsumerQueueStart ();
            //CountdownEventExample.Start ();
            //WaitHandles.Start ();
            //new AutoLockTest ().Start ();
            //AutoLockTest2.Start ();
            //Deadlock.Start ();
            BGWorker.Start ();
        }

        public static void ProducerConsumerQueueStart ()
        {
            using (ProducerConsumerQueue queue = new ProducerConsumerQueue ())
            {
                queue.EnqueueTask ("Hello");
                for (int i = 0; i < 10; i++)
                    queue.EnqueueTask ("Say: " + i);
                queue.EnqueueTask ("Goodbye!");
            }
        }
    }

    internal class ThreadSafe
    {
        private static List<string> list = new List<string> ();

        public static void Start ()
        {
            new Thread (AddItem).Start ();
            new Thread (AddItem).Start ();
        }

        private static void AddItem ()
        {
            string[] items;

            lock (list)
                list.Add ("Item " + list.Count);
            lock (list)
                items = list.ToArray ();

            foreach (string item in items)
                Console.WriteLine (item);
        }
    }

    internal class TwoWaySignaling
    {
        private static EventWaitHandle ready = new AutoResetEvent (false);
        private static EventWaitHandle go = new AutoResetEvent (false);
        private static readonly object locker = new object ();
        private static string message;

        public static void Start ()
        {
            new Thread (Work).Start ();

            ready.WaitOne (); // Wait unit worker is ready
            lock (locker) message = "ooo";
            go.Set ();

            ready.WaitOne ();
            lock (locker) message = "ahh"; // Set another message to worker
            go.Set ();

            ready.WaitOne ();
            //lock (locker) message = string.Empty; // Infinity loop
            lock (locker) message = null; // Signal to exit
            go.Set ();
        }

        private static void Work ()
        {
            while (true)
            {
                ready.Set ();
                go.WaitOne ();
                lock (locker)
                {
                    if (message == null) return;
                    Console.WriteLine (message);
                }
            }
        }
    }

    internal class ProducerConsumerQueue : IDisposable
    {
        private Thread worker;
        private readonly object locker = new object ();
        private Queue<string> tasks = new Queue<string> ();
        private EventWaitHandle waitHandle = new AutoResetEvent (false);

        public ProducerConsumerQueue ()
        {
            worker = new Thread (Work);
            worker.Start ();
        }

        public void EnqueueTask (string task)
        {
            lock (locker)
            {
                tasks.Enqueue (task);
            }

            waitHandle.Set ();
        }

        private void Work ()
        {
            while (true)
            {
                string task = null;

                lock (locker)
                {
                    if (tasks.Count > 0)
                    {
                        task = tasks.Dequeue ();
                        if (task == null)
                            return;
                    }
                }

                if (task != null)
                {
                    Console.WriteLine ("Performing task: " + task);
                    Thread.Sleep (500);
                }
                else
                {
                    waitHandle.WaitOne ();
                }
            }
        }

        public void Dispose ()
        {
            EnqueueTask (null);     // Exit signal
            worker.Join ();         // Wait for the thread to finish
            waitHandle.Close ();    // Release any OS resources
        }
    }

    internal class CountdownEventExample
    {
        private static CountdownEvent countdown = new CountdownEvent (3);

        public static void Start ()
        {
            new Thread (SaySomething).Start ("I am thread 1");
            new Thread (SaySomething).Start ("I am thread 2");
            new Thread (SaySomething).Start ("I am thread 3");

            countdown.Wait ();
            Console.WriteLine ("All threads have finished");
        }

        private static void SaySomething (object thing)
        {
            Thread.Sleep (1000);
            Console.WriteLine (thing.ToString ());
            countdown.Signal ();
        }
    }

    internal class WaitHandles
    {
        private static ManualResetEvent starter = new ManualResetEvent (false);

        public static void Start ()
        {
            RegisteredWaitHandle register = ThreadPool.RegisterWaitForSingleObject (starter, Go, "Some State", -1, true);

            Thread.Sleep (1000);
            Console.WriteLine ("Signaling worker...");

            starter.Set ();
            Console.ReadLine ();
            Console.WriteLine ("Press <Any key> to continue...");
            register.Unregister (starter);
        }

        private static void Go (object state, bool timedOut)
        {
            Console.WriteLine ("Started - " + state);
            Console.WriteLine ("Press <Any key> to continue...");
        }
    }

    #region Synchronization Context

    [Synchronization]
    internal class AutoLock : ContextBoundObject
    {
        public void Demo ()
        {
            Console.WriteLine ("Start...");
            Thread.Sleep (1000);
            Console.WriteLine ("End...");
        }
    }

    internal class AutoLockTest
    {
        public void Start ()
        {
            AutoLock safeInstance = new AutoLock ();
            new Thread (safeInstance.Demo).Start ();
            new Thread (safeInstance.Demo).Start ();
            safeInstance.Demo ();
        }
    }

    [Synchronization]
    internal class AutoLockTest2
    {
        public void Demo ()
        {
            Console.WriteLine ("Start...");
            Thread.Sleep (1000);
            Console.WriteLine ("End...");
        }

        public void Test ()
        {
            new Thread (Demo).Start ();
            new Thread (Demo).Start ();
            new Thread (Demo).Start ();
            Console.ReadLine ();
        }

        public static void Start ()
        {
            new AutoLockTest2 ().Test ();
        }
    }

    [Synchronization]
    public class Deadlock : ContextBoundObject
    {
        public Deadlock other;

        public void Demo ()
        {
            Thread.Sleep (1000);
            other.Hello ();
        }

        public void Hello ()
        {
            Console.WriteLine ("Hello...");
        }

        public static void Start ()
        {
            Deadlock dead1 = new Deadlock ();
            Deadlock dead2 = new Deadlock ();

            dead1.other = dead2;
            dead2.other = dead1;

            new Thread (dead1.Demo).Start ();
            dead2.Demo ();
        }
    }

    #endregion

    #region Background Worker

    internal class BGWorker
    {
        private static BackgroundWorker BackgroundWorker;

        public static void Start ()
        {
            BackgroundWorker = new BackgroundWorker ()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            BackgroundWorker.DoWork += DoWork;
            BackgroundWorker.ProgressChanged += ProgressChanged;
            BackgroundWorker.RunWorkerCompleted += RunWorkerCompleted;
            BackgroundWorker.RunWorkerAsync ("Message to worker");

            Console.ReadLine ();

            if (BackgroundWorker.IsBusy)
                BackgroundWorker.CancelAsync ();

            Console.ReadLine ();
        }

        private static void DoWork (object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i+=20)
            {
                if (BackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                BackgroundWorker.ReportProgress (i);
                Thread.Sleep (700);
            }

            e.Result = 123;
        }

        private static void ProgressChanged (object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine ("Reached {0}%", e.ProgressPercentage);
        }

        private static void RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                Console.WriteLine ("Canceled!");
            else if (e.Error != null)
                Console.WriteLine ("Worker Exception: {0}", e.Error.ToString ());
            else Console.WriteLine ("Completed Result: {0}", e.Result);
        }
    }

    #endregion
}
