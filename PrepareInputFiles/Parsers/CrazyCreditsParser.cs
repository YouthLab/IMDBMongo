using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class CrazyCreditsParser : MultilineFileParser
    {
        private readonly List<CrazyCredits> _records;

        public CrazyCreditsParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "";
            PreHeaderLine2 = "==========================";
            RegularList = new List<string>
            {
                @"(?s)(?<=\n)#.*?(?=\n\n)"
            };
            MovieIdentifier = "# ";
            ValueIdentifier = "- ";
            _records = new List<CrazyCredits>();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(_records, Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _records.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var rawRecord = line.ToString().Split('\n');
                var record = new CrazyCredits();
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
                            record.Credits.Add(linesInCredit.ToString().Trim());
                            linesInCredit.Clear();
                        }

                        linesInCredit.Append(s.Remove(0, 2));
                    }
                    linesInCredit.Append(' ');
                }
                if (linesInCredit.Length != 0)
                    record.Credits.Add(linesInCredit.ToString().Trim());
                _records.Add(record);
            }
        }
    }
}