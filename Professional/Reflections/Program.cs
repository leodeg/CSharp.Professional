using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflections
{
    class Program
    {
        static void Main (string[] args)
        {
            //MethodGetType ();
			GetReflectionInformations ();
        }

        static void MethodGetType ()
        {
            TypeClass typeClass = new TypeClass ();
            Type type;

            type = typeClass.GetType ();
            Console.WriteLine ("GetType(): {0}", type.ToString());

            type = Type.GetType ("Reflections.TypeClass");
            Console.WriteLine ("Static GetType(): {0}", type.ToString ());

            type = typeof (TypeClass);
            Console.WriteLine ("typeof (): {0}", type.ToString ());
        }

		public static void GetReflectionInformations ()
		{
			ReflectionInfoClass example = new ReflectionInfoClass ();

			ReflectionInfo.PrintBaseInfo (example);
			ReflectionInfo.PrintFields (example);
			ReflectionInfo.PrintConstructors (example);
			ReflectionInfo.PrintProperties (example);
			ReflectionInfo.PrintMethods (example);
			ReflectionInfo.PrintInterfaces (example);
		}
    }

    internal class TypeClass { }

	internal class ReflectionInfoClass
	{
		public int PropFirst { get; set; }
		public int PropSecond { get; set; }

		public void MethodFirst ()
		{
			throw new NotImplementedException ();
		}

		public void MethodSecond ()
		{
			throw new NotImplementedException ();
		}
	}
}
