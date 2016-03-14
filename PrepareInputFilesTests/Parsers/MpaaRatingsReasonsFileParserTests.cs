using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class MpaaRatingsReasonsFileParserTests
    {
        private MpaaRatingsReasonsFileParser mrrfp;

        [TestInitialize]
        public void Setup()
        {
            mrrfp = new MpaaRatingsReasonsFileParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\mpaa-ratings-reasons.list");
        }

        [TestMethod]
        public void ParseFileTest()
        {
            //arrange
            //act
            var parseResult = mrrfp.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\mpaa-ratings-reasons.json");
            //assert
            Assert.IsTrue(parseResult);
        }
    }
}