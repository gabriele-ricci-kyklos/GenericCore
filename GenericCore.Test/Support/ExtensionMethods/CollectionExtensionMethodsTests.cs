﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Support;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GenericCore.Test.Support.ExtensionMethods
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

        private IList<Item> GenerateTestList()
        {
            return
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
        }

        [TestMethod]
        public void TestToTupleGeneratesNotNullListAndWithSameLengthOneTypes()
        {
            IList<Item> list = GenerateTestList();
            IList<Tuple<string>> tupleList = list.ToTupleList(x => x.Code);
            Assert.IsNotNull(tupleList);
            Assert.IsTrue(tupleList.Count == list.Count);
        }

        [TestMethod]
        public void TestToTupleGeneratesNotNullListAndWithSameLengthTwoTypes()
        {
            IList<Item> list = GenerateTestList();
            IList<Tuple<long, string>> tupleList = list.ToTupleList(x => x.Id, x => x.Code);
            Assert.IsNotNull(tupleList);
            Assert.IsTrue(tupleList.Count == list.Count);
        }

        [TestMethod]
        public void TestToTupleGeneratesNotNullListAndWithSameLengthThreeTypes()
        {
            IList<Item> list = GenerateTestList();
            IList<Tuple<long, string, bool>> tupleList = list.ToTupleList(x => x.Id, x => x.Code, x => x.IsActive);
            Assert.IsNotNull(tupleList);
            Assert.IsTrue(tupleList.Count == list.Count);
        }

        [TestMethod]
        public void TestSelectFirst()
        {
            IList<Item> list = GenerateTestList();
            string firstCode = list.SelectFirst(x => x.Code);
            Assert.IsNotNull(firstCode);
            Assert.IsTrue(firstCode == list.First().Code);
        }

        [TestMethod]
        public void TestSelectLast()
        {
            IList<Item> list = GenerateTestList();
            string lastCode = list.SelectLast(x => x.Code);
            Assert.IsNotNull(lastCode);
            Assert.IsTrue(lastCode == list.Last().Code);
        }

        [TestMethod]
        public void TestToDictionary()
        {
            var list = GenerateTestList();
            var dict = 
                list
                    .Select(x => new KeyValuePair<long, string>(x.Id, x.Code))
                    .ToDictionary();

            Assert.IsInstanceOfType(dict, typeof(IDictionary<long, string>));
        }
    }

    public static class CollectionExtensionMethodsTestsExtMethods
    {
    }
}