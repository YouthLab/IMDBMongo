using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class SoundMixParser : FileParser
    {
        private readonly List<SoundMix> _records;

        public SoundMixParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "SOUND-MIX LIST";
            PreHeaderLine2 = "==============";
            RegularList = new List<string>
            {
                @".*\(....\/*\w*\).*\t.*"
            };
            _records = new List<SoundMix>();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(
                _records.OrderBy(y => y.Year).ThenBy(n => n.MovieName),
                Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _records.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            var record = new SoundMix();
            foreach (var line in lines)
            {
                var rawRecord = UtfStr(line);
                var recordSplit = rawRecord.Split('\t').Where(val => val != "").ToArray();
                if (recordSplit.Length < 1) continue;
                var movie = new MovieBase();
                FixMovieNames(movie, recordSplit[0]);
                if (!movie.Equals((MovieBase)record))
                {
                    if (!string.IsNullOrWhiteSpace(record.MovieName))
                        _records.Add(record);
                    record = new SoundMix()
                    {
                        Episode = movie.Episode,
                        MovieName = movie.MovieName,
                        Year = movie.Year
                    };
                }
                if (recordSplit.Length >= 2)
                    record.SoundModeList.Add(recordSplit[1]);
            }
        }
    }
}