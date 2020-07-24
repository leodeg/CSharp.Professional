using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskParallelLibrary
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			StartTask();
			//TaskFactory ();
		}



		public static void StartTask()
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Base thread was started.");
			Console.ForegroundColor = ConsoleColor.Gray;

			Action action = new Action(TaskExample);
			Task task = new Task(action);
			task.Start();

			for (int i = 0; i < 60; i++)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write('.');
				Console.ForegroundColor = ConsoleColor.Gray;
				Thread.Sleep(50);
			}

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("\nBase thread was completed.");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void TaskExample()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Task was started.");
			Console.ForegroundColor = ConsoleColor.Gray;

			for (int count = 0; count < 20; count++)
			{
				Thread.Sleep(200);
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Count is: {0}", count);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Task was completed.");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void TaskFactory()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Main thread was started.");
			Console.ForegroundColor = ConsoleColor.Gray;

			Task task = Task.Factory.StartNew(() => {
				Console.WriteLine("Task was started.");
				for (int count = 0; count < 10; count++)
				{
					Thread.Sleep(100);
					Console.WriteLine("Counter is equal to: {0}", count);
				}
				Console.WriteLine("Task was completed");
			});

			task.Wait();
			task.Dispose();

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Main thread was completed.");
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
