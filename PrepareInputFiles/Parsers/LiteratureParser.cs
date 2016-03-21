using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class LiteratureParser : FileParser
    {
        #region Public Constructors

        public LiteratureParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "MPAA RATINGS REASONS LIST";
            PreHeaderLine2 = "==========================";
            RegularList = new List<string>
            {
                @"(?s)(?<=\n)MOVI.*?(?=\n-)"
            };
            _records = new List<LiteratureCredit>();
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
                var rawRecord = UtfStr(line).Split('\n');
                rawRecord = rawRecord.Where(val => val != "").ToArray();
                var record = new LiteratureCredit();
                foreach (var s in rawRecord)
                {
                    var lc = new LitContribution();
                    var details = s.Split(":".ToCharArray(), 2);
                    if (details.Length != 2)
                    {
                        LogTo.Fatal("Parser error ");
                        // ReSharper disable once CoVariantArrayConversion
                        LogTo.Fatal("Message: {0}", rawRecord);
                        throw new Exception("Parser error in Literature Parser");
                    }
                    switch (details[0])
                    {
                        case "MOVI":
                            FixMovieNames(record, details[1]);
                            break;

                        case "ADPT":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.Adaptation;
                            break;

                        case "BOOK":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.Book;
                            break;

                        case "CRIT":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.CriticalReview;
                            break;

                        case "ESSY":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.Essay;
                            break;

                        case "IVIW":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.Interview;
                            break;

                        case "NOVL":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.Novel;
                            break;

                        case "PROT":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.ProdProtocol;
                            break;

                        case "SCRP":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.Script;
                            break;

                        case "OTHR":
                            ExtractDetails(details[1], line, lc);
                            lc.CreditedFor = CreditedFor.Other;
                            break;
                    }
                    if (string.IsNullOrWhiteSpace(lc.Name)) continue;
                    if (record.Contributions.ContainsKey(lc.Name))
                        record.Contributions[lc.Name].Details.Add(lc.Details.FirstOrDefault());
                    else
                        record.Contributions.Add(lc.Name, lc);
                }
                _records.Add(record);
            }
        }

        #endregion Protected Methods

        #region Private Fields

        private readonly List<LiteratureCredit> _records;

        #endregion Private Fields

        #region Private Methods

        private static void ExtractDetails(string details, Match line, LitContribution lc)
        {
            var subDetails = details.Split(".".ToCharArray(), 2);
            if (subDetails.Length >= 1)
                lc.Name = subDetails[0];
            if (subDetails.Length > 1)
                lc.Details.Add(subDetails[1].Replace('\"', ' ').Trim());
        }

        #endregion Private Methods
    }
}