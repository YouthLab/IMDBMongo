using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class TagLinesParser : FileParser
    {
        private List<TagLine> _records;

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
            _records = new List<TagLine>();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            _records = _records.OrderBy(m => m.MovieName).ThenBy(y => y.Year).ToList();
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(_records, Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _records.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var rawRecord = line.ToString().Split('\n');
                var record = new TagLine();
                var movieName = rawRecord.FirstOrDefault(m => m.StartsWith("#"));
                if (movieName != null)
                {
                    movieName = movieName.Replace('#', ' ');
                    FixMovieNames(record, movieName.Trim());
                }
                foreach (var str in rawRecord.Where(s => !s.StartsWith("#")))
                {
                    record.TagLines.Add(str.Trim());
                }
                //foreach (var tagLine in
                //    _records.Where(tagLine => (MovieBase)tagLine ==
                //        ((MovieBase)record)))
                //{
                //    foreach (var str in record.TagLines)
                //        tagLine.TagLines.Add(str);
                //    record.TagLines.Clear();
                //}
                if (record.TagLines.Any())
                    _records.Add(record);
            }
        }
    }
}