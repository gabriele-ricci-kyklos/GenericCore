using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GenericCore.Compression.LZW;
using System.Text;

namespace GenericCore.Test.Compression
{
    [TestClass]
    public class LZWTest
    {
        [TestMethod]
        public void TestLZW()
        {
            string inputStr = @"TOBEORNOTTOBEORTOBEORNOT";
            byte[] inputBytes = Encoding.ASCII.GetBytes(inputStr);

            byte[] compressed = LZW.Compress(inputBytes);
            byte[] decompressedBytes = LZW.Decompress(compressed);

            string decompressedStr = Encoding.ASCII.GetString(decompressedBytes);

            CollectionAssert.AreEqual(inputBytes, decompressedBytes);
            Assert.AreEqual(inputStr, decompressedStr);
        }
    }
}
