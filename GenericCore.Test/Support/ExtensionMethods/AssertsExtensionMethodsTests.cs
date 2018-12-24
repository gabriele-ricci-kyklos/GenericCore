using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCore.Support;

namespace GenericCore.Test.Support.ExtensionMethods
{
    [TestClass]
    public class AssertsExtensionMethodsTests
    {
        [TestMethod]
        public void AssertsTest()
        {
            (1 == 1).AssertIsTrue("the expression is not true");
            (1 != 1).AssertIsFalse("the expression is not false");
            1.AssertNotNull("The value is null");
            "abc".AssertHasText("name");
            var list = new string[] { "a", "b", "c" };
            list.AssertHasElements(nameof(list));
            list.AssertHasElementsNotNull(nameof(list));
            list.AssertNotNullAndHasElements(nameof(list));
            list.AssertNotNullAndHasElementsNotNull(nameof(list));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertIsTrue()
        {
            false.AssertIsTrue("msg");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertIsFalse()
        {
            true.AssertIsFalse("msg");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFailAssertNotNull()
        {
            string x = null;
            x.AssertNotNull(nameof(x));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFailAssertHasText()
        {
            string x = "";
            x.AssertHasText(nameof(x));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertHasElements()
        {
            var list = new string[] { };
            list.AssertHasElements(nameof(list));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertHasElementsNotNull()
        {
            var list = new string[] { "1", null };
            list.AssertHasElementsNotNull(nameof(list));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertNotNullAndHasElements()
        {
            string[] list = null;
            list.AssertNotNullAndHasElements(nameof(list));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertNotNullAndHasElements2()
        {
            string[] list = new string[] { };
            list.AssertNotNullAndHasElements(nameof(list));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertNotNullAndHasElementsNotNull()
        {
            string[] list = null;
            list.AssertNotNullAndHasElementsNotNull(nameof(list));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFailAssertNotNullAndHasElementsNotNull2()
        {
            string[] list = new string[] { "1", null };
            list.AssertNotNullAndHasElementsNotNull(nameof(list));
        }
    }
}
