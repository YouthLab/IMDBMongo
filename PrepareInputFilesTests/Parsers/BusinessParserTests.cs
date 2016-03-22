using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class BusinessParserTests
    {
        private BusinessParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = new BusinessParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\business.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _parser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\business.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}