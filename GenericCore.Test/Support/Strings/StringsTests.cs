using GenericCore.Support.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Test.Support.Strings
{
    [TestClass]
    public class StringsTests
    {
        [TestMethod]
        public void BinaryStringTest()
        {
            BinaryString str = new BinaryString("hello world");
            BinaryString str2 = new BinaryString("hello world");

            Assert.AreEqual(str, str2);
            Assert.AreEqual("0110100001100101011011000110110001101111001000000111011101101111011100100110110001100100", str.ToString());
        }
    }
}
