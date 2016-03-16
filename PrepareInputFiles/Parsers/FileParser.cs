using Anotar.NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public abstract class FileParser : ImdbLineParser
    {
        protected List<string> RegularList { get; set; }
        protected string MovieIdentifier { get; set; }
        protected string ValueIdentifier { get; set; }

        protected void ReadRecords()
        {
            if (string.IsNullOrEmpty(SourceFile) || !File.Exists(SourceFile))
            {
                LogTo.Fatal("Did not find list file {0}", SourceFile);
                throw new Exception("Did not find list file " + SourceFile);
            }
            foreach (var lines in RegularList.Select(GetRegExpMatches))
            {
                PopulateRecords(lines);
            }
        }

        protected abstract void PopulateRecords(IEnumerable<Match> lines);
    }
}