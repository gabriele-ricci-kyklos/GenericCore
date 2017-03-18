using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Cryptography;

namespace GenericCore.Test.Cryptography
{
    [TestClass]
    public class CryptographyTests
    {
        [TestMethod]
        public void GenericEncryptionTest()
        {
            string str = "ciaooo";
            string encoded = GenericEncryption.FromString(str, 50);
            string decoded = GenericEncryption.FromEncryptedString(encoded);

            Assert.AreEqual(str, decoded);
        }
    }
}
