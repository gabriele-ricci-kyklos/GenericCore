using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Serialization.Xml;
using GenericCore.Support;

namespace GenericCore.Test.Serialization
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestQuickXmlSerializer()
        {
            QuickXmlSerializerTestClass testObj = new QuickXmlSerializerTestClass { Value = "lol", Number = 1 };
            string xml = QuickXmlSerializer.GetXMLFromObject(testObj);
            Assert.IsNotNull(xml);

            QuickXmlSerializerTestClass deserializedObj = QuickXmlSerializer.ObjectToXML<QuickXmlSerializerTestClass>(xml);
            Assert.AreEqual(testObj, deserializedObj);
        }
    }

    public class QuickXmlSerializerTestClass
    {
        public string Value { get; set; }
        public int Number { get; set; }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj.IsNull())
            {
                return false;
            }

            QuickXmlSerializerTestClass typedObj = obj as QuickXmlSerializerTestClass;

            if (typedObj.IsNull())
            {
                return false;
            }

            return Value.Equals(typedObj.Value) && Number.Equals(typedObj.Number);
        }
    }
}
