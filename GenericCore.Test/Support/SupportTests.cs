using GenericCore.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericCore.Test.Support.Strings
{
    [TestClass]
    public class SupportTests
    {
        [TestMethod]
        public void IOUtilities_EmptyFolder_Test()
        {
            IOUtilities.EmptyFolder(@"C:\temp");
        }

        [TestMethod]
        public void IOUtilities_CopyDirectory_Test()
        {
            IOUtilities.CopyFolderTo(@"C:\temp\folder1", @"C:\temp\folder2", true, true);
        }
    }
}
