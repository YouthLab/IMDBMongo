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
    public class MpaaRatingsReasonsFileParser : MultilineFileParser
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MpaaRatingsReasonsFileParser"/> class.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        public MpaaRatingsReasonsFileParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "MPAA RATINGS REASONS LIST";
            PreHeaderLine2 = "==========================";
            _recordHeaderLine = HeaderLine;
            RegularList = new List<string>
            {
                @"MV:(.*?(\n))RE: .*\n\n",
                @"MV:(.*?(\n))RE: .*\nRE: .*\nRE: .*\n",
                @"MV:(.*?(\n))RE: .*\nRE: .*\n\n"
            };
            MovieIdentifier = "MV:";
            ValueIdentifier = "RE:";
            _records = new List<MpaaRatingsReasons>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Parses the file.
        /// </summary>
        /// <param name="destinationFile">The destination file.</param>
        /// <returns></returns>
        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBegin Parsing files");
            ReadRecords();
            LogTo.Debug(("\n\tEnd Parsing input file"));
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(_records, Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _records.Any();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Populates records.
        /// </summary>
        /// <param name="lines">The lines.</param>
        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var rawRecord = line.ToString().Split('\n');
                var record = new MpaaRatingsReasons();
                var movieName = rawRecord.FirstOrDefault(m => m.StartsWith("MV:"));
                if (movieName != null)
                    FixMovieNames(record, movieName.Trim());

                var reasonRecord = new StringBuilder();
                foreach (var s in rawRecord.Where(s => s.StartsWith("RE:")))
                    reasonRecord.AppendFormat("{0} ", s.Remove(0, 3).Trim());
                record.Reason = reasonRecord.ToString().Trim();
                _records.Add(record);
            }
        }

        #endregion Protected Methods

        #region Private Fields

        private readonly List<MpaaRatingsReasons> _records;
        private string _recordHeaderLine;

        #endregion Private Fields
    }
}