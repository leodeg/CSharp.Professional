using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialization
{
    class Program
    {
        static void Main (string[] args)
        {
            StartSerializationDeserializationExample ();
        }

        public static void StartSerializationDeserializationExample ()
        {
            List<string> objectGraph = new List<string> { "Jeff", "Romeo", "John" };
            Stream stream = SerializeToMemory (objectGraph);

            stream.Position = 0;
            objectGraph = DeserializeFromMemory (stream) as List<string>;
            foreach (object item in objectGraph)
                Console.WriteLine (item);
        }

        private static MemoryStream SerializeToMemory (object objectGraph)
        {
            MemoryStream stream = new MemoryStream ();
            BinaryFormatter formatter = new BinaryFormatter ();
            formatter.Serialize (stream, objectGraph);
            return stream;
        }

        public static object DeserializeFromMemory (Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter ();
            return formatter.Deserialize (stream);
        }
    }
}
