using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflections
{
	internal static class ReflectionInfo
	{
		public static void PrintBaseInfo<T> (T classType)
		{
			Type type = classType.GetType ();

			Console.WriteLine (new string ('-', 30));
			Console.WriteLine ("Information about {0}", type.Name);

			Console.WriteLine ("Full name: {0}", type.FullName);
			Console.WriteLine ("Base type: {0}", type.BaseType);
			Console.WriteLine ("Is abstract: {0}", type.IsAbstract);
			Console.WriteLine ("Is COM object: {0}", type.IsCOMObject);
			Console.WriteLine ("Is sealed: {0}", type.IsSealed);
			Console.WriteLine ("Is class: {0}", type.IsClass);
		}

		public static void PrintMethods<T> (T classType)
		{
			Type type = classType.GetType ();

			Console.WriteLine (new string ('-', 30));
			Console.WriteLine ("Information about methods of the {0}", type.Name);

			MethodInfo[] methodsInfo = type.GetMethods (BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

			foreach (MethodInfo method in methodsInfo)
			{
				Console.WriteLine ("Method: {0}", method.Name);
			}
		}

		public static void PrintFields<T> (T classType)
		{
			Type type = classType.GetType ();

			Console.WriteLine (new string ('-', 30));
			Console.WriteLine ("Information about fields of the {0}", type.Name);

			FieldInfo[] fieldsInfo = type.GetFields (BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (FieldInfo field in fieldsInfo)
			{
				Console.WriteLine ("Field: {0}", field.Name);
			}
		}


		public static void PrintProperties<T> (T classType)
		{
			Type type = classType.GetType ();

			Console.WriteLine (new string ('-', 30));
			Console.WriteLine ("Information about methods of the {0}", type.Name);

			PropertyInfo[] propertiesInfo = type.GetProperties ();

			foreach (PropertyInfo property in propertiesInfo)
			{
				Console.WriteLine ("Property: {0}", property.Name);
			}
		}

		public static void PrintInterfaces<T> (T classType)
		{
			Type type = classType.GetType ();

			Console.WriteLine (new string ('-', 30));
			Console.WriteLine ("Information about interfaces of the {0}", type.Name);

			Type[] interfacesInfo = type.GetInterfaces ();

			foreach (Type item in interfacesInfo)
			{
				Console.WriteLine ("Interface: {0}", item.Name);
			}
		}

		public static void PrintConstructors<T> (T classType)
		{
			Type type = classType.GetType ();

			Console.WriteLine (new string ('-', 30));
			Console.WriteLine ("Information about constructors of the {0}", type.Name);

			ConstructorInfo[] constructorInfos = type.GetConstructors ();

			foreach (ConstructorInfo item in constructorInfos)
			{
				Console.WriteLine ("Constructor: {0}", item.Name);
			}
		}
	}
}
