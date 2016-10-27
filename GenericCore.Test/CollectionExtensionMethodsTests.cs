using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Support;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GenericCore.Test
{
    [TestClass]
    public class CollectionExtensionMethodsTests
    {
        class Item
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Descr { get; set; }
            public bool IsActive { get; set; }
        }

        [TestMethod]
        public void TestToTupleGeneratesNotNullListAndWithSameLengthOneTypes()
        {
            IList<Item> list =
                new List<Item>
                {
                    new Item
                    {
                        Id = 1,
                        Code = "A",
                        Descr = "A letter",
                        IsActive = true
                    },
                    new Item
                    {
                        Id = 2,
                        Code = "B",
                        Descr = "B letter",
                        IsActive = true
                    },
                    new Item
                    {
                        Id = 3,
                        Code = "C",
                        Descr = "C letter",
                        IsActive = true
                    }
                };

            IList<Tuple<string>> tupleList = list.ToTupleList(x => x.Code);
            Assert.IsNotNull(tupleList);
            Assert.IsTrue(tupleList.Count == list.Count);
        }

        [TestMethod]
        public void TestToTupleGeneratesNotNullListAndWithSameLengthTwoTypes()
        {
            IList<Item> list =
                new List<Item>
                {
                    new Item
                    {
                        Id = 1,
                        Code = "A",
                        Descr = "A letter",
                        IsActive = true
                    },
                    new Item
                    {
                        Id = 2,
                        Code = "B",
                        Descr = "B letter",
                        IsActive = true
                    },
                    new Item
                    {
                        Id = 3,
                        Code = "C",
                        Descr = "C letter",
                        IsActive = true
                    }
                };

            IList<Tuple<long, string>> tupleList = list.ToTupleList(x => x.Id, x => x.Code);
            Assert.IsNotNull(tupleList);
            Assert.IsTrue(tupleList.Count == list.Count);
        }

        [TestMethod]
        public void TestToTupleGeneratesNotNullListAndWithSameLengthThreeTypes()
        {
            IList<Item> list =
                new List<Item>
                {
                    new Item
                    {
                        Id = 1,
                        Code = "A",
                        Descr = "A letter",
                        IsActive = true
                    },
                    new Item
                    {
                        Id = 2,
                        Code = "B",
                        Descr = "B letter",
                        IsActive = true
                    },
                    new Item
                    {
                        Id = 3,
                        Code = "C",
                        Descr = "C letter",
                        IsActive = true
                    }
                };

            IList<Tuple<long, string, bool>> tupleList = list.ToTupleList(x => x.Id, x => x.Code, x => x.IsActive);
            Assert.IsNotNull(tupleList);
            Assert.IsTrue(tupleList.Count == list.Count);
        }
    }

    public static class CollectionExtensionMethodsTestsExtMethods
    {
    }
}
