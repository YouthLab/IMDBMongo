using FetchInputFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FetchInputFilesTests
{
    [TestClass()]
    public class FetchFilesTests
    {
        private FetchFiles ff;

        [TestInitialize]
        public void Setup()
        {
            ff = new FetchFiles()
            {
                CommandLineArguments = new List<string>()
                {
                    "-d",
                    System.IO.Path.GetTempPath(),
                    "-u",
                    "ftp://ftp.funet.fi/pub/mirrors/ftp.imdb.com/pub/",
                    "-f",
                    @"c:\users\sridhar\documents\visual studio 2013\Projects\IMDBMongo\FetchInputFiles\Data\FileList.txt"
                }
            };
        }

        [TestMethod()]
        public void ProcessCommandLineArgumentsTest()
        {
            ff.ProcessCommandLineArguments();
            Assert.AreEqual(ff.CommandLineOpts.SourceUrl, @"ftp://ftp.funet.fi/pub/mirrors/ftp.imdb.com/pub/");
        }

        [TestMethod]
        public void CheckExternalFilesDownloadedTest()
        {
            ff.ProcessCommandLineArguments();
            var count = ff.GetNumberOfFilesDownloaded();
            Assert.AreEqual(count, 35);
        }
    }
}