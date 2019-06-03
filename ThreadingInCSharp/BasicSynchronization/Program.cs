using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicSynchronization
{
    internal class Program
    {
        public delegate int BinaryOperation (int data, int time);
        private static AutoResetEvent waitHandle = new AutoResetEvent (false);

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
            //BGWorker.Start ();
            //RulyCancelerTest.Start ();
            //new Nonblocking ().Start ();
            //SimpleWaitPulse.Start ();
            //ProducerConsumerQueueWithPulse.Start ();
            //IAsyncResultExample ();
            //IAsyncCallbackExample ();
            //ThreadInfoExample ();
            //AutoResetEventExample ();
            //SampleThread.Start ();
            //ThreadSynchronization.Start ();
            Clocks.StartTest ();
        }

        public static void ThreadInfoExample ()
        {
            Console.WriteLine ("Information about current program.");
            Thread thread = Thread.CurrentThread;
            thread.Name = "CurrentThread";
            Console.WriteLine ("Domain name: {0}\nContext ID: {1} \nThread name: {2} \nIsAlive: {3} \nThread priority: {4} \nThread state: {5}", Thread.GetDomain ().FriendlyName, Thread.CurrentContext.ContextID, thread.Name, thread.IsAlive, thread.Priority, thread.ThreadState);
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

        public static void IAsyncResultExample ()
        {
            Console.WriteLine ("Sync method call:");
            DelegateThread (1, 3000);

            Console.WriteLine ("\nAsync method call:");
            BinaryOperation binaryOp = DelegateThread;
            IAsyncResult asyncResult = binaryOp.BeginInvoke (1, 3000, null, null);
            while (!asyncResult.IsCompleted)
            {
                Console.Write (".");
                Thread.Sleep (50);
            }

            int result = binaryOp.EndInvoke (asyncResult);
            Console.WriteLine ("\nResult: {0}", result);
            Console.ReadLine ();
        }

        private static int DelegateThread (int data, int delay)
        {
            Console.WriteLine ("Delegate Thread was started!");
            Thread.Sleep (delay);
            Console.WriteLine ("Delegate Thread was completed!");
            return ++data;
        }

        public static void IAsyncCallbackExample ()
        {
            Console.WriteLine ("Async call");
            BinaryOperation binaryOperation = DelegateThread;

            binaryOperation.BeginInvoke (1, 3000, TakesAWhileCompleted, binaryOperation);

            for (int i = 0; i < 100; i++)
            {
                Console.Write (".");
                Thread.Sleep (50);
            }
        }

        private static void TakesAWhileCompleted (IAsyncResult ar)
        {
            if (ar == null) throw new ArgumentNullException ();

            BinaryOperation binaryOperation = ar.AsyncState as BinaryOperation;
            Trace.Assert (binaryOperation != null, "Wrong type.");
            int result = binaryOperation.EndInvoke (ar);
            Console.WriteLine ("Result: {0}", result);
        }

        public static void AutoResetEventExample ()
        {
            Console.WriteLine ("Main thread. ID: {0}", Thread.CurrentThread.ManagedThreadId);

            Params param = new Params (10, 10);
            Thread thread = new Thread (new ParameterizedThreadStart (Add));
            Thread.Sleep (5);

            waitHandle.WaitOne ();
            Console.WriteLine ("All threads is completed");
            Console.ReadLine ();
        }

        private static void Add (object obj)
        {
            if (obj is Params)
            {
                Console.WriteLine ("ID thread of method Add(): {0}", Thread.CurrentThread.ManagedThreadId);
                Params param = (Params)obj;
                Console.WriteLine ("{0} + {1} = {2}", param.a, param.b, param.a + param.b);
                waitHandle.Set ();
            }
        }
    }

    internal class Params
    {
        public int a, b;

        public Params (int a, int b)
        {
            this.a = a;
            this.b = b;
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

    #region Resources Contest

    public class StateObject
    {
        private int state = 5;
        private static readonly object locker = new object ();

        public void ChangeState (int state)
        {
            if (state == 5)
            {
                this.state++;
                Trace.Assert (this.state == 6, "Resources contest was arose after " + state + " loops");
            }
            state = 5;
        }
    }

    public class SampleThread
    {
        public static void Start ()
        {
            var state = new StateObject ();
            for (int i = 0; i < 20; i++)
                new Task (new SampleThread ().RaceCondition, state).Start ();
            Thread.Sleep (10000);
        }

        public void RaceCondition (object obj)
        {
            Trace.Assert (obj is StateObject, "obj must have type of StateObject");
            StateObject state = obj as StateObject;
            int i = 0;
            while (true)
            {
                state.ChangeState (i++);
            }
        }
    }

    #endregion

    #region Synchronization

    public class ThreadSynchronization
    {
        private object threadLock = new object ();

        public static void Start ()
        {
            var synchronization = new ThreadSynchronization ();
            Thread[] threads = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread (new ThreadStart (synchronization.ThreadNumber));
                threads[i].Name = string.Format ("Working thread: #{0}", i);
            }

            foreach (Thread thread in threads)
                thread.Start ();

            Console.ReadLine ();
        }

        public void ThreadNumber ()
        {
            lock (threadLock)
            {
                Console.WriteLine ("{0} thread use method ThreadNumber", Thread.CurrentThread.Name);

                Console.WriteLine ("Numbers:");
                for (int i = 0; i < 10; i++)
                {
                    Random random = new Random ();
                    Thread.Sleep (200 * random.Next (5));
                    Console.Write (i + " ");
                }

                Console.WriteLine ();
            }
        }
    }

    #endregion

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
            for (int i = 0; i <= 100; i += 20)
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

    #region Safe Cancellation

    internal class RulyCanceler
    {
        private object cancelLocker = new object ();
        private bool cancelRequest;

        public bool IsCacellationRequested
        {
            get { lock (cancelLocker) return cancelRequest; }
        }

        public void Cancel ()
        {
            lock (cancelLocker)
                cancelRequest = true;
        }

        public void ThrowIfCancellationRequested ()
        {
            if (IsCacellationRequested)
                throw new OperationCanceledException ();
        }
    }

    internal class RulyCancelerTest
    {
        public static void Start ()
        {
            RulyCanceler canceler = new RulyCanceler ();
            new Thread (() =>
            {
                try { DoWork (canceler); }
                catch (OperationCanceledException ex)
                { Console.WriteLine ("Canceled!"); }
            }).Start ();

            Thread.Sleep (1000);
            canceler.Cancel ();
        }

        private static void DoWork (RulyCanceler canceler)
        {
            while (true)
            {
                canceler.ThrowIfCancellationRequested ();
                try { OtherMethod (canceler); }
                finally { /* cleanup */ }
            }
        }

        private static void OtherMethod (RulyCanceler canceler)
        {
            canceler.ThrowIfCancellationRequested ();
        }
    }

    #endregion

    #region Nonblocking Synchronization

    internal class Nonblocking
    {
        private int answer;
        private bool complete;

        public void Start ()
        {
            DoSomething ();
            DoSomethingElse ();
        }

        private void DoSomething ()
        {
            answer = 123;
            Thread.MemoryBarrier ();
            complete = true;
            Thread.MemoryBarrier ();
        }

        private void DoSomethingElse ()
        {
            Thread.MemoryBarrier ();
            if (complete)
            {
                Thread.MemoryBarrier ();
                Console.WriteLine (answer);
            }
        }
    }

    #endregion

    #region Wait Pulse

    internal class SimpleWaitPulse
    {
        private static readonly object locker = new object ();
        private static bool go;

        public static void Start ()
        {
            new Thread (Work).Start ();
            Console.ReadLine ();

            lock (locker)
            {
                go = true;
                Monitor.Pulse (locker);
            }
        }

        private static void Work ()
        {
            lock (locker)
                while (!go)
                    Monitor.Wait (locker);

            Console.WriteLine ("Done!!!");
        }
    }

    internal class TickTock
    {
        private object locker = new object ();

        public void Tick (bool running)
        {
            lock (locker)
            {
                if (!running)
                {
                    // Stop clock
                    Monitor.Pulse (locker);
                    return;
                }

                Console.Write ("Tick ");
                // Start Tock method
                Monitor.Pulse (locker);
                // Wait completed of Tock method
                Monitor.Wait (locker);
            }
        }

        public void Tock (bool running)
        {
            lock (locker)
            {
                if (!running)
                {
                    // Stop clock
                    Monitor.Pulse (locker);
                    return;
                }

                Console.WriteLine ("Tock ");
                // Start Tick method
                Monitor.Pulse (locker);
                // Wait completed of Tick method
                Monitor.Wait (locker);
            }
        }
    }

    internal class Clocks
    {
        public Thread thread;
        TickTock clocks;

        public Clocks (string threadName, TickTock tickTock)
        {
            thread = new Thread (this.Run);
            clocks = tickTock;
            thread.Name = threadName;
            thread.Start ();
        }

        private void Run ()
        {
            if (thread.Name == "Tick")
            {
                for (int i = 0; i < 5; i++)
                    clocks.Tick (true);
                clocks.Tick (false);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    clocks.Tock (true);
                clocks.Tock (false);
            }
        }

        public static void StartTest ()
        {
            TickTock tickTock = new TickTock ();
            Clocks tick = new Clocks ("Tick", tickTock);
            Clocks tock = new Clocks ("Tock", tickTock);

            tick.thread.Join ();
            tock.thread.Join ();

            Console.WriteLine ("The clocks was stopped.");
            Console.ReadLine ();
        }
    }

    #endregion

    #region Producer/Consumer Queue

    internal class ProducerConsumerQueueWithPulse
    {
        private Thread[] workers;
        private readonly object locker = new object ();
        private Queue<Action> itemQueue = new Queue<Action> ();

        public static void Start ()
        {
            var queue = new ProducerConsumerQueueWithPulse (2);
            Console.WriteLine ("Enqueuing 10 items...");

            for (int i = 0; i < 10; i++)
            {
                int itemNumber = i;
                queue.EnqueueItem (() =>
                {
                    Thread.Sleep (1000);
                    Console.WriteLine ("Task: {0}", itemNumber);
                });
            }

            queue.Shutdown (true);
            Console.WriteLine ();
            Console.WriteLine ("Workers complete!");
        }

        public ProducerConsumerQueueWithPulse (int workerCount)
        {
            workers = new Thread[workerCount];

            for (int i = 0; i < workerCount; i++)
                (workers[i] = new Thread (Consume)).Start ();
        }

        public void Shutdown (bool waitForWorkers)
        {
            foreach (Thread worker in workers)
                EnqueueItem (null);

            if (waitForWorkers)
                foreach (Thread worker in workers)
                    worker.Join ();
        }

        public void EnqueueItem (Action item)
        {
            lock (locker)
            {
                itemQueue.Enqueue (item);
                Monitor.Pulse (locker);
            }
        }

        private void Consume ()
        {
            while (true)
            {
                Action item;
                lock (locker)
                {
                    while (itemQueue.Count == 0)
                        Monitor.Wait (locker);
                    item = itemQueue.Dequeue ();
                }

                if (item == null)
                    return;
                item ();
            }
        }
    }

    #endregion
}
