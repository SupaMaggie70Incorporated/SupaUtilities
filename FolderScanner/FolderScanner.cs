using System;
using System.IO;
using System.Collections.Generic;
using System;
namespace FolderScanner
{
    internal class Program
    {
        public static List<string> matches;
        public static string query = "";
        public static string filename = "./items.txt";
        public static string parentdir;
        static void Main(string[] args)
        {
            matches = new List<string>();
            parentdir = "";
            if (args.Length == 0)
            {
                Console.Write("Directory to scan: ");
                parentdir = Console.ReadLine();
            }
            else
            {
                parentdir = args[0];
            }
            if (args.Length < 2)
            {
                Console.Write("Query: ");
                query = Console.ReadLine().ToLower();
            }
            else
            {
                parentdir = args[1];
            }
            ScanDirectory(parentdir);
            WriteFile();
        }
        public static void ScanDirectory(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            foreach(string file in files)
            {
                Uri uri = new Uri(file);
                string[] segments = uri.Segments;
                var name = segments[segments.Length - 1];
                if(name.ToLower().Contains(query))
                {
                    Uri relative = new Uri(parentdir).MakeRelativeUri(uri);
                    matches.Add(relative.ToString());
                }
            }
            string[] folders = Directory.GetDirectories(folder);
            foreach(string dir in folders)
            {
                ScanDirectory(dir);
            }
        }
        public static void WriteFile()
        {
            string toprint = String.Join('\n', matches.ToArray());
            if (File.Exists(filename)) File.Delete(filename);
            StreamWriter fs = File.CreateText(filename);
            fs.Write(toprint);
            if (matches.Count <= 16)
            {
                Console.WriteLine(toprint + "\nYou can find a complete list on "+filename+".");
            }
            else
            {
                Console.WriteLine("More than 16 files matching your query were found! You can find them in " + filename + ".");
            }

        }
    }
}
