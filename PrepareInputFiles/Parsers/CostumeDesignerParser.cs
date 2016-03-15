using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class CostumeDesignerParser : MultilineFileParser
    {
        private readonly List<CostumeDesigner> _costumeDesigners;

        public CostumeDesignerParser(string sourceFile)
        {
            SourceFile = sourceFile;
            RegularList = new List<string>()
            {
                @"(?s)(?<=\n\n).*?(?=\n\n)"
            };
            _costumeDesigners = new List<CostumeDesigner>();

            PreHeaderLine1 = @"Name			Titles";
            PreHeaderLine2 = @"----			------";
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(_costumeDesigners, Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _costumeDesigners.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            CostumeDesigner record = null;
            foreach (var line in lines)
            {
                var rawRecord = line.ToString().Split('\n');
                if (!rawRecord[0].Contains('\t'))
                    continue;
                rawRecord = rawRecord.Where(val => val != PreHeaderLine1).ToArray();
                rawRecord = rawRecord.Where(val => val != PreHeaderLine2).ToArray();
                var scan = new Regex(@"^[\w-,'.].*\t.*");
                var artistLine = scan.Match(rawRecord[0]);
                if (artistLine.Success)
                {
                    if (record != null && !string.IsNullOrWhiteSpace(record.Name))
                        _costumeDesigners.Add(record);
                    record = new CostumeDesigner();
                    var artistLineSplit = artistLine.Value.Split('\t');
                    if (artistLineSplit.Length > 2) //remove blank elements
                    {
                        artistLineSplit = artistLineSplit.Where(val => val != "").ToArray();
                    }
                    if (artistLineSplit.Length >= 1)
                        record.Name = artistLineSplit[0];
                    if (artistLineSplit.Length >= 2)
                        FixMovieNames(record, artistLineSplit[1]);
                }
            }
            if (record != null && !string.IsNullOrWhiteSpace(record.Name))
                _costumeDesigners.Add(record);
        }
    }
}