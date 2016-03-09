using CommandLine;

namespace FetchInputFiles
{
    public class CommandLineOptions
    {
        [Option('u', "url", Required = true, HelpText = "FTP URL to get files from")]
        public string SourceUrl { get; set; }

        [Option('d', "dir", Required = true, HelpText = "Folder where to drop files")]
        public string DestinationFolder { get; set; }

        [Option('f', "listFile", Required = true, HelpText = "List of files to download")]
        public string FilesList { get; set; }
    }
}