using System;
using System.IO;

namespace IO
{
    class Program
    {
        static void Main (string[] args)
        {
            //CreateDirectories ();
            //DeleteDirectories ();
            //CreateFile ();
            WriteDrives ();
        }

        private static void CreateDirectories ()
        {
            DirectoryInfo directory = new DirectoryInfo (@".");

            if (directory.Exists)
            {
                directory.CreateSubdirectory ("Subdirectory");
                directory.CreateSubdirectory (@"Directory\SuperDirectory");
                Console.WriteLine ("Directories is created.");
            }
            else
            {
                throw new System.InvalidOperationException ("Directory [" + directory.Name + "] is not exist.");
            }
        }

        private static void DeleteDirectories ()
        {
            string[] drives = Directory.GetLogicalDrives ();
            Console.WriteLine ("Exist drives: ");
            foreach (string drive in drives)
            {
                Console.WriteLine ("- {0}", drive);
            }

            DirectoryInfo directory = new DirectoryInfo (@".");

            if (directory.Exists)
            {
                Console.WriteLine ("Start deleting directories: ");
                Console.WriteLine (directory.FullName + @"Subdirectory");
                Console.WriteLine (directory.FullName + @"Directory\SuperDirectory");
                Console.WriteLine ("Press enter to delete directories.");
                Console.ReadKey ();

                try
                {
                    Directory.Delete (@"Subdirectory");
                    Directory.Delete (@"Directory\SuperDirectory", true);
                    Console.WriteLine ("The directories was deleted.");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine (ex.Message); 
                }
            }
            else
            {
                throw new System.InvalidOperationException ("Directory [" + directory.Name + "] is not exist.");
            }
        }

        private static void CreateFile ()
        {
            FileInfo file = new FileInfo (@".\Test.txt");
            //FileStream stream = file.Create ();
            FileStream stream = file.Open (FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);

            Console.WriteLine ("Full Name: {0}", file.FullName);
            Console.WriteLine ("Attributes: {0}", file.Attributes.ToString());
            Console.WriteLine ("Creation Time: {0}", file.CreationTime.ToString());

            Console.WriteLine ("\nPress any key to delete file.");
            Console.ReadKey ();

            stream.Close ();
            file.Delete ();

            Console.WriteLine ("File successfully was deleted");
            Console.WriteLine ("\nPress any key to exit.");
            Console.ReadKey ();

        }

        private static void WriteDrives ()
        {
            DriveInfo[] drives = DriveInfo.GetDrives ();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine ("Drive: {0} Type: {1}", drive.Name, drive.DriveType);
            }

            Console.WriteLine ("\nPress any key to exit.");
            Console.ReadKey ();
        }
    }
}
