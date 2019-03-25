using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCollector
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            //IDisposable_SimpleExample ();
            IDisposable_DisposeResourceWrapper ();
        }

        private static void GarbageCollector_DeleteUnusedObjects ()
        {
            Test[] tests = new Test[1000];

            try
            {
                for (int i = 0; i < tests.Length; i++)
                {
                    Test test = new Test ();
                    test.PrintElementAt (i);
                }
            }
            catch (OutOfMemoryException ex)
            {
                ConsoleColor temp = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine (ex.Message);
                Console.WriteLine ("Heap is overloaded!");
                Console.ForegroundColor = temp;
            }
        }

        private static void GarbageCollector_CannotDeleteUnusedObjects ()
        {
            Test[] tests = new Test[1000];

            try
            {
                for (int i = 0; i < tests.Length; i++)
                {
                    tests[i] = new Test ();
                    tests[i].PrintElementAt (i);
                }
            }
            catch (OutOfMemoryException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine (ex.Message);
                Console.WriteLine ("Heap is overloaded!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private static void IDisposable_SimpleExample ()
        {
            DisposeExample instance = new DisposeExample ();
            if (instance is DisposeExample)
            {
                instance.Dispose ();
            }

            Console.WriteLine (new string ('-', 30));
            using (instance = new DisposeExample ())
            {

            }

            Console.ReadKey ();
        }

        private static void IDisposable_DisposeResourceWrapper ()
        {
            PrintText ("Use 'using' keyword", ConsoleColor.Green);

            using (ResourceWrapper wrapper = new ResourceWrapper ())
            {
                Console.WriteLine (wrapper);
            }

            PrintText ("Use explicit form", ConsoleColor.Green);

            ResourceWrapper secondWrapper = new ResourceWrapper ();
            Console.WriteLine (secondWrapper);

            secondWrapper.Dispose ();
            secondWrapper.Dispose ();
            secondWrapper.Dispose ();
            secondWrapper.Dispose ();
            secondWrapper.Dispose ();

            PrintText ("Use destructor", ConsoleColor.Green);

            ResourceWrapper thirdWrapper = new ResourceWrapper ();

            Console.WriteLine ("Press any key to dispose and exit...");
            Console.ReadKey ();
        }

        private static void PrintText (string text, ConsoleColor color)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (text);
            Console.ForegroundColor = temp;
        }
    }

    internal class Test
    {
        private int[] array = new int[1000_000_000];

        public void PrintElementAt (int index)
        {
            Console.WriteLine (array[index]);
        }

        ~Test ()
        {
            Console.WriteLine ("Object " + this.GetHashCode () + " was deleted.");
        }
    }

    internal class DisposeExample : IDisposable
    {
        public void Dispose ()
        {
            Console.WriteLine ("The Dispose method is complete his work: " + this.GetHashCode ());
        }

        ~DisposeExample ()
        {
            Console.WriteLine (new string ('-', 30));
        }
    }

    internal class ResourceWrapper : IDisposable
    {
        private bool disposed;

        ~ResourceWrapper ()
        {
            Console.WriteLine ("Use Destructor!");
            CleanUp (false);
            Console.WriteLine ();
        }

        public void Dispose ()
        {
            Console.WriteLine ("Use Dispose()!");
            CleanUp (true);
            GC.SuppressFinalize (this);
            Console.WriteLine ();
        }

        private void CleanUp (bool clean)
        {
            if (disposed)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine ("Resources already was cleaned.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            if (!disposed)
            {
                if (clean)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine ("Start cleaning resources.");
                    Console.WriteLine (new string ('/', 35));

                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine ("End of the cleaning resources.");
            }
            disposed = true;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
