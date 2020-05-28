using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace reprocli
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var path = args[0];

                Console.WriteLine($"Path: {path}");


                var stopwatch = Stopwatch.StartNew();
                    var driveInfo = new DriveInfo(Directory.GetDirectoryRoot(path));
                    Console.WriteLine($"Free Space: {driveInfo.AvailableFreeSpace / 1024L / 1024L / 1024L} GB, Format: {driveInfo.DriveFormat} Name: {driveInfo.Name}");
                stopwatch.Stop();
                Console.WriteLine("Time: " + stopwatch.Elapsed);

                var stopwatchDir = Stopwatch.StartNew();
                    var size = CaculateSize(new DirectoryInfo(path));
                    Console.WriteLine("Size: " + (size / 1024L / 1024L + " MB"));
                stopwatchDir.Stop();
                Console.WriteLine("Time: " + stopwatchDir.Elapsed);



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static long CaculateSize(DirectoryInfo directoryInfo)
        {
            Console.WriteLine("Caclulation size for " + directoryInfo.FullName);

            var files = directoryInfo.GetFiles();
            var dirs = directoryInfo.GetDirectories();

            var size = files.Select(f => f.Length).Sum();

            return size + dirs.Select(CaculateSize).Sum();

        }
    }
}
