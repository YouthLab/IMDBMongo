using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class SoundMixParserTests
    {
        private SoundMixParser _soundMixParser;

        [TestInitialize]
        public void Setup()
        {
            _soundMixParser = new SoundMixParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\sound-mix.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = _soundMixParser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\sound-mix.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}