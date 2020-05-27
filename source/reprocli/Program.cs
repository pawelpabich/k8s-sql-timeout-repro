using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.SqlClient;

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
                var driveInfo = new DriveInfo(Directory.GetDirectoryRoot(path));
                Console.WriteLine($"Free Space: {driveInfo.AvailableFreeSpace / 1024L / 1024L / 1024L} GB, Format: {driveInfo.DriveFormat} Name: {driveInfo.Name}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }


    }
}
