using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class CostumeDesignerParserTests
    {
        private CostumeDesignerParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = new CostumeDesignerParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\costume-designers.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _parser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\costume-designers.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}