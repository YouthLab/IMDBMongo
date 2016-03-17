using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrepareInputFiles;
using System.Collections.Generic;

namespace PrepareInputFilesTests
{
    [TestClass()]
    public class PrepareFilesTests
    {
        private PrepareFiles pf;

        [TestInitialize]
        public void Setup()
        {
            pf = new PrepareFiles()
            {
                CommandLineArguments = new List<string>()
                {
                    "-s",
                    System.IO.Path.GetTempPath(),
                    "-d",
                    @"c:\Users\Sridhar\Downloads\Delme\IMDB\",
                    "-f",
                    @"c:\users\sridhar\documents\visual studio 2013\Projects\IMDBMongo\FetchInputFiles\Data\FileList.txt"
                }
            };
        }

        [TestMethod]
        public void UnCompressFilesTest()
        {
            pf.ProcessCommandLineArguments();
            Assert.AreEqual(true, pf.UnCompressFiles());
        }
    }
}