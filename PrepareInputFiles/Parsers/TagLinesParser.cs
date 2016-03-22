using Anotar.NLog;
using DataModel.InputFileProcessing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class TagLinesParser : FileParser
    {
        private readonly Dictionary<MovieBase, TagLine> _records;

        public TagLinesParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-----------------------------------------------------------------------------";
            PreHeaderLine1 = "TAG LINES LIST";
            PreHeaderLine2 = "==============";
            RegularList = new List<string>
            {
                @"(?s)(?<=\n)#.*?(?=\n\n)"
            };
            MovieIdentifier = "# ";
            ValueIdentifier = "- ";
            _records = new Dictionary<MovieBase, TagLine>();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(
                _records.Values.OrderBy(y => y.Year).ThenBy(m => m.MovieName),
                Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _records.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var rawRecord = UtfStr(line).Split('\n');
                var record = new TagLine();
                var movieName = rawRecord.FirstOrDefault(m => m.StartsWith("# "));
                if (string.IsNullOrWhiteSpace(movieName))
                    continue;

                movieName = movieName.Replace('#', ' ');
                FixMovieNames(record, movieName.Trim());

                foreach (var str in rawRecord.Where(s => !s.StartsWith("#")))
                {
                    record.TagLines.Add(str.Trim());
                }
                if (_records.ContainsKey((MovieBase) record))
                {
                    var recordToUpdate = _records.FirstOrDefault(m => m.Key == (MovieBase) record).Value;
                    foreach (var tagLine in record.TagLines.ToArray())
                    {
                        recordToUpdate.TagLines.Add(tagLine);
                        record.TagLines.Remove(tagLine);
                    }
                }

                if (record.TagLines.Any())
                    _records.Add((MovieBase) record, record);
            }
        }
    }
}