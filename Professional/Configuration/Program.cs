using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace ConfigurationExample
{
    class Program
    {
        static void Main (string[] args)
        {

        }

        private static void ConfigurationBasic ()
        {
            string value = ConfigurationSettings.AppSettings["Key1"];
            Console.WriteLine (value);
            Console.WriteLine (new string('-', 20));
        }
    }
}
