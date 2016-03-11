using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        protected IEnumerable<Match> GetRegExpMatches(string pattern)
        {
            var scan = new Regex(pattern);
            var lines = scan.Matches(File.ReadAllText(SourceFile))
                .Cast<Match>()
                .Where(m => m.Groups[1].Success);
            return lines;
        }

        protected string FixMovieNames(string nameInFile)
        {
            return nameInFile.Replace("\"", "").Trim();
        }

        #endregion Protected Methods

        #region Private Properties

        private StreamReader SourceFileStream { get; set; }

        #endregion Private Properties
    }
}