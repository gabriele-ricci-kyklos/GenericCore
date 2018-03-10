using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GenericCore.Collections;
using System.Linq;
using GenericCore.Support;

namespace GenericCore.Test.Collections
{
    [TestClass]
    public class CollectionsTest
    {
        [TestMethod]
        public void GroupKeyTest()
        {
            var list = GetTestList();

            var ignoreNullValuesGroups = list.GroupBy(x => new IgnoreNullValuesKey(x.Name));
            var considerNullValuesGroups = list.GroupBy(x => new ConsiderNullValuesKey(x.Name));

            Assert.IsTrue(ignoreNullValuesGroups.Count() == 2);
            Assert.IsTrue(considerNullValuesGroups.Count() == 3);
        }

        [TestMethod]
        public void SafeElementAtTests()
        {
            var list = GetTestList();
            var elementNotFound = list.SafeElementAt(15, new GenericItem { ID = 0 });
            var elementFound = list.SafeElementAt(0, new GenericItem { ID = 0 });

            Assert.IsTrue(elementNotFound.ID == 0);
            Assert.AreEqual(elementFound, list[0]);
        }

        [TestMethod]
        public void SafeGetValueTests()
        {
            var list = GetTestList();
            var elementNotFound = list.SafeGetValue(15, x => x.ID, 0);
            var elementFound = list.SafeGetValue(0, x => x.ID, 0);

            Assert.IsTrue(elementNotFound == 0);
            Assert.AreEqual(elementFound, list[0].ID);
        }

        [TestMethod]
        public void DistinctByTests()
        {
            var list = GetTestList();
            var distinct = list.DistinctBy(x => x.ID);

            Assert.IsTrue(distinct.Count() == 2);
            Assert.IsTrue(distinct.First().ID == 1);
            Assert.IsTrue(distinct.ElementAt(1).ID == 2);
            Assert.IsTrue(distinct.First().Value == "1");
            Assert.IsTrue(distinct.ElementAt(1).Value == "3");
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
