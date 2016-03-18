using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class ProdDesignerParserTests
    {
        private ProdDesignerParser _pdp;

        [TestInitialize]
        public void Setup()
        {
            _pdp = new ProdDesignerParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\production-designers.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _pdp.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\production-designers.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}