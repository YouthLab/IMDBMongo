using DataModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class TagLinesParser : FileParser
    {
        private readonly List<TagLine> _records;

        public TagLinesParser(string sourceFile)
        {
            SourceFile = sourceFile;
            HeaderLine = "-------------------------";
            PreHeaderLine1 = "";
            PreHeaderLine2 = "==========================";
            RegularList = new List<string>
            {
                @"(?s)(?<=\n)#.*?(?=\n\n)"
            };
            MovieIdentifier = "# ";
            ValueIdentifier = "- ";
            _records = new List<TagLine>();
        }

        public override bool ParseFile(string destinationFile)
        {
            throw new System.NotImplementedException();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            throw new System.NotImplementedException();
        }
    }
}