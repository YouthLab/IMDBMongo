using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class CertificatesParserTests
    {
        private CertificatesParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = new CertificatesParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\certificates.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _parser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\certificates.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}