using System;
using System.Collections.Generic;

namespace GenericsType
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            CallingConvertIList ();
        }

        private static void CallingConvertIList ()
        {
            // Base list of a strings
            IList<string> listString = new List<string> ();
            listString.Add ("Some string.");
            listString.Add ("Second some string.");
            listString.Add ("Third some string.");
            PrintIList (listString);

            // To object
            IList<object> listObject = ConvertList<string, object> (listString);
            PrintIList (listObject);

            // To IComparable
            IList<IComparable> listComparable = ConvertList<string, IComparable> (listString);
            PrintIList (listComparable);

            // To IComparable<string>
            IList<IComparable<string>> listComparableString = ConvertList<string, IComparable<string>> (listString);
            PrintIList (listComparableString);

            // To string
            IList<string> listStringTwo = ConvertList<string, string> (listString);
            PrintIList (listStringTwo);

            // To Exception
            //IList<Exception> listException = ConvertList<string, Exception> (listStringTwo); // Error
        }

        private static List<TBase> ConvertList<T, TBase> (IList<T> list) where T : TBase
        {
            List<TBase> baseList = new List<TBase> (list.Count);

            for (int i = 0; i < list.Count; i++)
                baseList.Add (list[i]);

            return baseList;
        }

        private static void PrintIList<T> (IList<T> list)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine ("List of type of the ({0}): ", typeof (T));
            Console.ForegroundColor = ConsoleColor.White;
            foreach (T item in list)
                Console.WriteLine (item.ToString ());
            Console.WriteLine ();
        }

    }

    #region Constraints

    public sealed class GenericEnumeratedType<T> where T : IEnumerator<T>
    {
        // Compile time error
    }

    public sealed class GenericEnumeratedType2<T>
    {
        static GenericEnumeratedType2 ()
        {
            if (!typeof (T).IsEnum)
            {
                // Runtime error
                throw new System.ArgumentException ("T must be an enumerated type!");
            }
        }
    }

    internal sealed class ConstructorConstraint<T> where T : new()
    {
        public static T CreateInstance ()
        {
            return new T ();
        }
    }

    #endregion

    #region Comparing

    internal sealed class GenericComparing
    {
        public bool IsClassesEqual<T> (T first, T second) where T : class
        {
            return first == second;
        }

        public int CompareStructures<T> (T first, T second) where T : IComparable<T>
        {
            return first.CompareTo(second);
        }
    }

    #endregion

    #region Inheritance

    internal class Base
    {
        public virtual void M<T1, T2> ()
            where T1 : struct
            where T2 : class
        {

        }
    }

    internal class Derived : Base
    {
        public override void M<T1, T2> ()
        //where T1 : EventArgs // Error: cannot specified directly inherited constraints
        //where T2 : class // Error: cannot specified directly inherited constraints
        {

        }
    }

    #endregion
}
