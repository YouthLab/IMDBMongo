using Anotar.NLog;
using DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class CostumeDesignerParser : FileParser
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CostumeDesignerParser"/> class.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
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

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Parses the file.
        /// </summary>
        /// <param name="destinationFile">The destination file.</param>
        /// <returns></returns>
        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(_costumeDesigners, Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return _costumeDesigners.Any();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Populates the records.
        /// </summary>
        /// <param name="lines">The lines.</param>
        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            CostumeDesigner record = null;
            foreach (var rawRecords in
                from line in lines
                select line.ToString().Split('\n')
                into rawRecords
                where rawRecords[0].Contains('\t')
                select rawRecords.Where(val => val != PreHeaderLine1).ToArray()
                into rawRecords
                select rawRecords.Where(val => val != PreHeaderLine2).ToArray())
            {
                record = new CostumeDesigner();
                foreach (var rawRecord in rawRecords)
                {
                    var scan = new Regex(@"^\s\s*");
                    var artistLine = scan.Match(rawRecord);
                    if (!artistLine.Success) //new artist
                    {
                        var artistLineSplit = rawRecord.Split('\t');
                        artistLineSplit = artistLineSplit.
                            Where(val => val != "").ToArray(); //remove blank elements

                        if (artistLineSplit.Length > 0)
                            record.Name = artistLineSplit[0].Trim();
                        if (artistLineSplit.Length >= 2)
                            AddMovieDetails(artistLineSplit[1], record);
                    }
                    else //other work by the same artist
                    {
                        var artistLineSplit = rawRecord.Split('\t');
                        artistLineSplit = artistLineSplit.
                            Where(val => val != "").ToArray(); //remove blank elements
                        foreach (var s in artistLineSplit)
                            AddMovieDetails(s, record);
                    }
                }
                if (!string.IsNullOrWhiteSpace(record.Name))
                    _costumeDesigners.Add(record);
            }
            if (record != null && !string.IsNullOrWhiteSpace(record.Name))
                _costumeDesigners.Add(record);
        }

        #endregion Protected Methods

        #region Private Fields

        private readonly List<CostumeDesigner> _costumeDesigners;

        #endregion Private Fields

        #region Private Methods

        private void AddMovieDetails(string s, CostumeDesigner record)
        {
            var mb = new MovieBase();
            FixMovieNames(mb, s);
            record.MovieBases.Add(mb);
        }

        #endregion Private Methods
    }
}