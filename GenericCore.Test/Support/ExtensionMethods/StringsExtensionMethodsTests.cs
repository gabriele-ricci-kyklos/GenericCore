using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCore.Support;

namespace GenericCore.Test.Support.ExtensionMethods
{
    [TestClass]
    public class StringsExtensionMethodsTests
    {
        [TestMethod]
        public void TestStringAppend()
        {
            string str = "lol";
            string token = "asd";
            string separator = "-";
            string str2 = str.StringAppend(token);
            string str3 = str.StringAppend(token, separator);
            
            Assert.IsNotNull(str2);
            Assert.IsNotNull(str3);
            Assert.IsTrue(str2 == "{0}{1}".FormatWith(str, token));
            Assert.IsTrue(str3 == "{0}{1}{2}".FormatWith(str, separator, token));
        }

        [TestMethod]
        public void TestShuffle()
        {
            string s = "abcdef";
            string shuffled = s.Shuffle();

            Assert.AreNotEqual(s, shuffled);
        }
    }
}
