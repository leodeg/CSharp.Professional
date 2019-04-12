using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FunctionalProgramming
{
    internal class Program
    {
        private static void Main (string [] args)
        {
            //ExpressionExample ();
            ExpressionTreeExample ();
        }

        #region Lambdas

        public static void LamdaUsing ()
        {
            Func<int, double> function = new Func<int, double> (x => x / 2);
            Console.WriteLine ("9 / 2 = {0}", function (9));
            Console.WriteLine ("4 / 2 = {0}", function (4));
            Console.WriteLine ("2 / 2 = {0}", function (2));
        }

        public static void StartWriteStream ()
        {
            int number = 0;
            WriteStream (() => number++);
            Console.WriteLine ("Result is: {0}", number);
        }

        private static void WriteStream (Func<int> counter)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write ("{0}, ", counter.Invoke ());
                //Console.Write ("{0}, ", counter ());
            }
        }

        public static void FindMemberInList ()
        {
            List<string> members = new List<string>
            {
                "One",
                "Two",
                "Three",
                "Four",
            };

            //Func<string, string, bool> predicate = new Func<string, string, bool> ((x, y) => x.ToLower ().StartsWith (y));

            //Func<string, string, bool> predicate = (x, y) => x.ToLower ().StartsWith (y);

            //FindMember (members, "one", predicate);

            FindMember (members, "one", (x, y) => x.ToLower ().StartsWith (y));
        }

        public static void FindMember (IEnumerable<string> members, string name, Func<string, string, bool> predicate)
        {
            foreach (string member in members)
            {
                if (predicate (member, name))
                {
                    Console.WriteLine (member);
                    return;
                }
            }

            Console.WriteLine ("The name '{0}' is not exist.", name);
        }

        #endregion

        #region Expression

        public static void ExpressionExample ()
        {
            Expression<Func<int, double>> expression = (x) => x + 1;
            Func<int, double> function = expression.Compile ();

            for (int counter = 0; counter < 10; counter++)
            {
                Console.WriteLine ("Result is: {0}", function(counter));
            }
        }

        public static void ExpressionTreeExample ()
        {
            ParameterExpression name_n = Expression.Parameter (typeof (int), "n");
            ParameterExpression name_t = Expression.Parameter (typeof (int), "t");

            ConstantExpression constantValue = Expression.Constant (1);
            ConstantExpression constantValue2 = Expression.Constant (2);

            Expression addBody = Expression.Add (name_n, constantValue);
            Expression multBody = Expression.Multiply (addBody, name_t);
            Expression divideBody = Expression.Divide (multBody, constantValue2);

            var expression = Expression.Lambda<Func<int, int, int>> (divideBody, name_t, name_n);
            Console.WriteLine ("The expression: {0}", expression);

            Func<int, int, int> function = expression.Compile ();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine ("Result: {0}", function(i, i));
            }
        }

        #endregion
    }
}
