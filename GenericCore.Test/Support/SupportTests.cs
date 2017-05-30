using GenericCore.Support;
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
    public class SupportTests
    {
        [TestMethod]
        public void CounterTest()
        {
            Assert.AreEqual(1, Counter.Next);
            Assert.AreEqual(2, Counter.Next);
            Assert.AreEqual(3, Counter.Next);

            Assert.AreEqual(3, Counter.Last);
        }
    }
}
