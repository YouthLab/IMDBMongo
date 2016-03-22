using Anotar.NLog;
using DataModel.InputFileProcessing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class SoundTrackParser : FileParser
    {
        private List<SoundTrack> Records { get; set; }

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
            Records = new List<SoundTrack>();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBegin Parsing files");
            ReadRecords();
            LogTo.Debug(("\n\tEnd Parsing input file"));
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(Records.OrderBy(y => y.Year).
                ThenBy(m => m.MovieName),
                Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return Records.Any();
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
                Records.Add(record);
            }
        }
    }
}