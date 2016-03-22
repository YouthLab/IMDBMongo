using Anotar.NLog;
using DataModel.InputFileProcessing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class AlternateVersionsParser : FileParser
    {
        private List<AlternateVersion> Records { get; set; }

        public AlternateVersionsParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "MPAA RATINGS REASONS LIST";
            PreHeaderLine2 = "==========================";
            RegularList = new List<string>
            {
                @"(?s)(?<=\n)#.*?(?=\n\n)"
            };
            MovieIdentifier = "# ";
            ValueIdentifier = "- ";
            Records = new List<AlternateVersion>();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(Records, Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return Records.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var rawRecord = UtfStr(line).Split('\n');
                var record = new AlternateVersion();
                var movieName = rawRecord.FirstOrDefault(m => m.StartsWith("#"));
                if (movieName != null)
                {
                    movieName = movieName.Replace('#', ' ');
                    FixMovieNames(record, movieName.Trim());
                }

                var linesInCredit = new StringBuilder();
                foreach (var s in rawRecord.Where(s => !s.StartsWith("#")))
                {
                    if (!s.StartsWith("-"))
                        linesInCredit.Append(s.Trim());
                    else
                    {
                        if (linesInCredit.Length != 0x0)
                        {
                            record.AlternateList.Add(linesInCredit.ToString().Trim());
                            linesInCredit.Clear();
                        }

                        linesInCredit.Append(s.Remove(0, 2));
                    }
                    linesInCredit.Append(' ');
                }
                if (linesInCredit.Length != 0)
                    record.AlternateList.Add(linesInCredit.ToString().Trim());
                Records.Add(record);
            }
        }
    }
}