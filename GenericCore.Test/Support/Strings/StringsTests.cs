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
            BinaryString str = BinaryString.FromString("hello world");
            BinaryString str2 = BinaryString.FromString("hello world");

            Assert.AreEqual(str, str2);
            Assert.AreEqual("0110100001100101011011000110110001101111001000000111011101101111011100100110110001100100", str.ToString());

            BinaryString str3 = BinaryString.FromString("a");

            Assert.AreEqual("01100001", str3.ToString());

            BinaryString invertedStr3 = str3.InvertBinaries();
            Assert.AreEqual("10011110", invertedStr3.ToString());

            BinaryString reversedStr3 = str3.ReverseBinaries();
            Assert.AreEqual("10000110", reversedStr3.ToString());
        }
    }
}
