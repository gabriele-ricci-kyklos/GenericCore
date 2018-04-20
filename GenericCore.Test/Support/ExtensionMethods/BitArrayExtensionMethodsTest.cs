using GenericCore.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Test.Support.ExtensionMethods
{
    [TestClass]
    public class BitArrayExtensionMethodsTest
    {
        [TestMethod]
        public void TestToByteArray()
        {
            BitArray bits = new BitArray(Enumerable.Range(0, 1000000).ToArray());
            byte[] bytes = bits.ToByteArray();

            Assert.IsTrue(bytes.Length == (bits.Length / 8));
        }

        [TestMethod]
        public void TestToIntArray()
        {
            int[] input = new int[] { 1, 2, 3 };
            BitArray bits = new BitArray(input);
            int[] ints = bits.ToIntArray();

            Assert.IsTrue(ints.Length == input.Length);
            Assert.AreEqual(input[0], ints[0]);
            Assert.AreEqual(input[1], ints[1]);
            Assert.AreEqual(input[2], ints[2]);
        }
    }
}
