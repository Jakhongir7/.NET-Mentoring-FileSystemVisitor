using System;
using System.IO;
using System.Linq;

namespace FileSystemVisitor
{
    class Program
    {
        

        public static void Main()
        {
            Console.WriteLine("*** Enter the path of the directory: ***");
            var rootDirectorypath = Console.ReadLine();

            Notifications n = new Notifications();
            n.Notify += DisplayMessage;
            n.Start();

            void DisplayMessage(string message) => Console.WriteLine(message);

            Console.WriteLine($"Getting directory tree of '{rootDirectorypath}'");
            DirectoryTree.PrintDirectoryTree(rootDirectorypath);

            n.Finish();

            Console.WriteLine("\n*** Please type the extension(txt, docx, jpg) of the file if you want to filter: ***");
            var fileExtension = Console.ReadLine();
            DirectoryFilesByExtension.FilterDirectoryFilesByExtension(rootDirectorypath, fileExtension);

            Console.WriteLine("\nPress 'Enter' to quit...");
            Console.ReadLine();
        }
    }
}
