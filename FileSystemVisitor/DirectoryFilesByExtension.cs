using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FileSystemVisitor
{
    public class DirectoryFilesByExtension
    {
        public static void FilterDirectoryFilesByExtension(string rootDirectoryPath, string fileExtension)// alohida class - filter -- nomi and extension
        {
            if (String.IsNullOrEmpty(fileExtension))
            {
                DirectoryTree.PrintDirectoryTree(rootDirectoryPath);
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
    }
}
