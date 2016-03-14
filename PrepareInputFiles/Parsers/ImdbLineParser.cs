using DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public abstract class ImdbLineParser : IDisposable
    {
        #region Public Methods

        public void Dispose()
        {
            if (SourceFileStream != null)
                SourceFileStream.Dispose();
        }

        public abstract bool ParseFile(string destinationFile);

        #endregion Public Methods

        #region Protected Properties

        protected string HeaderLine { get; set; }
        protected string PreHeaderLine1 { get; set; }
        protected string PreHeaderLine2 { get; set; }
        protected string SourceFile { get; set; }

        #endregion Protected Properties

        #region Protected Constructors

        protected ImdbLineParser()
        {
            SourceFileStream = null;
        }

        #endregion Protected Constructors

        #region Protected Methods

        protected void FixMovieNames(MovieBase movieBase, string nameInFile)
        {
            var cleanStep1 = nameInFile.Replace("\"", "").Trim();
            var scan = new Regex(@"\([0-9][0-9][0-9][0-9]\)");
            cleanStep1 = ExtractYear(movieBase, scan, cleanStep1);
            scan = new Regex(@"{.*}");
            cleanStep1 = ExtractEpisode(movieBase, scan, cleanStep1);
            movieBase.MovieName = cleanStep1.Trim();
            return;
        }

        protected IEnumerable<Match> GetRegExpMatches(string pattern)
        {
            var scan = new Regex(pattern);
            var retValue = new List<Match>();

            var lines = scan.Match(File.ReadAllText(SourceFile));

            while (lines.Success)
            {
                retValue.Add(lines);
                lines = lines.NextMatch();
            }
            return retValue;
            //var lines = scan.Matches(File.ReadAllText(SourceFile))
            //    .Cast<Match>()
            //    .Where(m => m.Groups[1].Success);
            //return lines;
        }

        #endregion Protected Methods

        #region Private Properties

        private StreamReader SourceFileStream { get; set; }

        #endregion Private Properties

        #region Private Methods

        private string ExtractEpisode(MovieBase movieBase, Regex scan, string inputString)
        {
            var episode = scan.Match(inputString);
            if (!episode.Success) return inputString;
            var episodeString = episode.Value.Replace("{", "").Replace("}", "");
            movieBase.Episode = episodeString;
            inputString = scan.Replace(inputString, "");
            return inputString;
        }

        private string ExtractYear(MovieBase movieBase, Regex scan, string inputString)
        {
            var year = scan.Match(inputString);
            if (!year.Success) return inputString;
            var yearString = year.Value.Replace("(", "").Replace(")", "");
            int yearNum;
            var result = int.TryParse(yearString, out yearNum);
            if (result)
                movieBase.Year = yearNum;
            else
                movieBase.Year = null;
            inputString = scan.Replace(inputString, "");
            return inputString;
        }

        #endregion Private Methods
    }
}