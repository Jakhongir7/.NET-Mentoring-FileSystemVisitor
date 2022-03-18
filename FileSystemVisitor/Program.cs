using System;
using System.IO;
using System.Linq;

namespace FileSystemVisitor
{
    class Program
    {
        internal const int RootLevel = 0;
        internal const char Tab = '\t';

        public static void Main()
        {
            Console.WriteLine("*** Enter the path of the directory: ***");
            var rootDirectorypath = Console.ReadLine();

            Notifications n = new Notifications();
            n.Notify += DisplayMessage;
            n.Start();

            void DisplayMessage(string message) => Console.WriteLine(message);

            Console.WriteLine($"Getting directory tree of '{rootDirectorypath}'");
            PrintDirectoryTree(rootDirectorypath);

            n.Finish();

            Console.WriteLine("\n*** Please type the extension(txt, docx, jpg) of the file if you want to filter: ***");
            var fileExtension = Console.ReadLine();
            FilterDirectoryFilesByExtension(rootDirectorypath, fileExtension);

            Console.WriteLine("\nPress 'Enter' to quit...");
            Console.ReadLine();
        }

       

        public static void PrintDirectoryTree(string rootDirectoryPath)
        {
            try
            {
                if (!Directory.Exists(rootDirectoryPath))
                {
                    throw new DirectoryNotFoundException(
                        $"Directory '{rootDirectoryPath}' not found.");
                }

                var rootDirectory = new DirectoryInfo(rootDirectoryPath);
                PrintDirectoryTree(rootDirectory, RootLevel);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void PrintDirectoryTree(DirectoryInfo directory, int currentLevel)
        {
            var indentation = string.Empty;
            for (var i = RootLevel; i < currentLevel; i++)
            {
                indentation += Tab;
            }

            Console.WriteLine($"{indentation}-{directory.Name}");
            var nextLevel = currentLevel + 1;
            try
            {
                foreach (var subDirectory in directory.GetDirectories())
                {
                    PrintDirectoryTree(subDirectory, nextLevel);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"{indentation}-{e.Message}");
            }
        }

        public static void FilterDirectoryFilesByExtension(string rootDirectoryPath, string fileExtension)
        {
            if (String.IsNullOrEmpty(fileExtension))
            {
                PrintDirectoryTree(rootDirectoryPath);
            }
            else
            {
                try
                {
                    var files = Directory.EnumerateFiles(rootDirectoryPath, $"*.{fileExtension}", SearchOption.AllDirectories)
                                            .Where(s => s.EndsWith(fileExtension));

                    foreach (var file in files)
                    {
                        Console.WriteLine(" - " + Path.GetFileName(file));
                    }
                    Console.WriteLine($"\n*** {files.Count()} files found. ***");
                }
                catch (UnauthorizedAccessException uAEx)
                {
                    Console.WriteLine(uAEx.Message);
                }
                catch (PathTooLongException pathEx)
                {
                    Console.WriteLine(pathEx.Message);
                }
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnStarted(object sender, FileSystemEventArgs e)
        {
            string value = $"Started: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
