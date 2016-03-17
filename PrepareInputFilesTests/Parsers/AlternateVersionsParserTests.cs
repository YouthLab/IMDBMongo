using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class AlternateVersionsParserTests
    {
        private AlternateVersionsParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = new AlternateVersionsParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\alternate-versions.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _parser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\alternate-versions.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}