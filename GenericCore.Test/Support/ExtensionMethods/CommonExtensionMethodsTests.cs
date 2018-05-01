using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Support;

namespace GenericCore.Test.Support.ExtensionMethods
{
    [TestClass]
    public class CommonExtensionMethodsTests
    {
        [TestMethod]
        public void IsNullableTestMethod()
        {
            int a = 123;
            int? b = null;
            object c = new object();
            object d = null;
            int? e = 456;
            var f = (int?)789;

            Assert.IsFalse(a.IsNullable());
            Assert.IsTrue(b.IsNullable());
            Assert.IsFalse(c.IsNullable());
            Assert.IsTrue(d.IsNullable());
            Assert.IsTrue(e.IsNullable());
            Assert.IsTrue(f.IsNullable());
        }
    }
}
