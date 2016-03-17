using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class TagLinesParserTests
    {
        private TagLinesParser _tagLinesParser;

        [TestInitialize]
        public void Setup()
        {
            _tagLinesParser = new TagLinesParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\taglines.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _tagLinesParser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\taglines.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}