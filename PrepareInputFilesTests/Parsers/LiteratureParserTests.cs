using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class LiteratureParserTests
    {
        private LiteratureParser _literatureParser;

        [TestInitialize]
        public void Setup()
        {
            _literatureParser = new LiteratureParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\literature.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parserResult = _literatureParser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\literature.json");
            Assert.AreEqual(true, parserResult);
        }
    }
}