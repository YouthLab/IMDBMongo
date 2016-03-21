using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class SoundTrackParser : FileParser
    {
        private List<SoundTrack> _records { get; set; }

        public SoundTrackParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "SOUNDTRACKS";
            PreHeaderLine2 = "=============";
            RegularList = new List<string>
            {
                @"(?s)(?<=\n)#.*?(?=\n\n)"
            };
            _records = new List<SoundTrack>();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBegin Parsing files");
            ReadRecords();
            LogTo.Debug(("\n\tEnd Parsing input file"));
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(_records.OrderBy(y => y.Year).
                ThenBy(m => m.MovieName),
                Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _records.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var rawRecord = UtfStr(line).Split('\n');
                rawRecord = rawRecord.Where(val => val != "").ToArray();
                var record = new SoundTrack();

                foreach (var s in rawRecord.Where(
                    p => (p.StartsWith("#") || p.StartsWith("-"))))
                {
                    if (s.StartsWith("#"))
                        FixMovieNames(record, s.Remove(0, 1));
                    else
                        record.Titles.Add(s.Remove(0, 1).Replace('\"', ' ').Trim());
                }
                _records.Add(record);
            }
        }
    }
}