using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace XML
{
    class Program
    {
        static void Main (string[] args)
        {
            //LoadXmlFile ();
            //ReadXmlFile_FileStream ();
            //ReadXmlFile_StringReader ();
            //ReadXmlDocument_ChildNodes ();
            //ReadXmlDocument_ReadAttributes ();
            //WriteXmlDocument_XmlTextWriter ();
            ReadXmlDocument_Path ();
        }

        static void LoadXmlFile ()
        {
            XmlDocument document = new XmlDocument ();
            document.Load ("books.xml");

            Console.WriteLine (document.InnerText);
            Console.WriteLine (new string ('-', 20));
            Console.WriteLine (document.InnerXml);
        }

        static void ReadXmlFile_FileStream ()
        {
            FileStream stream = new FileStream ("books.xml", FileMode.Open);
            XmlTextReader reader = new XmlTextReader (stream);

            while (reader.Read ())
            {
                Console.WriteLine ("{0,-5} {1,-5} {2,-5}",
                    reader.NodeType,
                    reader.Name,
                    reader.Value);
            }

            reader.Close ();
            stream.Close ();
        }

        static void ReadXmlFile_StringReader ()
        {
            string xmlData = "<?xml version='1.0' encoding='utf-8' ?><books><book><name>Book 1</name><price>1</price></book></books>";

            StringReader stringReader = new StringReader (xmlData);
            XmlTextReader xmlReader = new XmlTextReader (stringReader);

            while (xmlReader.Read ())
            {
                Console.WriteLine ("{0,-15} {1,-10} {2,-10}",
                    xmlReader.NodeType,
                    xmlReader.Name,
                    xmlReader.Value);
            }

            xmlReader.Close ();
            stringReader.Close ();
        }

        static void ReadXmlDocument_ChildNodes ()
        {
            XmlDocument document = new XmlDocument ();
            document.Load ("books.xml");

            XmlNode root = document.DocumentElement;

            Console.WriteLine ("document.DocumentElement = {0}", root.LocalName);
            Console.WriteLine (new string ('-', 40));

            foreach (XmlNode books in root.ChildNodes)
            {
                Console.WriteLine ("Found Book: ");
                foreach (XmlNode book in books.ChildNodes)
                {
                    Console.WriteLine (String.Format ("{0}: {1}", book.Name, book.InnerText));
                }
                Console.WriteLine (new string ('-', 40));
            }
        }

        static void ReadXmlDocument_ReadAttributes ()
        {
            XmlTextReader xmlReader = new XmlTextReader ("books.xml");

            while (xmlReader.Read ())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.HasAttributes)
                    {
                        while (xmlReader.MoveToNextAttribute ())
                        {
                            Console.WriteLine (String.Format ("{0} = {1}", xmlReader.Name, xmlReader.Value));
                        }
                    }
                }
            }
        }

        static void WriteXmlDocument_XmlTextWriter ()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter ("Books.xml", null);

            xmlWriter.WriteStartDocument ();
            xmlWriter.WriteStartElement ("Books");
            xmlWriter.WriteStartElement ("Book");
            xmlWriter.WriteStartAttribute ("FontSize");
            xmlWriter.WriteString ("8");
            xmlWriter.WriteString ("18");
            xmlWriter.WriteEndAttribute ();
            xmlWriter.WriteString ("Title-1");
            xmlWriter.WriteEndElement ();
            xmlWriter.WriteEndElement ();

            xmlWriter.Close ();
        }

        static void ReadXmlDocument_Path ()
        {
            XPathDocument document = new XPathDocument ("books.xml");
            XPathNavigator navigator = document.CreateNavigator ();

            XPathNodeIterator iterator = navigator.Select ("books/book/name");
            while (iterator.MoveNext ())
            {
                Console.WriteLine (iterator.Current);
            }

            Console.WriteLine (new string('-', 20));

            XPathExpression expression = navigator.Compile ("books/book[2]/price");
            XPathNodeIterator expressionIterator = navigator.Select(expression);
            while (expressionIterator.MoveNext())
            {
                Console.WriteLine(expressionIterator.Current);
            }
        }
    }
}
