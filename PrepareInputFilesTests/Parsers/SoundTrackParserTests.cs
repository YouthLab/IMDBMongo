using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles.Parsers;

namespace PrepareInputFilesTests.Parsers
{
    [TestClass()]
    public class SoundTrackParserTests
    {
        private SoundTrackParser soundTrackParser;

        [TestInitialize]
        public void Setup()
        {
            soundTrackParser = new SoundTrackParser(@"c:\Users\Sridhar\Downloads\Delme\IMDB\soundtracks.list");
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            var parseResult = soundTrackParser.ParseFile(@"c:\Users\Sridhar\Downloads\Delme\IMDB\soundtracks.json");
            Assert.AreEqual(true, parseResult);
        }
    }
}