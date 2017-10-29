using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Support.Web;
using System.IO;

namespace GenericCore.Test.Support.Web
{
    [TestClass]
    public class WebTests
    {
        [TestMethod]
        public void TestPage()
        {
            string html = WebPageDataRetriever.RetrievePage("https://google.it/");
            Assert.IsNotNull(html);
        }

        [TestMethod]
        public void TestFile()
        {
            string localPath = @"C:\temp\master.zip";
            bool succeded = WebPageDataRetriever.DownloadFile("https://github.com/gabriele-ricci-kyklos/FantasyFootballStatistics/archive/master.zip", localPath);
            Assert.IsTrue(succeded);
            Assert.IsTrue(File.Exists(localPath));
        }
    }
}
