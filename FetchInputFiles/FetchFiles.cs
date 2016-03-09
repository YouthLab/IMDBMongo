using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FetchInputFiles
{
    public class FetchFiles
    {
        public CommandLineOptions CommandLineOpts { get; set; }
        public List<string> CommandLineArguments { get; set; }
        private int DownloadCount { get; set; }

        private static void Main(string[] args)
        {
            var ff = new FetchFiles { CommandLineArguments = new List<string>(args) };
            ff.ProcessCommandLineArguments();
        }

        /// <summary>
        /// Processes the command line arguments.
        /// </summary>
        public void ProcessCommandLineArguments()
        {
            CommandLineOpts = new CommandLineOptions();
            var args = CommandLineArguments.ToArray();
            Console.WriteLine(Parser.Default.ParseArguments(args, CommandLineOpts)
                ? "Worked"
                : "did not work");

            return;
        }

        /// <summary>
        /// Gets the files from server.
        /// </summary>
        private void GetFilesFromServer()
        {
            //var pathSplit = CommandLineOpts.SourceUrl.Split(new char[] { '/' }, 2);
            var files = File.ReadAllLines(CommandLineOpts.FilesList);
            var tasks = new List<Task>();
            foreach (var file in files)
            {
                tasks.Add(DownloadFileAsync(file));
                if (tasks.Count() >= 6)
                {
                    Task.WaitAny(tasks.ToArray());
                    foreach (var task in tasks.ToList().Where(task => task.IsCompleted ||
                                                                      task.IsCanceled ||
                                                                      task.IsFaulted))
                    {
                        DownloadCount++;
                        tasks.Remove(task);
                    }
                }
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks.ToList().Where(task => task.IsCompleted ||
                                                              task.IsCanceled ||
                                                              task.IsFaulted))
            {
                DownloadCount++;
                tasks.Remove(task);
            }
        }

        /// <summary>
        /// Downloads the file asynchronous.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private Task DownloadFileAsync(string file)
        {
            var uri = new Uri(CommandLineOpts.SourceUrl + file);
            using (var client = new WebClient())
            {
                const string userName = "Anonymous";
                const string password = "god@heaven.org";
                client.Credentials = new NetworkCredential(userName, password);

                var task = client.DownloadFileTaskAsync(uri,
                    CommandLineOpts.DestinationFolder + file);
                return task;
            }
        }

        /// <summary>
        /// Gets the number of files downloaded.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfFilesDownloaded()
        {
            GetFilesFromServer();
            return DownloadCount;
        }
    }
}