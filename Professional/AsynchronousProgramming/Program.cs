using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    class Program
    {
        static void Main (string[] args)
        {
            //StartAsyncMethod ();
            StartFuncAsyncMethod ();
        }

        #region Asynchronous Method

        public static void StartAsyncMethod ()
        {
            Console.WriteLine ("First thread: ID:{0}", Thread.CurrentThread.ManagedThreadId);
            Action methodDelegate = new Action (AsyncMethod);
            methodDelegate.BeginInvoke (null, null);

            Console.WriteLine ("Main Start");
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep (20);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write ("Main...");
            }

            Console.WriteLine ("Main was completed");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void AsyncMethod ()
        {
            Console.WriteLine ("Asynchronous method was started.");
            Console.WriteLine ("Second thread: ID:{0}", Thread.CurrentThread.ManagedThreadId);

            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep (20);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write ("Working...");
            }

            Console.WriteLine ("Asynchronous operation was completed.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void StartFuncAsyncMethod ()
        {
            Func<int, int, int> function = new Func<int, int, int> (FuncAsyncAdd);
            IAsyncResult asyncResult = function.BeginInvoke (1, 2, null, null);
            int integerResult = function.EndInvoke (asyncResult);
            Console.WriteLine ("Result is: {0}", integerResult);
        }

        public static int FuncAsyncAdd (int a, int b)
        {
            Thread.Sleep (10);
            return a + b;
        }

        #endregion
    }
}
