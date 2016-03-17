using Anotar.NLog;
using CommandLine;
using SharpCompress.Common;
using SharpCompress.Compressor;
using SharpCompress.Compressor.Deflate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrepareInputFiles
{
    public class PrepareFiles
    {
        private CommandLineOptions Options { get; set; }
        public List<string> CommandLineArguments { get; set; }

        private static void Main(string[] args)
        {
            var pf = new PrepareFiles() { CommandLineArguments = new List<string>(args) };
            if (pf.ProcessCommandLineArguments() == false)
            {
                Console.WriteLine("Invalid or incorrect command line arguments");
                return;
            }
            LogTo.Debug("Starting to process files");
            pf.UnCompressFiles();
            ProcessUncompression(pf);
        }

        private static void ProcessUncompression(PrepareFiles pf)
        {
            var fileList = pf.Options.FilesListFile;
            var files = File.ReadAllLines(fileList);
        }

        public bool UnCompressFiles()
        {
            var fileLIst = Options.FilesListFile;
            var files = File.ReadAllLines(fileLIst);
            foreach (var sfInfo in
                files.Select(file => Options.SourceFolder + file).
                Select(sourceFile => new FileInfo(sourceFile)))
            {
                try
                {
                    var uncompressedFile = Decompress(sfInfo);
                    switch (uncompressedFile)
                    {
                        case "mpaa-ratings-reasons.list":
                            ParseMpaaRatingsReasons(uncompressedFile);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogTo.Fatal(e.Message);
                    return false;
                }
            }
            return true;
        }

        private void ParseMpaaRatingsReasons(string inputFile)
        {
        }

        private static string Decompress(FileInfo fileToDecompress)
        {
            string newFileName;
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (
                        GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
            return newFileName;
        }

        public bool ProcessCommandLineArguments()
        {
            Options = new CommandLineOptions();
            var args = CommandLineArguments.ToArray();
            return Parser.Default.ParseArguments(args, Options);
        }
    }
}