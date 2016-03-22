using Anotar.NLog;
using DataModel.InputFileProcessing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class BusinessParser : FileParser
    {
        #region Public Constructors

        public BusinessParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "BUSINESS LIST";
            PreHeaderLine2 = "=============";

            RegularList = new List<string>
            {
                @"(?s)(?<=--------\n).*?(?=\n--------)"
            };
            _records = new List<Business>();
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods

        #region Protected Methods

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var rawRecord = UtfStr(line).Split('\n').
                    Where(val => val != PreHeaderLine1);
                rawRecord = rawRecord.Where(val => val != PreHeaderLine2).ToArray();
                rawRecord = rawRecord.Where(val => val != "").ToArray();
                var record = new Business();
                foreach (var s in rawRecord)
                {
                    char[] delimiter = {':'};
                    var details = s.Split(delimiter, 2);
                    if (details.Length < 2) continue;
                    details[1] = details[1].Trim();
                    switch (details[0])
                    {
                        case "MV":
                            if (details[1].Contains("Fictional Title"))
                                break;
                            FixMovieNames(record, details[1]);
                            break;

                        case "BT":
                            record.Budget = details[1];
                            break;

                        case "GR":
                            record.BoxOfficeGross.Add(details[1]);
                            break;

                        case "OW":
                            record.OpeningWeekend.Add(details[1]);
                            break;

                        case "RT":
                            record.Rental = details[1];
                            break;

                        case "AD":
                            record.Admissions.Add(details[1]);
                            break;

                        case "SD":
                            record.ShootingDates = details[1];
                            break;

                        case "PD":
                            record.ProductionDates = details[1];
                            break;

                        case "ST":
                            record.Studios.Add(details[1]);
                            break;

                        case "CP":
                            record.CopyrightHolder = details[1];
                            break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(record.MovieName))
                    _records.Add(record);
            }
        }

        #endregion Protected Methods

        #region Private Fields

        private readonly List<Business> _records;

        #endregion Private Fields
    }
}