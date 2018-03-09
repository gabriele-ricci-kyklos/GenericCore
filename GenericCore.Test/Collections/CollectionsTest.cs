using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GenericCore.Collections;
using System.Linq;

namespace GenericCore.Test.Collections
{
    [TestClass]
    public class CollectionsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = GetTestList();

            var ignoreNullValuesGroups = list.GroupBy(x => new IgnoreNullValuesKey(x.Name));
            var considerNullValuesGroups = list.GroupBy(x => new ConsiderNullValuesKey(x.Name));

            Assert.IsTrue(ignoreNullValuesGroups.Count() == 2);
            Assert.IsTrue(considerNullValuesGroups.Count() == 3);
        }

        static List<GenericItem> GetTestList() => new List<GenericItem> { new GenericItem { ID = 1, Name = null, Value = "1" }, new GenericItem { ID = 1, Name = null, Value = "2" }, new GenericItem { ID = 2, Name = "c", Value = "3" } };
    }

    class IgnoreNullValuesKey : GenericGroupKey
    {
        public string Name { get; }

        public IgnoreNullValuesKey(string name)
            : base(true)
        {
            Name = name;
        }
    }

    class ConsiderNullValuesKey : GenericGroupKey
    {
        public string Name { get; }

        public ConsiderNullValuesKey(string name)
            : base(false)
        {
            Name = name;
        }
    }

    class GenericItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
