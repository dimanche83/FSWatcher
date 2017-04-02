using System;
using System.IO;
using System.Security.Permissions;
using System.Collections.Generic;

namespace FSWatcher
{
    class Program
    {
        static List<String> logs = new List<string>();

        //static StreamWriter sw;

        static string pathToWatch = @"c:\test\";

        static string logFile;

        static void Main(string[] args)
        {
            logFile = Directory.GetCurrentDirectory() + @"\log" + pathToWatch.Substring(pathToWatch.LastIndexOf(@"\") + 1) + ".csv";

            if (!File.Exists(logFile))
            {

                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine("Time,File,Event");
                }

            }

            Watch(pathToWatch);            
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Watch(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = path;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;

            watcher.Filter = "*.xml";

            watcher.Created += new FileSystemEventHandler(onFileCreated);

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Write \'q\' to quit watching...");

            while (Console.Read() != 'q') ;

            //foreach (string log in logs)
            //{
            //    Console.WriteLine(log);
            //}

            //Console.ReadKey();         
        }

        private static void onFileCreated(object source, FileSystemEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(logFile))
            {
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + "," + e.Name + "," + e.ChangeType);
            }
                //same time
                //Console.WriteLine("Time: " + DateTime.Now.ToString("HH:mm:ss.fff") + " File name: " + e.Name + " " + e.ChangeType);
                // same time
                //logs.Add("Time: " + DateTime.Now.ToString("HH:mm:ss.fff") + " File name: " + e.Name + " " + e.ChangeType);
            }     
    }
}
