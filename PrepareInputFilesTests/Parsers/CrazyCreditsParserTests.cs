using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class CrazyCreditsParserTests
    {
        private CrazyCreditsParser _crazyCreditsParser;

        [TestInitialize]
        public void Setup()
        {
            _crazyCreditsParser = new CrazyCreditsParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\crazy-credits.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _crazyCreditsParser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\crazy-credits.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}