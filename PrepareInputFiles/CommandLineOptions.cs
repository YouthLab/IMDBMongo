using CommandLine;

namespace PrepareInputFiles
{
    public class CommandLineOptions
    {
        [Option('s', "sourceDir", Required = true, HelpText = "Folder where we have IMDB files")]
        public string SourceFolder { get; set; }

        [Option('d', "destinationDir", Required = true, HelpText = "Destination folder for processed files")]
        public string DestinationFolder { get; set; }

        [Option('f', "filesListFile", Required = true, HelpText = "List of files to process")]
        public string FilesListFile { get; set; }
    }
}