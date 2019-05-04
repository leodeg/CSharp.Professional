using System;
using System.IO;

namespace Arrays
{
    class Program
    {
        static void Main (string[] args)
        {

        }

        private static void ArraysCastExamples ()
        {
            object[,] objectStreams = new FileStream[5, 10];

            //Stream[] streams = (Stream[])objectStreams; // Cannot convert type
            Stream[,] streams = (Stream[,])objectStreams;
            string[,] stringsFileStreams = (string[,])objectStreams;

            int[] integers = new int[5];
            //object[] objectIntegers = (object[])integers; // Cannot convert type
            object[] objectIntegers = new object[integers.Length];
            Array.Copy (integers, objectIntegers, integers.Length);
        }
    }
}
