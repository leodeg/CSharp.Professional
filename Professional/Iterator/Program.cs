using Iterator.Pattern;

namespace Iterator
{
    class Program
    {
        static void Main (string[] args)
        {
            Aggregate aggregate = new ConcreteAggregate ();
            aggregate[0] = "Element 1";
            aggregate[1] = "Element 2";
            aggregate[2] = "Element 3";
            aggregate[3] = "Element 4";

            Iterator.Pattern.Iterator iterator = new ConcreteIterator (aggregate as ConcreteAggregate);

            System.Console.WriteLine ("First Element: ");
            object element = iterator.First ();
            System.Console.WriteLine (element);

            System.Console.WriteLine (new string ('-', 25));

            System.Console.WriteLine ("Iterations: ");
            while (!iterator.IsDone ())
            {
                System.Console.WriteLine (element);
                element = iterator.Next ();
            }

            System.Console.ReadKey ();
        }
    }
}
