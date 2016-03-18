using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class CertificatesParser : FileParser
    {
        private List<Certificate> Records { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificatesParser"/> class.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        public CertificatesParser(string sourceFile)
        {
            SourceFile = sourceFile;
            SourceFile = sourceFile;
            HeaderLine = "-----------------------------------------------------------------------------";
            PreHeaderLine1 = "CERTIFICATES LIST";
            PreHeaderLine2 = "=================";
            RegularList = new List<string>
            {
                @".*\t\w*.*"
            };
            MovieIdentifier = "# ";
            ValueIdentifier = "- ";
            Records = new List<Certificate>();
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
                var rawRecord = UtfStr(line).Split('\t');
                rawRecord = rawRecord.Where(val => val != string.Empty).ToArray();
                var certificate = new Certificate();
                if (rawRecord.Length >= 1)
                    FixMovieNames(certificate, rawRecord[0]);
                if (rawRecord.Length >= 2)
                    AddRatings(certificate, rawRecord[1]);
                if (!Records.Any())
                {
                    Records.Add(certificate);
                }
                else
                {
                    if ((MovieBase) Records.Last() == (MovieBase) certificate)
                        Records.Last().RatingList.Add(certificate.RatingList.First());
                    else
                        Records.Add(certificate);
                }
            }
        }

        private void AddRatings(Certificate workCertificate, string detail)
        {
            var details = detail.Split(':');
            var ratings = new Ratings()
            {
                Country = details[0],
                Rating = details[1]
            };
            workCertificate.RatingList.Add(ratings);
        }
    }
}