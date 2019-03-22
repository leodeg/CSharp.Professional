using System;
using System.Linq;
using System.Reflection;

namespace Attributes
{
	internal class Program
	{
		private static void Main (string[] args)
		{
			//GetAttributeInformation ();
			GetExecutingAssembly ();
		}

		private static void GetAttributeInformation ()
		{
			HowToUseAttribute userAttribute = new HowToUseAttribute ();
			Type attributeType = typeof (HowToUseAttribute);

			CreationTimeAttribute attribute = null;
			foreach (object type in attributeType.GetCustomAttributes (false))
			{
				attribute = type as CreationTimeAttribute;
				Console.WriteLine ("Type analysis: Day = {0}, Month = {1}, Year = {2}", attribute.Day, attribute.Month, attribute.Year);
			}
		}

		private static void GetExecutingAssembly ()
		{
			Assembly assembly = Assembly.GetExecutingAssembly ();
			object[] attributes = assembly.GetCustomAttributes (false);

			foreach (Attribute attribute in attributes)
			{
				Console.WriteLine ("Attribute: {0}", attribute.GetType ().Name);
			}

			AssemblyFileVersionAttribute appVersion = attributes.OfType<AssemblyFileVersionAttribute> ().Single ();

			Console.WriteLine ("Application version: {0}", appVersion.Version);
		}
	}

	[AttributeUsage (AttributeTargets.All, AllowMultiple = false)]
	internal class CreationTimeAttribute : System.Attribute
	{
		public CreationTimeAttribute (int day, int month, int year)
		{
			Day = day;
			Month = month;
			Year = year;
		}

		public int Day { get; }
		public int Month { get; }
		public int Year { get; }
	}

	[CreationTime (22, 03, 19)]
	public class HowToUseAttribute
	{
		[CreationTime (22, 03, 19)]
		public void Method ()
		{
			Console.WriteLine ("How to use a user attribute.");
		}
	}
}
