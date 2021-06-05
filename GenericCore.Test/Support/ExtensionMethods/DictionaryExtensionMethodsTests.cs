using GenericCore.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GenericCore.Test.Support.ExtensionMethods
{
    [TestClass]
    public class DictionaryExtensionMethodsTests
    {
        [TestMethod]
        public void GetValuesOrDefaultTest()
        {
            Dictionary<int, string> dict =
                new Dictionary<int, string>
                {
                    { 1, "a" },
                    { 2, "b" },
                    { 3, "c" }
                };

            List<int> keys = new List<int> { 1, 3 };

            var results = dict.GetValuesOrDefault(keys);

            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results.ContainsKey(1));
            Assert.IsTrue(results.ContainsKey(3));
        }
    }
}
